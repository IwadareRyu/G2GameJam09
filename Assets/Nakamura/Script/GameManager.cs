using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hikanyan.Core;
using UnityEngine.Rendering.Universal;

public enum GameState
{
    None = 0,
    Start,
    InGame,
    Result,
}

public class GameManager : AbstractSingleton<GameManager>
{
    [Tooltip("現在のゲームステート")]
    [SerializeField] private GameState NowGameState = GameState.None;
    [Tooltip("InGameから遷移するシーンの名前を設定")]
    [SerializeField] private string _InGameTOresult = "Result";
    [Tooltip("Resultから遷移するシーンの名前を設定")]
    [SerializeField] private string _resultTOinGame = "InGame";
    [Tooltip("ゲーム内のスコア")]
    [SerializeField] private int _score = 0;
    [Tooltip("ゲーム開始判定（ゲーム中の時はTrue）")]
    public bool is_Game = false;
    [Tooltip("ゲームクリア判定（クリア時にTrue）")]
    public bool is_Clear = false;
    [Tooltip("制限時間")]
    [SerializeField] private float _time = 180;
    [Tooltip("実際の計算に用いるタイマー変数")]
    private float _timeValue;
    [Tooltip("制限時間を入れるText")]
    [SerializeField] private Text _timeText;
    [Tooltip("スコアを入れるText")]
    [SerializeField] private Text _scoreText;
    [Tooltip("リザルト時にスコアをいれるText")]
    [SerializeField] private Text _resultScoreText;
    [Tooltip("Light")]
    private Light2D _light = null;
    [Tooltip("暗くする速度を調整するカウント変数")]
    [SerializeField] private float _lightCT = 3f;
    [Tooltip("暗くする速度")]
    [SerializeField] private float _douwLight = 0.01f;
    private float _holdCT = 0;

    public void LoadProssesing()
    {
        switch (NowGameState)
        {
            case GameState.Start:
                if (_light == null)
                {//Lightを持っていない場合に取ってくる
                    _light = GetComponentInChildren<Light2D>();
                }
                _timeText.enabled = true;
                _scoreText.enabled = true;
                _holdCT = _lightCT;
                //Audiの再生
                CRIAudioManager.Instance.CriBgmPlay(0);
                //スコア初期化処理
                _score = 0;
                //タイマーの初期化
                _timeValue = _time;
                //Textの初期化
                _timeText.text = _time.ToString("000");
                _scoreText.text = _score.ToString("00000");
                break;

            case GameState.Result:
                if (_light == null)
                {//Lightを持っていない場合に取ってくる
                    _light = GetComponentInChildren<Light2D>();
                }
                //ライトの明るさを戻す
                _light.intensity = 1;
                _resultScoreText.text = _score.ToString("00000");
                //Audiの再生
                CRIAudioManager.Instance.CriBgmPlay(1);
                is_Game = false;
                is_Clear = false;
                break;
        }
    }

    private void Update()
    {
        switch (NowGameState)
        {
            case GameState.Start:
                if (!is_Game && Input.GetKeyDown(KeyCode.Space))
                {//スペースキーが押されたらゲームスタート
                    is_Game = true;
                    NowGameState = GameState.InGame;
                }
                break;

            case GameState.InGame:
                if (!is_Game)
                {
                    return;
                }
                else
                {//ゲームが開始された時の処理
                    if (_timeText != null)
                    {
                        _lightCT -= Time.deltaTime;
                        _timeValue -= Time.deltaTime;
                        _timeText.text = _timeValue.ToString("000");

                        if (_lightCT <= 0)   
                        {
                            _lightCT = _holdCT;
                            if (_light != null) { _light.intensity -= _douwLight; }
                        }
                    }
                }
                break;

            case GameState.Result:
                if (Input.GetKeyDown(KeyCode.Return)) { SceneChange(_resultTOinGame); }
                break;
        }
    }

    public void ScoreValue (int value)
    {
        //スコアを変動させ、テキストを更新
        _score += value;
        _scoreText.text = _score.ToString("000000");
    }

    public void ClearCalculaton()
    {
        is_Clear = true;
        is_Game = false;
        _score += (int)_timeValue * 10;
        SceneChange(_InGameTOresult);
    }

    public void SceneChange(string scene)
    {//シーン遷移処理
        //遷移先のシーンに合わせてステートを変更
        if (scene == "InGame") 
        {
            NowGameState = GameState.Start; 
        }
        else if (scene == "Result")
        {
            NowGameState = GameState.Result;
            is_Game = false;
            is_Clear = false;
            _scoreText.enabled = false;
            _timeText.enabled = false;
        }

        SceneManager.LoadScene(scene);
        LoadProssesing();
    }
}

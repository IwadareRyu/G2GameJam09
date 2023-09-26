using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hikanyan.Core;
using Unity.VisualScripting;

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

    public void LoadProssesing()
    {
        Debug.Log("遷移");
        Debug.Log($"Start処理 {NowGameState}");
        switch (NowGameState)
        {
            case GameState.Start:
                //スコア初期化処理
                _score = 0;
                //タイマーの初期化
                _timeValue = _time;
                //Textの初期化
                _timeText.text = _time.ToString("000");
                _scoreText.text = _score.ToString("00000");
                break;

            case GameState.Result:
                //Debug.Log(NowGameState);
                _resultScoreText.text = _score.ToString("00000");
                is_Game = false;
                is_Clear = false;
                //Debug.Log("result");
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
                        _timeValue -= Time.deltaTime;
                        _timeText.text = _timeValue.ToString("000");
                    }
                }
                break;
        }
    }

    public void ScoreValue (int value)
    {
        //スコアを変動させ、テキストを更新
        _score += value;
        _scoreText.text = _score.ToString("000000");
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
            Debug.Log(is_Game);
        }

        Debug.Log(NowGameState);
        SceneManager.LoadScene(scene);
        LoadProssesing();
    }
}

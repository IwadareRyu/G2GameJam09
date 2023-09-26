using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hikanyan.Core;
using Unity.VisualScripting;

public enum GameState
{
    Start,
    InGame,
    Result,
}

public class GameManager : AbstractSingleton<GameManager>
{
    [Tooltip("現在のゲームステート")]
    [SerializeField] static public GameState NowGameState = GameState.InGame;
    [Tooltip("ゲーム内のスコア")]
    [SerializeField] private static int _score = 0;
    [Tooltip("InGame スコア表示用のテキストＵＩ")]
    [SerializeField] private GameObject _scoreTextUI;
    [Tooltip("InGame 操作説明用のpanel")]
    [SerializeField] private GameObject _tutorialPanelUI;
    [Tooltip("InGame 制限時間表示用のテキストUI")]
    [SerializeField] private GameObject _timeTextUI;
    [Tooltip("Rexult スコアを表示するテキストUI")]
    [SerializeField] private GameObject _resultScoreTextUI;
    [Tooltip("ゲーム開始判定（ゲーム中の時はTrue）")]
    public bool is_Game = false;
    [Tooltip("ゲームクリア判定（クリア時にTrue）")]
    public bool is_Clear = false;
    [Tooltip("制限時間")]
    [SerializeField] private float _time = 180;
    [Tooltip("制限時間を入れるText")]
    private Text _timeText;
    [Tooltip("スコアを入れるText")]
    private Text _scoreText;
    [Tooltip("リザルト時にスコアをいれるText")]
    private Text _resultScoreText;

    private void Start()
    {
        switch (NowGameState)
        {
            case GameState.Start:
                _score = 0;
                _timeText = _timeTextUI.GetComponent<Text>();
                _scoreText = _scoreTextUI.GetComponent<Text>();
                _tutorialPanelUI.SetActive(true);
                break;

            case GameState.Result:
                _resultScoreText = _resultScoreTextUI.GetComponent<Text>();
                _resultScoreText.text = _score.ToString("00000");
                break;
        }
    }

    private void Update()
    {
        switch (NowGameState)
        {
            case GameState.InGame:
                if (is_Game)
                {
                    _time -= Time.deltaTime;
                    _timeText.text = _time.ToString("000");
                }

                if (!is_Game && Input.GetKeyDown(KeyCode.Space))
                {
                    is_Game = true;
                }
                break;
        }
    }

    public void ScoreValue (int value)
    {
        _score += value;
        _scoreText.text = _score.ToString("000000");
    }

    public void SceneChange(string scene)
    {
        if (scene == "InGame") { NowGameState = GameState.Start; }
        else if (scene == "Result") { NowGameState = GameState.Result; }

        SceneManager.LoadScene(scene);
    }
}

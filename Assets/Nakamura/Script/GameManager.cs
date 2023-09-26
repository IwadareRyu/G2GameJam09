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
    [SerializeField] static public GameState GameState = GameState.InGame;
    [Tooltip("ゲーム内のスコア")]
    [SerializeField] private static int _score = 0;
    [Tooltip("スコア表示用のテキストＵＩ")]
    [SerializeField] private GameObject _scoreTextUI;
    [Tooltip("操作説明用のpanel")]
    [SerializeField] private GameObject _tutorialPanelUI;
    [Tooltip("制限時間表示用のテキストUI")]
    [SerializeField] private GameObject _timeTextUI;
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

    private void Start()
    {
        switch (GameState)
        {
            case GameState.InGame:
                _timeText = _timeTextUI.GetComponent<Text>();
                _scoreText = _scoreTextUI.GetComponent<Text>();
                _tutorialPanelUI.SetActive(true);
                break;
        }
    }

    private void Update()
    {
        switch (GameState)
        {
            case GameState.InGame:
                _time -= Time.deltaTime;
                _timeText.text = _time.ToString("000");
                break;
        }
    }

    public void ScoreValue (int value)
    {
        _score += value;
        _scoreText.text = _score.ToString("000000");
    }
}

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
    [Tooltip("���݂̃Q�[���X�e�[�g")]
    [SerializeField] static public GameState NowGameState = GameState.InGame;
    [Tooltip("�Q�[�����̃X�R�A")]
    [SerializeField] private static int _score = 0;
    [Tooltip("InGame �X�R�A�\���p�̃e�L�X�g�t�h")]
    [SerializeField] private GameObject _scoreTextUI;
    [Tooltip("InGame ��������p��panel")]
    [SerializeField] private GameObject _tutorialPanelUI;
    [Tooltip("InGame �������ԕ\���p�̃e�L�X�gUI")]
    [SerializeField] private GameObject _timeTextUI;
    [Tooltip("Rexult �X�R�A��\������e�L�X�gUI")]
    [SerializeField] private GameObject _resultScoreTextUI;
    [Tooltip("�Q�[���J�n����i�Q�[�����̎���True�j")]
    public bool is_Game = false;
    [Tooltip("�Q�[���N���A����i�N���A����True�j")]
    public bool is_Clear = false;
    [Tooltip("��������")]
    [SerializeField] private float _time = 180;
    [Tooltip("�������Ԃ�����Text")]
    private Text _timeText;
    [Tooltip("�X�R�A������Text")]
    private Text _scoreText;
    [Tooltip("���U���g���ɃX�R�A�������Text")]
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

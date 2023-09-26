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
    [SerializeField] static public GameState GameState = GameState.InGame;
    [Tooltip("�Q�[�����̃X�R�A")]
    [SerializeField] private static int _score = 0;
    [Tooltip("�X�R�A�\���p�̃e�L�X�g�t�h")]
    [SerializeField] private GameObject _scoreTextUI;
    [Tooltip("��������p��panel")]
    [SerializeField] private GameObject _tutorialPanelUI;
    [Tooltip("�������ԕ\���p�̃e�L�X�gUI")]
    [SerializeField] private GameObject _timeTextUI;
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

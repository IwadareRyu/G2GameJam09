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
    [Tooltip("���݂̃Q�[���X�e�[�g")]
    [SerializeField] private GameState NowGameState = GameState.None;
    [Tooltip("�Q�[�����̃X�R�A")]
    [SerializeField] private int _score = 0;
    [Tooltip("�Q�[���J�n����i�Q�[�����̎���True�j")]
    public bool is_Game = false;
    [Tooltip("�Q�[���N���A����i�N���A����True�j")]
    public bool is_Clear = false;
    [Tooltip("��������")]
    [SerializeField] private float _time = 180;
    [Tooltip("���ۂ̌v�Z�ɗp����^�C�}�[�ϐ�")]
    private float _timeValue;
    [Tooltip("�������Ԃ�����Text")]
    [SerializeField] private Text _timeText;
    [Tooltip("�X�R�A������Text")]
    [SerializeField] private Text _scoreText;
    [Tooltip("���U���g���ɃX�R�A�������Text")]
    [SerializeField] private Text _resultScoreText;

    public void LoadProssesing()
    {
        Debug.Log("�J��");
        Debug.Log($"Start���� {NowGameState}");
        switch (NowGameState)
        {
            case GameState.Start:
                //�X�R�A����������
                _score = 0;
                //�^�C�}�[�̏�����
                _timeValue = _time;
                //Text�̏�����
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
                {//�X�y�[�X�L�[�������ꂽ��Q�[���X�^�[�g
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
                {//�Q�[�����J�n���ꂽ���̏���
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
        //�X�R�A��ϓ������A�e�L�X�g���X�V
        _score += value;
        _scoreText.text = _score.ToString("000000");
    }

    public void SceneChange(string scene)
    {//�V�[���J�ڏ���
        //�J�ڐ�̃V�[���ɍ��킹�ăX�e�[�g��ύX
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

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
    [Tooltip("���݂̃Q�[���X�e�[�g")]
    [SerializeField] private GameState NowGameState = GameState.None;
    [Tooltip("InGame����J�ڂ���V�[���̖��O��ݒ�")]
    [SerializeField] private string _InGameTOresult = "Result";
    [Tooltip("Result����J�ڂ���V�[���̖��O��ݒ�")]
    [SerializeField] private string _resultTOinGame = "InGame";
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
    [Tooltip("Light")]
    private Light2D _light = null;
    [Tooltip("�Â����鑬�x�𒲐�����J�E���g�ϐ�")]
    [SerializeField] private float _lightCT = 3f;
    [Tooltip("�Â����鑬�x")]
    [SerializeField] private float _douwLight = 0.01f;
    private float _holdCT = 0;

    public void LoadProssesing()
    {
        switch (NowGameState)
        {
            case GameState.Start:
                if (_light == null)
                {//Light�������Ă��Ȃ��ꍇ�Ɏ���Ă���
                    _light = GetComponentInChildren<Light2D>();
                }
                _timeText.enabled = true;
                _scoreText.enabled = true;
                _holdCT = _lightCT;
                //Audi�̍Đ�
                CRIAudioManager.Instance.CriBgmPlay(0);
                //�X�R�A����������
                _score = 0;
                //�^�C�}�[�̏�����
                _timeValue = _time;
                //Text�̏�����
                _timeText.text = _time.ToString("000");
                _scoreText.text = _score.ToString("00000");
                break;

            case GameState.Result:
                if (_light == null)
                {//Light�������Ă��Ȃ��ꍇ�Ɏ���Ă���
                    _light = GetComponentInChildren<Light2D>();
                }
                //���C�g�̖��邳��߂�
                _light.intensity = 1;
                _resultScoreText.text = _score.ToString("00000");
                //Audi�̍Đ�
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
        //�X�R�A��ϓ������A�e�L�X�g���X�V
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
            _scoreText.enabled = false;
            _timeText.enabled = false;
        }

        SceneManager.LoadScene(scene);
        LoadProssesing();
    }
}

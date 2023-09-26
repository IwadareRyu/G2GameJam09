using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hikanyan.Core;

public class GameManager : AbstractSingleton<GameManager>
{
    [Tooltip("�Q�[�����̃X�R�A")]
    [SerializeField] private static int _score = 0;
    public int Score { get { return _score; } set {  _score = value; } }

    [Tooltip("�X�R�A�\���p�̃e�L�X�g�t�h")]
    [SerializeField] private GameObject _scoreText;
    [Tooltip("��������p��panel")]
    [SerializeField] private GameObject _tutorialPanel;
    /// <summary>�Q�[���J�n����i�Q�[�����̎���True�j</summary>
    public bool is_Game = false;
    /// <summary>�Q�[���N���A����i�N���A����True�j</summary>
    public bool is_Clear = false;
}

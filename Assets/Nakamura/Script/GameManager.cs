using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hikanyan.Core;

public class GameManager : AbstractSingleton<GameManager>
{
    [Tooltip("ゲーム内のスコア")]
    [SerializeField] private static int _score = 0;
    public int Score { get { return _score; } set {  _score = value; } }

    [Tooltip("スコア表示用のテキストＵＩ")]
    [SerializeField] private GameObject _scoreText;
    [Tooltip("操作説明用のpanel")]
    [SerializeField] private GameObject _tutorialPanel;
    /// <summary>ゲーム開始判定（ゲーム中の時はTrue）</summary>
    public bool is_Game = false;
    /// <summary>ゲームクリア判定（クリア時にTrue）</summary>
    public bool is_Clear = false;
}

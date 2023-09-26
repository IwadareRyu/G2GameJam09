using System.Collections;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyState _state;
    [SerializeField] int _enemyScoreA = 500;
    [SerializeField] int _enemyScoreB = 500;
    [SerializeField] int _enemyScoreC = 1000;
    [Tooltip("EnemyのDestroyされるまでの時間")]
    [SerializeField] float _waitDestroy = 1;
    [Tooltip("エネミーの吹っ飛ぶ速度")]
    [SerializeField] float _impactedSpeed = 3;
    [Tooltip("エネミーの吹っ飛び方")]
    [SerializeField] Vector2[] _vec2 = new[] { new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, -1)};
    /// <summary>
    /// エネミーにつけるAnimatorのパラメーターにはbool値"Defeat"をfalseで追加してください
    /// </summary>
    Rigidbody2D _rb;
    Animator _anim;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }
    private int GetScore()
    {
        if(_state == EnemyState.EnemyA)
        {
            return _enemyScoreA;
        }
        else if(_state == EnemyState.EnemyB)
        {
            return _enemyScoreB;
        }
        else
        {
            return _enemyScoreC;
        }
    }
    private int GetState()
    {
        if (_state == EnemyState.EnemyA)
        {
            return 0;
        }
        else if (_state == EnemyState.EnemyB)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    [Tooltip("エネミーを倒す時に呼ぶ")]
    public void Defeated()
    {
        StartCoroutine(IDefeated());
    }
    private IEnumerator IDefeated()
    {
        int score = GetScore();
        _anim.SetBool("Defeat", true);
        _rb.AddForce(_vec2[GetState()].normalized * _impactedSpeed,ForceMode2D.Impulse);
        GameManager.Instance.ScoreValue(score);
        //ここからanimatorの処理を描く
        yield return new WaitForSeconds(_waitDestroy);
        Destroy(this);
    }
}

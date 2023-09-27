using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyState _state;
    [SerializeField] int _enemyScoreA = 500;
    [SerializeField] int _enemyScoreB = 500;
    [SerializeField] int _enemyScoreC = 1000;
    [Tooltip("EnemyのDestroyされるまでの時間(s)")]
    [SerializeField] float _waitDestroy = 1;
    [Tooltip("エネミーの吹っ飛ぶ速度")]
    [SerializeField] float _impactedSpeed = 3;
    [Tooltip("エネミーの落下速度")]
    [SerializeField] float _gravity = 0.2f;
    [Tooltip("エネミーの1回転にかかる時間(s)")]
    [SerializeField] float _rotateTime = 0.2f;
    [Tooltip("エネミーの吹っ飛び方(Vector2)")]
    [SerializeField] Vector2[] _vec2 = new[] { new Vector2(1, 0), new Vector2(1, 2), new Vector2(2, -1)};
    /// <summary>
    /// エネミーにつけるAnimatorのパラメーターにはbool値"Defeat"をfalseで追加してください
    /// </summary>
    Animator _anim;
    Transform _tra;
    Rigidbody2D _rb;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _tra = GetComponent<Transform>();
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
        //ここからanimatorの処理を描く
        _anim.SetBool("Defeat", true);
        _rb.velocity = (_vec2[GetState()].normalized * _impactedSpeed);
        _rb.gravityScale = _gravity;
        var rotateDOTween = _tra.DORotate(new Vector3(0, 0, 360), _rotateTime, RotateMode.FastBeyond360);
        rotateDOTween.SetLoops(-1).SetEase(Ease.Linear).Play();
        GameManager.Instance.ScoreValue(score);
        yield return new WaitForSeconds(_waitDestroy);
        rotateDOTween.Kill();
        Destroy(this.gameObject);
    }
}

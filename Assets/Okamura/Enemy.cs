using System.Collections;
using System.ComponentModel;
using UnityEngine;
using DG.Tweening;


[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyState _state;
    [SerializeField] int _enemyScoreA = 500;
    [SerializeField] int _enemyScoreB = 500;
    [SerializeField] int _enemyScoreC = 1000;
    [Tooltip("Enemy��Destroy�����܂ł̎���")]
    [SerializeField] float _waitDestroy = 1;
    [Tooltip("�G�l�~�[�̐�����ԑ��x")]
    [SerializeField] float _impactedSpeed = 3;
    [Tooltip("�G�l�~�[�̗������x")]
    [SerializeField] float _gravity = 0.2f;
    [Tooltip("�G�l�~�[��1��]�ɂ����鎞��")]
    [SerializeField] float _rotateTime = 0.2f;
    [Tooltip("�G�l�~�[�̐�����ѕ�")]
    [SerializeField] Vector2[] _vec2 = new[] { new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, -1)};
    /// <summary>
    /// �G�l�~�[�ɂ���Animator�̃p�����[�^�[�ɂ�bool�l"Defeat"��false�Œǉ����Ă�������
    /// </summary>
    Transform _tra;
    Rigidbody2D _rb;
    Animator _anim;
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

    [Tooltip("�G�l�~�[��|�����ɌĂ�")]
    public void Defeated()
    {
        StartCoroutine(IDefeated());
    }
    private IEnumerator IDefeated()
    {
        int score = GetScore();
        _anim.SetBool("Defeat", true);
        _tra.DORotate(new Vector3(0, 0, 360), _rotateTime, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        _rb.velocity = (_vec2[GetState()].normalized * _impactedSpeed);
        //_rb.AddForce(_vec2[GetState()].normalized * _impactedSpeed,ForceMode2D.Impulse);
        _rb.gravityScale = _gravity;
        GameManager.Instance.ScoreValue(score);
        //��������animator�̏�����`��
        yield return new WaitForSeconds(_waitDestroy);
        Destroy(this);
    }
}

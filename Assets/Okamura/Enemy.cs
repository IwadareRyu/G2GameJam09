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
    [Tooltip("Enemy��Destroy�����܂ł̎���(s)")]
    [SerializeField] float _waitDestroy = 1;
    [Tooltip("�G�l�~�[�̐�����ԑ��x")]
    [SerializeField] float _impactedSpeed = 3;
    [Tooltip("�G�l�~�[�̗������x")]
    [SerializeField] float _gravity = 0.2f;
    [Tooltip("�G�l�~�[��1��]�ɂ����鎞��(s)")]
    [SerializeField] float _rotateTime = 0.2f;
    [Tooltip("�G�l�~�[�̐�����ѕ�(Vector2)")]
    [SerializeField] Vector2[] _vec2 = new[] { new Vector2(1, 0), new Vector2(1, 2), new Vector2(2, -1)};
    /// <summary>
    /// �G�l�~�[�ɂ���Animator�̃p�����[�^�[�ɂ�bool�l"Defeat"��false�Œǉ����Ă�������
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

    [Tooltip("�G�l�~�[��|�����ɌĂ�")]
    public void Defeated()
    {
        StartCoroutine(IDefeated());
    }
    private IEnumerator IDefeated()
    {
        int score = GetScore();
        //��������animator�̏�����`��
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

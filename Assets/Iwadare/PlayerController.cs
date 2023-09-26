using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D _rb;

    [Header("�Œ�X�s�[�h")]
    [SerializeField]
    private float _minSpeed = 0.5f;

    [Tooltip("���݂̃X�s�[�h")]
    private float _speed;

    [Header("�ō��X�s�[�h")]
    [SerializeField]
    private float _maxSpeed = 5f;
    [Header("���Ԃ̃X�s�[�h")]
    [SerializeField]
    private float _midSpeed = 2.5f;
    [Header("�X�s�[�h�ɉ������v���X����d�͂̊���")]
    [SerializeField]
    private float _plusGravity = 0.3f;

    [Tooltip("�U���̃N�[���^�C��")]
    private float _attackTime;

    private bool _attackbool;

    RaycastHit2D _groundHit;
    [SerializeField]
    LayerMask _groundLayer;

    RaycastHit2D _upGroundHit;
    [SerializeField]
    LayerMask _upGroundLayer;

    [Header("�W�����v���A���������ɖ߂��̂ŁAPlayer�̃X�v���C�g���A�^�b�`")]
    [SerializeField]
    Transform _sproteAngle;

    [Header("�G�ɓ������������炷�X�R�A")]
    [SerializeField]
    int _lowScore = -2000;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, new Vector2(0, -3), Color.red);
        Debug.DrawRay(new Vector3(transform.position.x + 1,transform.position.y - 0.5f,transform.position.z),new Vector2(0,1),Color.red);
        Debug.DrawRay(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), new Vector2(1,0), Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        _groundHit = Physics2D.Raycast(transform.position, Vector2.down, 3.0f, _groundLayer);
        _upGroundHit = Physics2D.Raycast(transform.position, Vector2.down, 3.0f, _upGroundLayer);

        float h = Input.GetAxis("Horizontal");
        if (_attackbool)
            _attackTime += Time.deltaTime;

        if (h > 0)
        {
            if (_speed < _maxSpeed)
            {
                PlayerSpeed(0.01f);
            }
        }
        if (h < 0)
        {
            if (_speed > _minSpeed)
            {
                PlayerSpeed(-0.01f);
            }
        }

        if (_attackTime > 0.3f)
        {
            _attackbool = false;

            //�s�����݂����ȉ����o�������H

            _attackTime = 0;
        }
        if (!_attackbool)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                AttackMotion(EnemyState.EnemyA);
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                AttackMotion(EnemyState.EnemyB);
            }
            else if (Input.GetButtonDown("Fire3"))
            {
                AttackMotion(EnemyState.EnemyC);
            }
        }
    }

    private void FixedUpdate()
    {
        float n = 0f;

        if(_groundHit.collider)
        {
            n = -_speed * _plusGravity;
        }
        if(_upGroundHit.collider)
        {
            n = -_speed * 0.1f;
        }
        _rb.velocity = new Vector2(_speed, _rb.velocity.y + n);
    }

    void PlayerSpeed(float num)
    {
        _speed += num;
        if (_speed >= _maxSpeed - 0.1f)
        {
            //����
        }
        else if (_speed > _midSpeed)
        {
            //�㕗
        }
        else
        {
            //�Ȃ�
        }
    }

    void AttackMotion(EnemyState state)
    {

        //state�ɍ��킹���A�^�b�N���[�V����

        Collider2D[] cols;
        cols = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + 1,transform.position.y),1.0f);

        foreach(var col in cols)
        {
            if(col)
            {
                Enemy enemy;
                if(enemy = col.GetComponent<Enemy>())
                {
                    if(enemy._state == state)
                    {
                        enemy.Defeated();
                    }
                }
            }
        }
        _attackTime = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            _speed = 0.5f;
            GameManager.Instance.ScoreValue(_lowScore);

        }
    }
}

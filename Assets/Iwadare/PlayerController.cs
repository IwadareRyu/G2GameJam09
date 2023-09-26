using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D _rb;

    [Header("最低スピード")]
    [SerializeField]
    private float _minSpeed = 0.5f;

    [Tooltip("現在のスピード")]
    private float _speed;

    [Header("最高スピード")]
    [SerializeField]
    private float _maxSpeed = 5f;
    [Header("スピードに応じたプラスする重力の割合")]
    [SerializeField]
    private float _plusGravity = 0.3f;

    private float _attackTime;

    private bool _attackBool;
    EnemyState _enemyState = EnemyState.EnemyA;

    RaycastHit2D _groundHit;
    [SerializeField]
    LayerMask _groundLayer;

    RaycastHit2D _upGroundHit;
    [SerializeField]
    LayerMask _upGroundLayer;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, new Vector2(0, -3), Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        _groundHit = Physics2D.Raycast(transform.position, Vector2.down, 3.0f, _groundLayer);
        _upGroundHit = Physics2D.Raycast(transform.position, Vector2.down, 3.0f, _upGroundLayer);

        float h = Input.GetAxis("Horizontal");
        if (_attackBool)
        {
            _attackTime += Time.deltaTime;
        }

        if(h > 0)
        {
            if (_speed < _maxSpeed)
            {
                PlayerSpeed(0.01f);
            }
        }
        if(h < 0)
        {
            if (_speed > _minSpeed)
            {
                PlayerSpeed(-0.01f);
            }
        }
        
        if(Input.GetButtonDown("Fire1"))
        {
            AttackMotion(EnemyState.EnemyA);
        }
        else if(Input.GetButtonDown("Fire2"))
        {
            AttackMotion(EnemyState.EnemyB);
        }
        else if (Input.GetButtonDown("Fire3"))
        {
            AttackMotion(EnemyState.EnemyC);
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
    }

    void AttackMotion(EnemyState state)
    {
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
    }
}

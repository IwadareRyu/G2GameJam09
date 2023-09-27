using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Callbacks;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    Rigidbody2D _rb;
    CircleCollider2D _circleColider;

    [SerializeField]
    GameObject _wind;

    [SerializeField]
    GameObject _locus;


    [SerializeField]
    Sprite _playerSprite;

    Animator _animator;

    int _rotaCount;

    [Header("最低スピード")]
    [SerializeField]
    private float _minSpeed = 0.5f;

    [Tooltip("現在のスピード")]
    [SerializeField]
    private float _speed;

    [Header("最高スピード")]
    [SerializeField]
    private float _maxSpeed = 5f;
    [Header("中間のスピード")]
    [SerializeField]
    private float _midSpeed = 2.5f;
    [SerializeField]
    private float _jumpPower = 3f;
    [Header("スピードに応じたプラスする重力の割合")]
    [SerializeField]
    private float _plusGravity = 0.3f;

    [Tooltip("攻撃のクールタイム")]
    private float _attackTime;

    private bool _attackBool;

    private bool _jumpBool;

    private bool _actionBool;

    private bool _invisibleBool;

    RaycastHit2D _groundHit;
    [SerializeField]
    LayerMask _groundLayer;

    //RaycastHit2D _upGroundHit;
    //[SerializeField]
    //LayerMask _upGroundLayer;

    [Header("ジャンプ時、向きを元に戻すので、Playerのスプライトをアタッチ")]
    [SerializeField]
    Transform _spriteAngle;

    [Header("敵に当たった時減らすスコア")]
    [SerializeField]
    int _lowScore = -2000;

    [Header("スピードを表示するテキスト(かなり盛ってる)")]
    [SerializeField]
    Text _speedText;

    float _speedTimer;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _circleColider = GetComponent<CircleCollider2D>();
        _speed = _minSpeed;
        _animator = GetComponent<Animator>();
        _wind.SetActive(false);
        _locus.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, new Vector2(0, -2), Color.red);
        Debug.DrawRay(new Vector3(transform.position.x + 1,transform.position.y - 0.8f,transform.position.z),new Vector2(0,1),Color.red);
        Debug.DrawRay(new Vector3(transform.position.x + 0.5f, transform.position.y - 0.3f, transform.position.z), new Vector2(1,0), Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.Instance.is_Game)
        //{
        _groundHit = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, _groundLayer);
        //_upGroundHit = Physics2D.Raycast(transform.position, Vector2.down, 3.0f, _upGroundLayer);

        float h = Input.GetAxis("Horizontal");
        if (_attackBool)
        {
            _attackTime += Time.deltaTime;
        }
        _speedTimer += Time.deltaTime;

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
            _attackBool = false;

            //ピロンみたいな音を出すかも？

            _attackTime = 0;
        }

        if (!_attackBool)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                AttackMotion(EnemyState.EnemyA);
                CRIAudioManager.Instance.CriSePlay(3);
                _animator.Play("Attack1");
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                AttackMotion(EnemyState.EnemyB);
                CRIAudioManager.Instance.CriSePlay(3);
                _animator.Play("Attack2");
            }
            else if (Input.GetButtonDown("Fire3"))
            {
                AttackMotion(EnemyState.EnemyC);
                CRIAudioManager.Instance.CriSePlay(3);
                _animator.Play("Attack3");
            }
        }



        if (_speedTimer > 1f)
        {
            int score = 0;
            if (_speed >= _maxSpeed - 0.1f)
            {
                score = 300;
                //強風
                if(_wind.active == false)
                {
                    _wind.SetActive(true);
                }
                if(_locus.active == false)
                {
                    _locus.SetActive(true);
                }
            }
            else if (_speed > _midSpeed)
            {
                score = 200;
                //弱風
                if (_locus.active == false)
                {
                    _locus.SetActive(true);
                }
                if(_wind.active == true)
                {
                    _wind.SetActive(false);
                }
            }
            else
            {
                if(_locus.active == true)
                {
                    _locus.SetActive(false);
                }
                if(_wind.active == true)
                {
                    _wind.SetActive(false);
                }
            }
            if (GameManager.Instance)
            {
                GameManager.Instance.ScoreValue(score);
            }
        }
        //}
    }

    private void FixedUpdate()
    {
        float n = 0f;


        if(_groundHit.collider)
        {
            n = -_speed * _plusGravity;
        }

        //if(_upGroundHit.collider)
        //{
        //    n = -_speed * 0.2f;
        //}


        _rb.velocity = new Vector2(_speed, _rb.velocity.y + n);

        //if (Input.GetButton("Jump") && _jumpBool)
        //{
        //    _actionBool = true;
        //    _rb.velocity = new Vector2(_speed,_jumpPower);
        //    CRIAudioManager.Instance.CriSePlay(2);
        //    _jumpBool = false;
        //}

        //if (Input.GetButtonUp("Jump") && _actionBool)
        //{
        //    n = 0;

        //    _spriteAngle.Rotate(0, 0, 45);
        //    _rotaCount++;

        //    if (_rotaCount == 8)
        //    {
        //        _rotaCount = 0;
        //        GameManager.Instance.ScoreValue(1000);
        //    }
        //}

        if (_speedText)
        {
            var total = (Mathf.Abs(_rb.velocity.x) + Mathf.Abs(_rb.velocity.y)) * 8;
            _speedText.text = $"Speed: {total.ToString("0.00")} km";
        }

        _animator.SetFloat("Speed",_speed);
    }

    void PlayerSpeed(float num)
    {
        _speed += num;
    }

    void AttackMotion(EnemyState state)
    {

        //stateに合わせたアタックモーション

        Collider2D[] cols;
        cols = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + 1,transform.position.y - 0.3f),1.0f);

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
        if(collision.gameObject.tag == "JumpGround")
        {
            _jumpBool = true;
            _actionBool = false;
        }

        if(collision.gameObject.tag == "Ground")
        {
            _jumpBool = false;
            if (_actionBool)
            {
                _actionBool = false;
                _rotaCount = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && !_invisibleBool)
        {
            _speed = _minSpeed;
            CRIAudioManager.Instance.CriSePlay(0);
            if (GameManager.Instance)
            {
                GameManager.Instance.ScoreValue(_lowScore);
            }
            StartCoroutine(EnemyDamageCoroutine());
        }
    }

    IEnumerator EnemyDamageCoroutine()
    {
        _invisibleBool = true;
        yield return new WaitForSeconds(2f);
        _invisibleBool = false;
    }
}

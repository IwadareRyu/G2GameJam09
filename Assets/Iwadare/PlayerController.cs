using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D _rb;
    
    [SerializeField] 
    private float _speed;
    [SerializeField] 
    private float _maxSpeed = 5f;

    private float _attackTime;

    private bool _attackBool;
    EnemyState _enemyState;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_attackBool)
        {
            _attackTime += Time.deltaTime;
        }
    }
}

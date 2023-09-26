using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyState _state;
    [SerializeField] int _enemyScoreA = 500;
    [SerializeField] int _enemyScoreB = 500;
    [SerializeField] int _enemyScoreC = 1000;
    [Tooltip("Enemy��Destroy�����܂ł̎���")]
    [SerializeField] float _waitDestroy = 1;
    
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

    [Tooltip("�G�l�~�[��|�����ɌĂ�")]
    public void Defeated()
    {
        StartCoroutine(IDefeated());
    }
    private IEnumerator IDefeated()
    {
        int score = GetScore();
        GameManager.Instance.ScoreValue(score);
        yield return new WaitForSeconds(_waitDestroy);
        //��������animator�̏�����`��
        Destroy(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Tooltip("スコアのポイント")]
    [SerializeField] private int _scoreValue = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") { Action(); }
    }

    private void Action()
    {
        GameManager.Instance.ScoreValue(_scoreValue);
        CRIAudioManager.Instance.CriSePlay(1);
        Destroy(gameObject);
    }
}
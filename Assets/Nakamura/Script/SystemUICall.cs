using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemUICall : MonoBehaviour
{
    [Tooltip("Score、Timeを表示するテキストの背景UI")]
    [SerializeField] private GameObject _systemUI = null;
    private bool is_UI = false;
    private void Start()
    {
        if (_systemUI != null) {_systemUI.SetActive(false);}
        else {Debug.Log("シリアライズしてください");}
    }

    void Update()
    {
        if (!is_UI && GameManager.Instance.is_Game && _systemUI != null)
        {
            is_UI = true;
            _systemUI.SetActive((true));
        }
    }
}

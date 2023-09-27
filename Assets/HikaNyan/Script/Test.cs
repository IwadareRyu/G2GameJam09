using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class Test : MonoBehaviour
{
    private void Start()
    {
        CRIAudioManager.Instance.CriBgmPlay(0);
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.A))
            .Subscribe(_ => {
                CRIAudioManager.Instance.CriBgmPlay(1);
                Debug.Log("Play");
            });
        
    }
}

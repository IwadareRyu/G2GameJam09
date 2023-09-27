using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;
using UniRx;
using Cysharp.Threading.Tasks;
using Hikanyan.Core;


public class CRIAudioManager : AbstractSingleton<CRIAudioManager>
{
    [SerializeField] string _streamingAssetsPathAcf = "";
    [SerializeField] string _cueSheetBGM = "";
    [SerializeField] string _cueSheetSe = "";

    CriAtomSource _criAtomSourceBgm;
    CriAtomSource _criAtomSourceSe;

    private CriAtomExPlayback _criAtomExPlaybackBGM;
    CriAtomEx.CueInfo _cueInfo;

    protected override void OnAwake()
    {
        //acf設定
        string path = Common.streamingAssetsPath + $"/{_streamingAssetsPathAcf}.acf";

        CriAtomEx.RegisterAcf(null, path);

        // CriAtom作成
        new GameObject().AddComponent<CriAtom>();

        // BGM acb追加
        CriAtom.AddCueSheet(_cueSheetBGM, $"{_cueSheetBGM}.acb", null, null);
        // SE acb追加
        CriAtom.AddCueSheet(_cueSheetSe, $"{_cueSheetSe}.acb", null, null);


        //BGM用のCriAtomSourceを作成
        _criAtomSourceBgm = gameObject.AddComponent<CriAtomSource>();
        _criAtomSourceBgm.cueSheet = _cueSheetBGM;
        //SE用のCriAtomSourceを作成
        _criAtomSourceSe = gameObject.AddComponent<CriAtomSource>();
        _criAtomSourceSe.cueSheet = _cueSheetSe;
    }
    
    float lastResumeBgmTime = 0;
    // Update is called once per frame.
    void Update () {

        if(lastResumeBgmTime + 2 < Time.timeSinceLevelLoad){
            ResumeBGM();
            lastResumeBgmTime = Time.timeSinceLevelLoad;
        }
    }

    private int _indexStay = 0;
    public void ResumeBGM()
    {
        /* Play if the status is in the PlayEnd or the Stop. (automatically restart when ACB is updated) */
        CriAtomSource.Status status = _criAtomSourceBgm.status;
        if ((status == CriAtomSource.Status.Stop) || (status == CriAtomSource.Status.PlayEnd))
        {
            CriBgmPlay(_indexStay);
        }
    }

    public void CriBgmPlay(int index)
    {
        CriAtomSource.Status status = _criAtomSourceBgm.status;
        if ((status == CriAtomSource.Status.Stop) || (status == CriAtomSource.Status.PlayEnd))
        {
            _criAtomExPlaybackBGM = _criAtomSourceBgm.Play(index);
            _indexStay = index;
        }
        
    }

    public void CriBgmStop()
    {
        _criAtomSourceBgm.Stop();
    }

    public void CriSePlay(int index)
    {
        _criAtomSourceSe.Play(index);
    }

    public void CriSeStop()
    {
        _criAtomSourceSe.Stop();
    }
}
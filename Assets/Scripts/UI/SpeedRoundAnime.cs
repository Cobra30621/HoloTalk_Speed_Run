using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SpeedRoundAnime : MonoBehaviour
{
    public GameObject GOSpeedRun;
    public Transform startPos;
    public Transform endPos;
    public Ease moveEase;

    public float scaleSize;
    public float scaleTime;
    public float fadeTime;

    void Start(){
        endPos.position = GOSpeedRun.transform.position;
        GOSpeedRun.SetActive(false);
    }

    [ContextMenu("Test")]
    public void Test(){
        PlayAnime();
    }

    public void PlayAnime(){
        GOSpeedRun.transform.position = startPos.position;

        GOSpeedRun.SetActive(true);
        Sequence sequence = DOTween.Sequence();

        sequence.Append(GOSpeedRun.transform.DOMove( endPos.position, 0.5f).SetEase(moveEase))
        .Append(GOSpeedRun.transform.DOScale( scaleSize *  GOSpeedRun.transform.localScale, scaleTime))
        .Append(GOSpeedRun.GetComponent<Image>().DOFade(0, fadeTime))
        .AppendCallback(onComplete);
        
    }

    private void onComplete(){
        GOSpeedRun.SetActive(false);
    }
}

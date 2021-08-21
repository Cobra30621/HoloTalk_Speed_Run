using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OptionButton : MonoBehaviour
{
    // 寬背景
    [SerializeField]private Image img_bar;
    // 長細條
    [SerializeField]private Image img_buckle;
    [SerializeField]private Image img_mask;
    [SerializeField]private Transform endPos;
    public Ease moveEase;

    // layout
    [SerializeField]private RectTransform button_layout;
    [SerializeField]private Text lab_info;

    [ContextMenu("Test")]
    public void Test(){
        SetLayout();
        // PlayAnime(0.5f);
    }

    [ContextMenu("PlayAnime")]
    public void PlayAnime(float animeTime, bool fadeIn, bool fadeOut){
        Debug.Log($"fadeIn{fadeIn}, fadeOut{fadeOut}");
        img_buckle.transform.DOMoveX(endPos.position.x, animeTime).SetLoops(2, LoopType.Yoyo).SetEase(moveEase);
        img_mask.GetComponent<RectTransform>().DOAnchorMin(new Vector2(1f, 0f), animeTime).SetLoops(2, LoopType.Yoyo).SetEase(moveEase);

        Sequence sequence = DOTween.Sequence();
        if(fadeIn){
            // 背景關閉再撥放
            // img_bar.color = new Color(1,1,1,0);
            sequence.AppendInterval(animeTime)
            .Append(img_bar.DOFade(1f, 0f));
        }

        if(fadeOut){
            sequence.PrependInterval(animeTime)
            .Append(img_bar.DOFade(0f, 0f))
            .Append(img_buckle.DOFade(0f, animeTime + 0.2f));
        }
    }

    public void SetImageUnActive(){
        img_bar.color = new Color(1,1,1,0);
    }

    public void SetLayout(){
        img_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(button_layout.rect.width, button_layout.rect.height);
        lab_info.GetComponent<RectTransform>().sizeDelta = new Vector2(button_layout.rect.width -30, button_layout.rect.height);
    }
}

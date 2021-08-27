using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OptionBar : MonoBehaviour
{
    [SerializeField]private Image img_head;
    [SerializeField]private Image img_mask;
    [SerializeField]private GameObject option1;
    [SerializeField]private GameObject option2_1;
    [SerializeField]private GameObject option2_2;

    [SerializeField]private Text lab_option1;
    [SerializeField]private Text lab_option2_1;
    [SerializeField]private Text lab_option2_2;

    [SerializeField]private Transform endPos;
    public Ease moveEase;

    private const float animeTime = 0.4f;

    private bool oneOption;
    private bool option2;

    [SerializeField]private string info_option1;
    [SerializeField]private string info_option2_1;
    [SerializeField]private string info_option2_2;

    [ContextMenu("Test")]
    public void Test(){

    }

    [ContextMenu("PlayAnime")]
    public void PlayAnime(bool oneOption, bool option2){
        this.oneOption = oneOption;
        this.option2 = option2;
        
        img_head.transform.DOMoveX(endPos.position.x, animeTime).SetLoops(2, LoopType.Yoyo).SetEase(moveEase);
        img_mask.GetComponent<RectTransform>().DOAnchorMin(new Vector2(1f, 0f), animeTime).SetLoops(2, LoopType.Yoyo).SetEase(moveEase);

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(animeTime)
        .AppendCallback(SetOptionActive)
        .AppendCallback(SetOptionLab);

    }

    public void SetOptionLab(){
        lab_option1.text = info_option1;
        lab_option2_1.text = info_option2_1;
        lab_option2_2.text = info_option2_2;
    }

    // 設定選項文字
    public void SetOption1Info(string info){
        info_option1 = info;
    }

    public void SetOption2Info(int id, string info){
        if(id == 0)
            info_option2_1 = info;
        if(id == 1)
            info_option2_2 = info;
    }

    public void Init(){
        img_head.color = new Color(1,1,1,0);
        SetAllOptionUnActive();
    }

    public void FadeIn(){
        SetAllOptionUnActive();
        Sequence sequence = DOTween.Sequence();
        img_head.color = new Color(1,1,1,0);
        sequence.Append(img_head.DOFade(1f, 0.1f));
    }

    public void FadeOut(){
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(animeTime + 0.05f) // 將所有選項關閉
        .AppendCallback(SetAllOptionUnActive)
        .AppendInterval(animeTime - 0.1f)
        .Append(img_head.DOFade(0f, 0.2f));
    }

    private void SetAllOptionUnActive(){
        option1.SetActive(false);
        option2_1.SetActive(false);
        option2_2.SetActive(false);
    }

    private void SetOptionActive(){
        option1.SetActive(oneOption);
        option2_1.SetActive(!oneOption);
        option2_2.SetActive(!oneOption && option2);
    }

}

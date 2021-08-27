using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCardSystem : MonoBehaviour
{
    [SerializeField] private Text lab_BG;
    [SerializeField] private Text lab_FG;
    [SerializeField] private Image img_BG;
    [SerializeField] private Image img_FG;
    [SerializeField] private Sprite [] sprite_cardTypes;

    [SerializeField] private GameObject GO_textCard_FG;
    [SerializeField] private Transform transform_textBox;
    // 點籍可以觸發下一個對話
    [SerializeField] private GameObject GO_nextConversationClick;


    private string now_text;
    [SerializeField] private Animator animator;
    
    // public bool clickToPlayNextCard; 
    public int cardTypeID;
    public bool waitClick;

    public void SetNextCard(TextCard textCard){
        GO_textCard_FG.transform.position = transform_textBox.position;

        now_text = textCard.info;
        cardTypeID = textCard.cardType;
        // clickToPlayNextCard = textCard.clickToPlayNextCard;

        lab_BG.text = now_text;
        if(sprite_cardTypes[cardTypeID] != null)
            img_BG.sprite = sprite_cardTypes[cardTypeID];
        animator.SetTrigger("nextcard");
    }

    public void SetTextCardBack(){
        lab_FG.text = now_text;
        if(sprite_cardTypes[cardTypeID] != null)
            img_FG.sprite = sprite_cardTypes[cardTypeID];

        GO_textCard_FG.transform.position = transform_textBox.position;
    }

    public void SetTextCardInfo(string info){
        now_text = info;
        lab_BG.text = now_text;
        lab_FG.text = now_text;
    }

    public void OnClick(){
        SetWaitClick(false);
    }

    public void SetWaitClick(bool bo){
        waitClick = bo;
        GO_nextConversationClick.SetActive(bo);
    }
}


public class TextCard{
    public string info;
    // public bool clickToPlayNextCard = false;
    public int cardType; // 0 = Question 1 = Conversation

    public TextCard(string info, int cardType){
        this.info = info;
        this.cardType = cardType;
    }

}
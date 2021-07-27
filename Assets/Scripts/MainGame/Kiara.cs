using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Localization;

public class Kiara : MonoBehaviour
{
    public Animator animator;

    public GameObject kiaraBubble;
    public Text lab_speech;
    
    // 0:idle 1: 
    public void SetKiaraAnime(string trigger){
        animator.SetTrigger(trigger);
    }

    public void SetKiaraText(string text){
        SetBubbleActive(true);
        lab_speech.text = LeanLocalization.GetTranslationText(text);
    }

    public void SetBubbleActive(bool bo){
        kiaraBubble.SetActive(true);
    }
}


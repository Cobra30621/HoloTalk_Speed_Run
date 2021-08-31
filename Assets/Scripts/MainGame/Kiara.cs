using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Localization;

public class Kiara : MonoBehaviour
{
    public Animator animator;
    // 0:holotalkspeedrun 1:yabe 2:sad 3:pant color 4:ohh 5:nnn 6:why
    public SFXManager sfx;

    public GameObject kiaraBubble;
    public Text lab_speech;

    public Sprite[] kiara_sprites;
    public Image img_kiara;

    void Update(){
        if(Input.GetKeyDown(KeyCode.F)){
            
        }
    }

    public KiaraState kiaraState;
    [ContextMenu("TestAnime")]
    public void TestAnime(){
        SetKiaraAnime(kiaraState);
    }

    public void SetKiaraAnime(KiaraState state){
        switch (state)
        {
            case KiaraState.Idle:
                animator.SetTrigger("idle");
                break;
            case KiaraState.Talking:
                animator.SetTrigger("talking");
                break;
            case KiaraState.Except:
                animator.SetTrigger("except");
                break;
            case KiaraState.Exciting:
                animator.SetTrigger("exciting");
                break;
            case KiaraState.Unexcept:
                animator.SetTrigger("unexcept");
                break;
            case KiaraState.Sad:
                animator.SetTrigger("sad");
                break;
            case KiaraState.Drug:
                animator.SetTrigger("drug");
                break;
            case KiaraState.KeepTalking:
                animator.SetTrigger("keeptalking");
                break;
            case KiaraState.Smile:
                animator.SetTrigger("smile");
                break;
            case KiaraState.SmallSmile:
                animator.SetTrigger("smallsmile");
                break;
            default:
                Debug.Log($"不存在State:{state}");
                break;
        }
        Debug.Log($"撥放KiaraState:{state}");
    }

    public void PlaySFX(KiaraSFX kiaraSFX){
        if(kiaraSFX == KiaraSFX.None){return;}
        int id = (int)kiaraSFX;
        sfx.PlaySFX(id);
    }

    public void SetKiaraText(string text){
        SetBubbleActive(true);
        Debug.Log(text + LeanLocalization.GetTranslationText(text));
        lab_speech.text = LeanLocalization.GetTranslationText(text);
    }

    public void SetBubbleActive(bool bo){
        kiaraBubble.SetActive(true);
    }
}

[System.Serializable]
public enum KiaraState{
    Idle, Talking, Except, Unexcept, Exciting, Sad, Drug, KeepTalking, Smile, SmallSmile
}

[System.Serializable]
public enum KiaraSFX{
    SpeedRun = 0 , Yabe =1, Sad =2, Pant=3, NNN=4, Ohh=5, Why=6 , OhhLow=7, OK=8, allRight =9, nnnnn = 10, ohhQuestion =11, None = 100
}
// 0:holotalkspeedrun 1:yabe 2:sad 3:pant color 4:ohh 5:nnn 6:why
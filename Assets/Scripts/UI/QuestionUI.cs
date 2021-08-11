using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionUI : MonoBehaviour
{
    public GameManager gameManager;
    
    // 超級爛的寫法
    public void SetQuestionPosInfoBack(){
        gameManager.SetQuestionPosInfoBack();
    }
}

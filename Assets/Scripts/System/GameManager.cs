using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;

public class GameManager : MonoBehaviour{

    // 儲存玩家的答題情況
    public List<int> playerAnswers;
    public Dictionary<VTuber, float> similarityDictionary;

    public int now_question; // 現在的題目數
    private int questionCount = 19; // 最多題目數

    // 問題顯示介面
    public LeanLocalizedText LLT_question;
    public List<LeanLocalizedText> LLT_options;

    private const string optionRawKey = "_option";
    private const string questionRawKey = "_question";

    [ContextMenu("Test2")]
    public void Test2(){
        SetQuetionWithId(now_question);
    }


    public void NextQuetion(){
        now_question ++;
        if(now_question >= questionCount){
            now_question = 0;
        }

        SetQuetionWithId(now_question);
    }

    public void SetQuetionWithId(int id){
        LLT_question.TranslationName = id + questionRawKey ;

        // 要去判斷題目有幾題
        for (int i = 0; i < LLT_options.Count; i++)
        {
            // 題目_option_選項  0_option0
            string optionKey = id + optionRawKey + i ; 
            LLT_options[i].TranslationName = optionKey;
        }
    }
}




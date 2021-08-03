using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Localization;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    // 儲存玩家的答題情況
    public List<int> playerAnswers;

    public int now_question; // 現在的題目數
    private int questionCount = 19; // 最多題目數

    // 問題顯示介面
    private const string optionRawKey = "_option";
    private const string questionRawKey = "_question";

    public GridLayoutGroup option_gridLayoutGroup;
    private float moreThanFourOptionsWeight = 1000;
    private float lessThanFourOptionsWeight = 500;
    private float optionsHeight = 100;

    public Text lab_questionInfo;
    public GameObject[] options;
    public Text[] lab_options;

    void Start(){
        StartGaming();
    }

    [ContextMenu("StartGaming")]
    public void StartGaming(){
        now_question = 0;
        SetQuetionWithId(now_question);
    }

    public void Again(){
        SceneManager.LoadScene("Game");
    }

    public void AnswererQuestion(int answer){
        playerAnswers[now_question] = answer;
        Debug.Log($"Q{now_question}:{answer}");

        now_question ++;
        if(now_question >= questionCount){
            Debug.Log("答完題了");
            ResultUI.ShowResult(playerAnswers);
            now_question = 0;
            return;
        }

        SetQuetionWithId(now_question);
    }

    public void SetQuetionWithId(int questionId){
        // 取得題目的選項數
        int optionCount = QuestionDataManager.questionOptionCount[questionId];
        

        // 設定題目
        string questionInfo_key = questionId + "_question";
        lab_questionInfo.text =  LeanLocalization.GetTranslationText(questionInfo_key);

        // 設定選項
        SetOptionsLayout(optionCount);
        SetOptionsActive(optionCount);
        for (int op = 0; op < optionCount; op++)
        {
            string optionKey = questionId + optionRawKey + op ;
            lab_options[op].text = LeanLocalization.GetTranslationText(optionKey);
        }
    }

    private void SetOptionsLayout(int optionCount){
        if(optionCount > 4){
            option_gridLayoutGroup.constraintCount = 2;
            option_gridLayoutGroup.cellSize = new Vector2(lessThanFourOptionsWeight, 100);
        }
        else{
            option_gridLayoutGroup.constraintCount = 1;
            option_gridLayoutGroup.cellSize = new Vector2(moreThanFourOptionsWeight, 100);
        }
    }

    private void SetOptionsActive(int optionCount){
        foreach (GameObject option in options)
        {
            option.SetActive(false);
        }

        for (int i = 0; i < optionCount; i++)
        {
            options[i].SetActive(true);
        }
    } 
}




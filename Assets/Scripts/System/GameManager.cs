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
    public GameObject optionPanel;

    // 流程控制
    public Kiara kiara;
    public BGMManager bgm;
    // 0:holotalkspeedrun 1:yabe 2:sad 3:pant color
    public SFXManager sfx_kiara;

    void Start(){
        StartGame();
    }

    [ContextMenu("StartGaming")]
    public void StartGame(){
        now_question = 0;
        GameSettings.ResetToDefaults();
        optionPanel.SetActive(false);
        StartCoroutine(GameCoroutine());
    }

    public void Again(){
        SceneManager.LoadScene("StartScene");
    }

    IEnumerator GameCoroutine()
    {
        yield return new WaitForSeconds(1);
        for (int i = 1; i <= 5; i++)
        {
            string info = $"Kiara/Intro{i}";
            Debug.Log(info);
            kiara.SetKiaraText(info);
            yield return new WaitForSeconds(1);
        }

        sfx_kiara.PlaySFX(0);
        kiara.SetKiaraText("Kiara/Intro6");
        yield return new WaitForSeconds(5);

        optionPanel.SetActive(true);
        now_question = 0;
        SetQuetionWithId(now_question);
    }

    // IEnumerable KiaraSay(string info){
    //     kiara.SetKiaraText(info);
    //     yield return new WaitForSeconds(3);
    // }


    public void AnswererQuestion(int answer){
        playerAnswers[now_question] = answer;
        Debug.Log($"Q{now_question}:{answer}");

        KiaraResponceWhenAnswer(now_question, answer);
        now_question ++;
        KiaraResponceWhenQuestioning(now_question);
        
        if(now_question >= questionCount){
            Debug.Log("答完題了");
            StartCoroutine(EndGameCoroutine());
            now_question = 0;
            return;
        }

        SetQuetionWithId(now_question);
    }

    IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(1);
        for (int i = 1; i <= 3; i++)
        {
            string info = $"Kiara/End{i}";
            Debug.Log(info);
            kiara.SetKiaraText(info);
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1);
        
        ResultUI.ShowResult(playerAnswers); 
    }


    private void KiaraResponceWhenQuestioning(int questionId){
        switch (questionId)
        {
            case 18:
                kiara.SetKiaraAnime("exciting");
                sfx_kiara.PlaySFX(3);
                break;
            default:
                break;
        }
    }

    private void KiaraResponceWhenAnswer(int questionId, int answer){
        switch (questionId)
        {
            case 8:
                if(answer == 1) {sfx_kiara.PlaySFX(2);}
                break;
            case 12:
                if(answer == 0) {sfx_kiara.PlaySFX(1);}
                break;
            case 18:
                if(answer == 7) {sfx_kiara.PlaySFX(1);}
                break;
            default:
                break;
        }
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




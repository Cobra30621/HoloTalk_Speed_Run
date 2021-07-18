using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;

public class QuestionDataManager : MonoBehaviour{
    // 資料庫
    public QuestionData questionData;
    public CsvLoader csvLoader;

    // 儲存玩家的答題情況
    public List<int> playerAnswers;
    public VTuber foundVTuber;
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


    [ContextMenu("Test")]
    public void Test(){
        CalculateAllSimilarity(playerAnswers);
        
    }

    public Question GetQuestionById(int id){
        return questionData.GetQuestionById(id);
    }

    public Dictionary<VTuber, float> CalculateAllSimilarity(List<int> playerAnswers){
        similarityDictionary = new Dictionary<VTuber, float>();
        foreach (Answerer answerer in questionData.answerersList)
        {
            VTuber vTuber = answerer.vTuber;
            float rate = CalculateSimilarity(playerAnswers, answerer.answers);
            Debug.Log($"vTuber:{vTuber}的相似度為{rate}");
            similarityDictionary.Add(vTuber, rate);
        } 
        return similarityDictionary;
    }

    public float CalculateSimilarity(List<int> answers, List<int> vtAnswers){
        float effectiveQuetionCount = 0;
        float sameQuetionCount = 0;
        
        if(answers.Count != vtAnswers.Count){
            Debug.Log($"玩家與VTuber的答題數不同\n玩家:{answers.Count} VTuber{vtAnswers.Count}");
            return 0;
        }

        for (int i = 0; i < answers.Count; i++)
        {
            if(vtAnswers[i] != -1){ // -1表示該VTuber沒答此題目
                // Debug.Log($"answers[i]:{answers[i]},vtAnswers[i]{vtAnswers[i]}");
                if(answers[i] == vtAnswers[i]){
                    
                    sameQuetionCount ++;
                }
                effectiveQuetionCount ++;
            }
        }

        if(effectiveQuetionCount == 0){
            Debug.Log("有效題數為0");
            return 0;
        }

        Debug.Log($"sameQuetionCount:{sameQuetionCount},effectiveQuetionCount{effectiveQuetionCount}");
        return sameQuetionCount / effectiveQuetionCount;
    } 

    // ==================================
    // 資料CSV資料致QuestionData
    // ==================================
    [ContextMenu("LoadCSVToQuestionData")]
    public void LoadCSVToQuestionData(){
        csvLoader.Init();

        SetQuestionData();
        SetAnswerData();
    }

    private void SetQuestionData(){
        List<Question> questionsList = new List<Question>();

        // 問題的數量
        int questionCount =  csvLoader.GetQuetionCount();
        int option_max_count = csvLoader.options[0].Length - 1; // 要去掉題目

        for (int i = 0; i < questionCount; i++)
        {
            Question q = new Question();
            q.id = i ;

            // 取的問題敘述，並將問題前的換行去掉
            string rawQuestionInfo = csvLoader.GetOptionsDataByRowAndColFrom(i, 0);
            string questionInfo =  RemoveChangeLine(rawQuestionInfo);
            q.questionInfo = questionInfo;

            // 取得該問題的選項
            List<string> options = new List<string>();
            for (int op = 0; op < option_max_count; op++)
            {
                string option = csvLoader.GetOptionsDataByRowAndColFrom(i, op + 1);
                if (option != ""){
                    options.Add(option);
                }
            }
            q.options = options;
            questionsList.Add(q);
        } 

        questionData.questionsList = questionsList;
    }

    private void SetAnswerData(){

        int questionCount =  csvLoader.GetQuetionCount(); // 問題的數量
        int vTuberCount = csvLoader.GetVTuberCount(); // 取得VTuber數量

        List<Answerer> answerersList = new List<Answerer>();

        for (int vt = 0; vt < vTuberCount; vt++)
        {
            Answerer answerer = new Answerer();
            // 將VTuber前的換行去掉
            string rawName =  csvLoader.GetVTuberAnswersDataByRowAndColFrom(vt + 1, 0);
            answerer.name = RemoveChangeLine(rawName);
            
            // 尋找VTuber的答案
            List<int> answers = new List<int>();
            for (int q = 0; q < questionCount; q++)
            {
                string vtAnswer = csvLoader.GetVTuberAnswersDataByRowAndColFrom(vt + 1,q +1);
                List<string> options = questionData.questionsList[q].options;

                bool hasAnswer = false;
                for (int option = 0; option < options.Count; option++)
                {
                    Debug.Log("vtAnswer" + vtAnswer + ",option" + options[option]);
                    if(vtAnswer == options[option]){
                        answers.Add(option);
                        hasAnswer = true;
                    }
                }

                if(!hasAnswer){
                    answers.Add(-1); // 表示該題沒答案
                }
            }

            answerer.answers = answers;
            answerersList.Add(answerer);
        }     

        questionData.answerersList = answerersList;
    }

    private string RemoveChangeLine(string raw){
        return raw.Replace("\n", "");
    }

    
}
 


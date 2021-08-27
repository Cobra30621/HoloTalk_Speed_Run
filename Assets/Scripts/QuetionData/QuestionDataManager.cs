using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using System;

public class QuestionDataManager : MonoBehaviour{
    // 資料庫
    public QuestionData questionData;
    public CsvLoader csvLoader;
    public LeanLocalization localization;


    // 每一題的選項數量
    public static int[] questionOptionCount = 
    {2,2,3,2,2,
    2,3,2,2,2,
    3,2,2,6,3,
    2,2,2,9};
    private const string optionRawKey = "_option";

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

        // 語言設定為中文
        localization.SetCurrentLanguage(1);
        // 問題的數量
        int questionCount =  questionOptionCount.Length ;

        for (int questionId = 0; questionId < questionCount; questionId++)
        {
            Question q = new Question();
            q.id = questionId ;
            
            // 取得該問題的題目 
            string questionInfo_key = questionId + "_question";
            string questionInfo = LeanLocalization.GetTranslationText( questionInfo_key);
            q.questionInfo = questionInfo;

            // 取得該問題的選項
            int optionCount = questionOptionCount[questionId];
            List<string> options = new List<string>();
            for (int op = 0; op < optionCount; op++)
            {
                string option_key = questionId + optionRawKey + op ;
                string option = LeanLocalization.GetTranslationText(option_key);
                options.Add(option);
                Debug.Log(option_key + option );
            }

            q.options = options;
            questionsList.Add(q);
        }

        questionData.questionsList = questionsList;

        // // 問題的數量
        // // int questionCount =  csvLoader.GetQuetionCount();
        // int option_max_count = csvLoader.options[0].Length - 1; // 要去掉題目

        // for (int i = 0; i < questionCount; i++)
        // {
        //     Question q = new Question();
        //     q.id = i ;

        //     // 取的問題敘述，並將問題前的換行去掉
        //     string rawQuestionInfo = csvLoader.GetOptionsDataByRowAndColFrom(i, 0);
        //     string questionInfo =  RemoveChangeLine(rawQuestionInfo);
        //     q.questionInfo = questionInfo;

        //     // 取得該問題的選項
        //     List<string> options = new List<string>();
        //     for (int op = 0; op < option_max_count; op++)
        //     {
        //         string option = csvLoader.GetOptionsDataByRowAndColFrom(i, op + 1);
        //         if (option != ""){
        //             options.Add(option);
        //         }
        //     }
        //     q.options = options;
        //     questionsList.Add(q);
        // } 

        
    }

    private void SetAnswerData(){
        int questionCount =  questionOptionCount.Length ;
        int vTuberCount = csvLoader.GetVTuberCount(); // 取得VTuber數量

        List<Answerer> answerersList = new List<Answerer>();

        for (int vt = 0; vt < vTuberCount; vt++)
        {
            Answerer answerer = new Answerer();
            // 將VTuber前的換行去掉
            string rawName =  csvLoader.GetVTuberAnswersDataByRowAndColFrom(vt + 1, 0);
            answerer.name = RemoveChangeLine(rawName);
            
            // 設定Answer的腳色
            answerer.vTuber = (VTuber)vt;

            // 尋找VTuber的答案
            List<int> answers = new List<int>();
            for (int q = 0; q < questionCount; q++)
            {
                string vtAnswer = csvLoader.GetVTuberAnswersDataByRowAndColFrom(vt + 1,q +1);
                List<string> options = questionData.questionsList[q].options;

                bool hasAnswer = false;
                for (int option = 0; option < options.Count; option++)
                {
                    if(vtAnswer == options[option]){
                        answers.Add(option);
                        hasAnswer = true;
                        Debug.Log("vt"+ vt + "q" + q + vtAnswer );
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

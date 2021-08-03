using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using System;

public class QuestionDataManager : MonoBehaviour{
    // 資料庫
    public QuestionData questionData;
    public CsvLoader csvLoader;

    // 儲存玩家的答題情況
    // public List<int> playerAnswers;
    public Dictionary<VTuber, float> similarityDictionary;

    // 每一題的選項數量
    public static int[] questionOptionCount = 
    {2,2,3,2,2,
    2,3,2,2,2,
    3,2,2,5,3,
    2,2,2,8};
    private const string optionRawKey = "_option";


    // [ContextMenu("Test")]
    // public void Test(){
    //     ResultUI.ShowResult(playerAnswers);
    // }

    public Question GetQuestionById(int id){
        return questionData.GetQuestionById(id);
    }

    public List<VTuberSimilar> GetAllSimilarityWithSort(List<int> playerAnswers){
        List<VTuberSimilar> vTuberSimilars = new List<VTuberSimilar>();

        foreach (Answerer answerer in questionData.answerersList)
        {
            VTuber vTuber = answerer.vTuber;
            VTuberSimilar similar = GetVTuberSimilar(playerAnswers, answerer.answers, vTuber);
            vTuberSimilars.Add(similar);
        } 

        // Sort
        vTuberSimilars.Sort( (x, y) => - x.similarity.CompareTo(y.similarity)  );

        return vTuberSimilars;
    }


    public List<VTuberSimilar> GetMostSimilarVuber(List<int> playerAnswers){
        List<VTuberSimilar> most_answerer = new List<VTuberSimilar>();

        float high_similar = 0;
        foreach (Answerer answerer in questionData.answerersList)
        {
            VTuber vTuber = answerer.vTuber;
            VTuberSimilar similar = GetVTuberSimilar(playerAnswers, answerer.answers, vTuber);

            if(similar.similarity > high_similar){
                most_answerer.Clear();
                high_similar = similar.similarity;
                most_answerer.Add(similar);
            }
            else if (similar.similarity == high_similar)
            {
                most_answerer.Add(similar);
            }
        } 

        return most_answerer;
    }

    public VTuberSimilar GetVTuberSimilar(List<int> answers, List<int> vtAnswers, VTuber vTuber){
        float effectiveQuetionCount = 0;
        float sameQuetionCount = 0;
        
        if(answers.Count != vtAnswers.Count){
            Debug.Log($"玩家與VTuber的答題數不同\n玩家:{answers.Count} VTuber{vtAnswers.Count}");
            return null;
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
            return null;
        }

        // Debug.Log($"sameQuetionCount:{sameQuetionCount},effectiveQuetionCount{effectiveQuetionCount}");
        
        // 設定similar
        VTuberSimilar similar = new VTuberSimilar();
        float ratePercentage = (float) Math.Round(sameQuetionCount / effectiveQuetionCount*100);
        similar.similarity = ratePercentage;
        similar.sameCount = sameQuetionCount;
        similar.compareCount = effectiveQuetionCount;
        similar.vTuber =  vTuber ;
        similar.SetSprites(GetVtuberSprites(vTuber)) ;

        string nameID = "HoloMember/" + vTuber.ToString();
        // Debug.Log(nameID);
        similar.name = LeanLocalization.GetTranslationText(nameID ) ; 

        return similar;
    }

    public Sprite[] GetVtuberSprites(VTuber vTuber){
        foreach (VTuberOutcome answerer in questionData.vTuberOutcomesList)
        {
            if(vTuber == answerer.vTuber){
                return answerer.sprites;
            }
            
        }
        Debug.LogError($"找不到sprite{vTuber}");
        return null;
    }    


    /*
    public Dictionary<VTuber, float> CalculateAllSimilarity(List<int> playerAnswers){
        similarityDictionary = new Dictionary<VTuber, float>();
        foreach (Answerer answerer in questionData.answerersList)
        {
            VTuber vTuber = answerer.vTuber;
            float rate = CalculateSimilarity(playerAnswers, answerer.answers);
            float ratePercentage = (float) Math.Round(rate*100);
            Debug.Log($"vTuber:{vTuber}的相似度為{ratePercentage}");
            similarityDictionary.Add(vTuber, ratePercentage);
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

    */

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

[System.Serializable]
public class VTuberSimilar{
    
    public VTuber vTuber;
    public float similarity;
    public float sameCount;
    public float compareCount;
    public string name;
    public Sprite[] sprites;

    public void SetSprites(Sprite[] sprites){
        this.sprites = sprites;
    }
        
}


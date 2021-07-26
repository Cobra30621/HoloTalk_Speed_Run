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
    public List<int> playerAnswers;
    public Dictionary<VTuber, float> similarityDictionary;

    [ContextMenu("Test")]
    public void Test(){
        CalculateAllSimilarity(playerAnswers);
        
    }

    public Question GetQuestionById(int id){
        return questionData.GetQuestionById(id);
    }

    public List<VTuberSimilar> GetAllSimilarityWithSort(List<int> playerAnswers){
        Dictionary<VTuber, float> similarityDictionary = CalculateAllSimilarity(playerAnswers);
        List<VTuberSimilar> answerers = new List<VTuberSimilar>();
        foreach (KeyValuePair<VTuber, float> vtuber in similarityDictionary)
        {
            VTuberSimilar answerer = new VTuberSimilar(vtuber.Key, vtuber.Value, 0);
            answerer.name = vtuber.Key.ToString(); // 先隨便寫，之後要雙語實改
            answerer.SetSprites(GetVtuberSprites(vtuber.Key)) ;
            answerers.Add(answerer);
        }

        // Sort
        answerers.Sort( (x, y) => - x.similarity.CompareTo(y.similarity)  );

        return answerers;
    }

    public List<VTuberSimilar> GetMostSimilarVuber(List<int> playerAnswers){
        Dictionary<VTuber, float> similarityDictionary = CalculateAllSimilarity(playerAnswers);
        List<VTuberSimilar> most_answerer = new List<VTuberSimilar>();

        float high_similar = 0;
        foreach (KeyValuePair<VTuber, float> vtuber in similarityDictionary)
        {
            VTuberSimilar answerer = new VTuberSimilar(vtuber.Key, vtuber.Value, 0);
            answerer.name = vtuber.Key.ToString(); // 先隨便寫，之後要雙語實改
            answerer.SetSprites(GetVtuberSprites(vtuber.Key)) ;


            if(vtuber.Value > high_similar){
                most_answerer.Clear();
                high_similar = vtuber.Value;
                most_answerer.Add(answerer);
            }
            else if (vtuber.Value == high_similar)
            {
                most_answerer.Add(answerer);
            }
        }

        return most_answerer;
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

[System.Serializable]
public class VTuberSimilar{
    
    public VTuber vTuber;
    public float similarity;
    public float similarity_skip;
    public string name;
    public Sprite[] sprites;

    public VTuberSimilar(VTuber vTuber, float similarity, float similarity_skip){
        this.vTuber = vTuber;
        this.similarity = similarity;
        this.similarity_skip = similarity_skip;
    }

    public void SetSprites(Sprite[] sprites){
        this.sprites = sprites;
    }
        
}


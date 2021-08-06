using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using System;

public class VTuberSimilarityCalculator : MonoBehaviour{
    public QuestionData questionData;


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

        // 設定similar
        VTuberSimilar similar = new VTuberSimilar();
        float ratePercentage = (float) Math.Round(sameQuetionCount / effectiveQuetionCount*100);
        similar.similarity = ratePercentage;
        similar.sameCount = sameQuetionCount;
        similar.compareCount = effectiveQuetionCount;
        similar.vTuber =  vTuber ;
        similar.SetSprites(GetVtuberSprites(vTuber)) ;

        string nameID = "HoloMember/" + vTuber.ToString();
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


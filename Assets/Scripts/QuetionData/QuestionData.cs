
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
[CreateAssetMenu(fileName = "QuestionData", menuName = "ScriptableObjects/QuestionData")]
public class QuestionData: ScriptableObject {
    public List<Question> questionsList;
    public List<Answerer> answerersList;
    public List<VTuberOutcome> vTuberOutcomesList;

    public Question GetQuestionById(int id){
        int questionNum = questionsList.Count;
        if(id >= questionNum){
            Debug.LogError($"輸入的id{id}超過題目數量{questionNum}");
            return null;
        }
        else{
            return questionsList[id];
        }
    }

    public List<int> GetAnswersListByVTuber(VTuber vTuber){
        foreach (Answerer answerer in answerersList)
        {
            if(answerer.vTuber == vTuber){
                return answerer.answers;
            }
        }

        Debug.LogError($"找不到{vTuber}的資料");
        return null;
    }
}

// ==================================
// 資料結構
// ==================================

//Datastructure for storeing the quetions data
[System.Serializable]
public class Question // 一道題目
{
    public int id;
    public string questionInfo;         //question text
    public List<string> options;        //options to select

}


[System.Serializable]
public class Answerer{ // 一個角色的答案
    public VTuber vTuber;
    public string name;
    // option_id
    public List<int> answers; 
}

[System.Serializable]
public class VTuberOutcome{ // 一個角色的答案
    public VTuber vTuber;
    public Sprite[] sprites;
    public bool useCover = false;
}

[System.Serializable]
public enum Language{
    Chinese,English
}
[System.Serializable]
public enum VTuber{ 
    Shion = 0, Subaru, Miko, Mel, Aki,
    Suisei, Roboco, A,  Coco, Matsuri, 
    AZKi, Haachama, Kiara , Sora, Choco
} 

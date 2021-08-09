
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
[CreateAssetMenu(fileName = "KiaraResponceData", menuName = "ScriptableObjects/KiaraResponceData")]
public class KiaraResponceData: ScriptableObject {
    public List<KiaraResponce> questioningResponces;
    public List<KiaraResponce> answeredResponces;
}

[System.Serializable]
public class KiaraResponce{
    public string description;
    public int questionId;
    public int answer;
    public KiaraState kiaraState;
    public KiaraSFX kiaraSFX;
    
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public QuestionDataManager questionDataManager;

    [Header("UI")]
    public Text lab_answerCount;
    public Text lab_skipCount;
    public Text lab_similarity;
    public Text lab_similarity_skip;
    public Text lab_vubter;
    public Image img_vtuber;

    [Header("Info")]
    public int answerCount;
    public int skipCount;
    public float similarity;
    public float similarity_skip;
    public List<VTuberSimilar> most_simliarVTuber;
    public List<int> playerAnswers;


    [ContextMenu("Test")]
    public void Test(){
        SetOutCome();
    }


    public void SetOutCome(){
        StartCoroutine(ShowCoroutine());
        
    }

    IEnumerator ShowCoroutine()
    {
        most_simliarVTuber = questionDataManager.GetMostSimilarVuber(playerAnswers);
        VTuber vTuber = most_simliarVTuber[0].vTuber;
        similarity = most_simliarVTuber[0].similarity;

        lab_similarity.text = (similarity * 100) + "%";
        lab_vubter.text = vTuber.ToString();
        Debug.Log("(similarity * 100)"+ (similarity * 100));
        img_vtuber.sprite = questionDataManager.GetVtuberSprite(vTuber);

        // matsuriSpeech1.text = localize ? LeanLocalization.GetTranslationText(text) : text;
        yield return null;
    }
}

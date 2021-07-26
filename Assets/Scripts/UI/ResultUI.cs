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

    [Header("Detail")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private Transform bar_pos;
    private List<SimilarityBar> similarityBars;


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

        lab_similarity.text = (similarity ) + "%";
        lab_vubter.text = most_simliarVTuber[0].name;
        Debug.Log("(similarity * 100)"+ (similarity ));
        img_vtuber.sprite = most_simliarVTuber[0].sprites[0];

        // matsuriSpeech1.text = localize ? LeanLocalization.GetTranslationText(text) : text;
        yield return null;
    }


    public void ShowDetailPanel(){
        detailPanel.SetActive(true);
        RemoveAllBar();

        List<VTuberSimilar> allSimliaritys = questionDataManager.GetAllSimilarityWithSort(playerAnswers);
        similarityBars= new List<SimilarityBar>();
        foreach (VTuberSimilar simliarity in allSimliaritys)
        {
            CreateSimilarityBar(simliarity);
        }
    }

    public void CloseDetailPanel(){
        detailPanel.SetActive(false);
    }

    public void CreateSimilarityBar(VTuberSimilar simliarity )
    {
        var g = Instantiate(barPrefab, bar_pos);

        var l = g.GetComponent<SimilarityBar>();
        l.SetInfo( simliarity ); 
        similarityBars.Add( l);
    }

    private void RemoveAllBar(){
        if(similarityBars == null)
            return;
        
        foreach(SimilarityBar bar in similarityBars){
            if(bar != null)
                Destroy(bar.gameObject);
        }
    }

}

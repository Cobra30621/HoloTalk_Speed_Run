using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public static ResultUI resultUI;
    public VTuberSimilarityCalculator vTuberSimilarityCalculator;

    [Header("UI")]
    public GameObject outcomePanel;
    public Text lab_similarity;
    public Text lab_vubter;
    public Image img_vtuber;
    public Image img_percentage;

    [Header("Info")]
    public List<VTuberSimilar> most_simliarVTuber;
    public List<int> playerAnswers;

    [Header("Detail")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private Transform bar_pos;
    private List<SimilarityBar> similarityBars;

    void Awake(){
        resultUI = this;
    }

    [ContextMenu("Test")]
    public void Test(){
        SetOutCome(playerAnswers);
    }


    public static void ShowResult(List<int> playerAnswers)
    {
        resultUI.SetOutCome(playerAnswers);
    }

    public void SetOutCome(List<int> playerAnswers){
        outcomePanel.SetActive(true);
        StartCoroutine(ShowCoroutine(playerAnswers));
        
    }

    IEnumerator ShowCoroutine(List<int> playerAnswers)
    {
        this.playerAnswers = playerAnswers;
        most_simliarVTuber = vTuberSimilarityCalculator.GetMostSimilarVuber(playerAnswers);
        VTuber vTuber = most_simliarVTuber[0].vTuber;
        float similarity = most_simliarVTuber[0].similarity;

        lab_similarity.text = (similarity ) + "%";
        lab_vubter.text = most_simliarVTuber[0].name;
        img_percentage.fillAmount = similarity / 100f;
        Debug.Log("(similarity * 100)"+ (similarity ));
        img_vtuber.sprite = most_simliarVTuber[0].sprites[0];

        // matsuriSpeech1.text = localize ? LeanLocalization.GetTranslationText(text) : text;
        yield return null;
    }


    public void ShowDetailPanel(){
        detailPanel.SetActive(true);
        RemoveAllBar();

        List<VTuberSimilar> allSimliaritys = vTuberSimilarityCalculator.GetAllSimilarityWithSort(playerAnswers);
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

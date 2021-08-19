using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    public GameObject buttonPanel;

    [Header("Info")]
    public CharacterResult characterResult;
    public List<VTuberSimilar> most_simliarVTuber;
    public List<int> playerAnswers;

    [Header("Detail")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private Transform bar_pos;
    private List<SimilarityBar> similarityBars;

    public GoogleSheetRecorder googleSheetRecorder;

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
        InitResultInfo();

        this.playerAnswers = playerAnswers;
        most_simliarVTuber = vTuberSimilarityCalculator.GetMostSimilarVuber(playerAnswers);
        VTuber vTuber = most_simliarVTuber[0].vTuber;
        float similarity = most_simliarVTuber[0].similarity;

        // 照片顯示
        yield return new WaitForSeconds(0.5f);
        img_vtuber.gameObject.SetActive(true);
        yield return characterResult.Show(most_simliarVTuber[0].sprites[0]);
        // img_vtuber.sprite = most_simliarVTuber[0].sprites[0];

        // 其他資訊顯示
        yield return new WaitForSeconds(0.5f);
        // 顯示明子
        lab_vubter.text = most_simliarVTuber[0].name;
        lab_vubter.transform.rotation = Quaternion.Euler(0,0,45f);
        yield return lab_vubter.transform.DORotateQuaternion(Quaternion.Euler(0,0,0), 0.48763f);
        lab_vubter.transform.localScale = Vector3.one * 2.22f;
        yield return lab_vubter.transform.DOScale(1, 0.48763f).WaitForCompletion();
        yield return new WaitForSeconds(0.5f);

        // 顯示像似度進度條
        img_percentage.DOFillAmount(similarity / 100f, 1f);
        yield return new WaitForSeconds(1.3f);
        // .DOFillAmount(rate, progressBarAddTime)

        // 顯示像似度文字
        lab_similarity.text = (similarity ) + "%";
        lab_similarity.transform.localScale = Vector3.one * 2f;
        yield return lab_similarity.transform.DOScale(1, 0.5f).WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        
        // 顯示按鈕
        buttonPanel.SetActive(true);
        
        RecordOutcomeToGoogleSheet(most_simliarVTuber[0].name, (int)similarity);

        // matsuriSpeech1.text = localize ? LeanLocalization.GetTranslationText(text) : text;
        yield return null;
    }

    private void InitResultInfo(){
        // 其他資訊顯示
        img_vtuber.gameObject.SetActive(false);
        buttonPanel.SetActive(false);
        img_percentage.fillAmount = 0;
        lab_similarity.text = "";
        lab_vubter.text = "";
    }

    private void RecordOutcomeToGoogleSheet(string vtuber, int similiarity){
        googleSheetRecorder.RecordOutcome(vtuber, similiarity);
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

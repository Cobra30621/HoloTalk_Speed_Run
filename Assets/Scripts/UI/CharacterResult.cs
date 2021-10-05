using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI; 
 
public class CharacterResult : MonoBehaviour 
{ 
    public QuestionData questionData; 
    private List<VTuberOutcome> rawVTuberOutcomesList; 
    private List<VTuberOutcome> vTuberOutcomesList; 
    public int showSpriteNum = 10; 
    public float shortInterval = 0.1f; 
    public float longInterval = 0.4f; 
 
    // UI 
    public Image image; 
    public Image img_cover; 
 
    public bool similarityUseCover; 
 
    // 垃圾寫法 
    private void Init(Sprite similiarVTuberSprite) 
    { 
        rawVTuberOutcomesList = questionData.vTuberOutcomesList; 
        vTuberOutcomesList = new List<VTuberOutcome>(); 
 
        foreach (VTuberOutcome outcome in rawVTuberOutcomesList) 
        { 
            if (outcome.sprites[0] == similiarVTuberSprite) 
            { 
                similarityUseCover = outcome.useCover; 
            } 
            else 
            { 
                vTuberOutcomesList.Add(outcome); 
            } 
        } 
    } 
 
    public IEnumerator Show(Sprite similiarVTuberSprite) 
    { 
        Init(similiarVTuberSprite); 
 
        int f = Random.Range(0, vTuberOutcomesList.Count); 
 
        for (int i = 0; i < showSpriteNum; i++) 
        { 
            int index = GetIndex(f + i); 
 
            bool useCover = vTuberOutcomesList[index].useCover; 
            if (useCover) 
            { 
                image.gameObject.SetActive(false); 
                img_cover.sprite = vTuberOutcomesList[index].sprites[0]; 
            } 
            else 
            { 
                image.gameObject.SetActive(true); 
                image.sprite = vTuberOutcomesList[index].sprites[0]; 
            } 
 
            yield return new WaitForSeconds(shortInterval); 
        } 
 
        for (int i = 0; i < 3; i++) 
        { 
            int index = GetIndex(f + i); 
            bool useCover = vTuberOutcomesList[index].useCover; 
            if (useCover) 
            { 
                image.gameObject.SetActive(false); 
                img_cover.sprite = vTuberOutcomesList[index].sprites[0]; 
            } 
            else 
            { 
                image.gameObject.SetActive(true); 
                image.sprite = vTuberOutcomesList[index].sprites[0]; 
            } 
            yield return new WaitForSeconds(longInterval); 
        } 
 
        yield return new WaitForSeconds(0.2f); 
 
        if (similarityUseCover) 
        { 
            image.gameObject.SetActive(false); 
            img_cover.sprite = similiarVTuberSprite; 
        } 
        else 
        { 
            image.gameObject.SetActive(true); 
            image.sprite = similiarVTuberSprite; 
        } 
    } 
 
    private int GetIndex(int index) 
    { 
        return index % vTuberOutcomesList.Count; 
    } 
}
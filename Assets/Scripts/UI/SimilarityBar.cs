using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimilarityBar : MonoBehaviour
{
    [SerializeField] private Image img_vtuber;
    [SerializeField] private Image img_cover;
    [SerializeField] private Image img_bg;
    [SerializeField] private Text lab_name;
    [SerializeField] private Text lab_simility;
    [SerializeField] private Text lab_sameCount;
    [SerializeField] private Text lab_compareCount;

    // Start is called before the first frame update
    public void SetInfo(VTuberSimilar vTuberSimilar){
        bool useCover = vTuberSimilar.useCover;
        if(useCover){
            img_vtuber.gameObject.SetActive(false);
            img_cover.sprite = vTuberSimilar.sprites[0];
        }
        else{
            img_vtuber.gameObject.SetActive(true);
            img_cover.sprite = vTuberSimilar.sprites[0];
        }

        img_vtuber.sprite = vTuberSimilar.sprites[0];
        lab_name.text = vTuberSimilar.name;
        lab_simility.text = vTuberSimilar.similarity + "%";
        lab_sameCount.text = vTuberSimilar.sameCount + "";
        lab_compareCount.text = vTuberSimilar.compareCount + "";
    }

    public void SetBG(Sprite sprite){
        img_bg.sprite = sprite;
    }   
    
}

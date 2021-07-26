using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimilarityBar : MonoBehaviour
{
    [SerializeField] private Image img_vtuber;
    [SerializeField] private Text lab_name;
    [SerializeField] private Text lab_simility;
    [SerializeField] private Text lab_simility_skip;

    // Start is called before the first frame update
    public void SetInfo(VTuberSimilar vTuberSimilar){
        img_vtuber.sprite = vTuberSimilar.sprites[0];
        lab_name.text = vTuberSimilar.name;
        lab_simility.text = vTuberSimilar.similarity + "%";
        lab_simility_skip.text = vTuberSimilar.similarity_skip + "%";
    }
    
}

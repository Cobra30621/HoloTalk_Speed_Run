using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Lean.Localization;
public class CreditsUI : MonoBehaviour
{
    public BoardPanel boardPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowCreditsUI(){
        boardPanel.ShowPanel();
    }

    public void CloseOptionUI(){
        boardPanel.ClosePanel();
        // SaveSettings();
    }
}

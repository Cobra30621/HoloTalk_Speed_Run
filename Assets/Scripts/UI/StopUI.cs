using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StopUI : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    [SerializeField] public BoardPanel boardPanel;

    public void ShowStopPanel(){
        boardPanel.ShowPanel();
    }

    public void CloseStopPanel(){
        boardPanel.ClosePanel();
    }

    public void ShowOptionPanel(){
        optionPanel.SetActive(true);
    }

    public void CloseOptionPanel(){
        optionPanel.SetActive(false);
    }

    public void Again(){
        SceneManager.LoadScene("Game");
    }

    public void BackToTitle(){
        SceneManager.LoadScene("StartScene");
    }
}

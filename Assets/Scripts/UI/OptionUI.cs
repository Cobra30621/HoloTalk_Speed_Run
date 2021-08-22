using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OptionUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackToStart()
    {
        SceneManager.LoadScene("StartScene");
    }
    //控制全螢幕
    public void SetFullScreen(bool IsOn)
    {
        if(IsOn)
        {
            print("On");
            Screen.fullScreen=true;
        }
        else
        {
            print("Off");
            Screen.fullScreen=false;
        }
    }
    //重設成預設值
    public void ResetButton()
    {
        
    }
    public void LanguageChange()
    {
        
    }
}

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
            GameSettings.fullScreen=1;
        }
        else
        {
            print("Off");
            GameSettings.fullScreen=0;
        }
    }
    public void SetBGMValue(float newvalue)
    {
        GameSettings.bgmVolume=newvalue;
    }
    public void SetSFXValue(float newvalue)
    {
        GameSettings.sfxVolume=newvalue;
    }
    //重設成預設值
    public void ResetButton()
    {
        GameSettings.sfxVolume=0.5f;
        GameSettings.bgmVolume=0.5f;
        GameSettings.fullScreen=1;
    }
    public void LanguageChange()
    {
        
    }
}

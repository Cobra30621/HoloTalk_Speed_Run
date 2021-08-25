using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Lean.Localization;


public class OptionUI : MonoBehaviour
{
    public BoardPanel boardPanel;
    public Toggle fullScreenUI;
    public Dropdown langUI;
    public Scrollbar sfxVolumeUI, bgmVolumeUI;
    public LeanLocalization localization;
    // Start is called before the first frame update
    void Start()
    {
        LoadSettings();
        UpdateSettingsUI();
    }

    // Update is called once per frame
    void Update()
    {
        GameSettings.fullScreen = fullScreenUI.isOn ? 1 : 0;
        GameSettings.sfxVolume = sfxVolumeUI.value;
        GameSettings.bgmVolume = bgmVolumeUI.value;
        GameSettings.lang = langUI.value;
    }

    public void ShowOptionUI(){
        boardPanel.ShowPanel();
        UpdateSettingsUI();
    }

    public void CloseOptionUI(){
        boardPanel.ClosePanel();
        SaveSettings();
    }

    //控制全螢幕
    public void SetFullScreen()
    {
        // if(IsOn)
        // {
        //     print("On");
        //     Screen.fullScreen=true;
        // }
        // else
        // {
        //     print("Off");
        //     Screen.fullScreen=false;
        // }
        Screen.fullScreen = GameSettings.fullScreen == 1 ? true : false;
    }
    //重設成預設值
    public void ResetButton()
    {
        GameSettings.ResetToDefaults();
        UpdateSettingsUI();
    }

    public void SaveSettings()
    {
        GameSettings.SaveSettings();
    }

    public void LoadSettings()
    {
        GameSettings.LoadSettings();
    }

    void UpdateSettingsUI()
    {
        fullScreenUI.isOn = GameSettings.fullScreen == 1 ? true : false;
        sfxVolumeUI.value = GameSettings.sfxVolume;
        bgmVolumeUI.value = GameSettings.bgmVolume;
        langUI.value = GameSettings.lang;
        localization.SetCurrentLanguage(GameSettings.lang);
    }

}

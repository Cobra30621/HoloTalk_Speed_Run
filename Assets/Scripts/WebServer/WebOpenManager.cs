using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class WebOpenManager : MonoBehaviour
{

    // 推特用
    private float similiarity ;
    private string vtuber;
    private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";

    // 換行用 %0a
    private string twitterMainText ;
    private string GAME_LINK = "https://cobra3279.itch.io/holotalk-speed-round";
    private string twittertags = "&hashtags=holoTalkSpeedRound%2Cartsofashes";
    

    // 表單用
    private string googleFormUrl = "https://docs.google.com/forms/d/e/1FAIpQLSdJ8RsflPbnXWhRSSfDUFkC9SpHl8wn39CSeR7s4tE4wuuXWA/viewform";

    
    public void OpenGoogleForm(){
#if !UNITY_EDITOR
        OpenTab(googleFormUrl); 
#else
        Application.OpenURL(googleFormUrl);
#endif
    }

    public void PressedTwitterButton(){
        // score = Manager.current.GetScore();  HoloTalk Speed Round!%0a 
        twitterMainText = $"HoloTalk Speed Round!%0aI'm {similiarity} percent similar to {vtuber}!";
#if !UNITY_EDITOR
        OpenTab(TWITTER_ADDRESS + "?text=" + twitterMainText + "%0a" +
        GAME_LINK + "%0a%0a" + twittertags); 
#else
        Application.OpenURL(TWITTER_ADDRESS + "?text=" + twitterMainText + "%0a" +
        GAME_LINK + "%0a%0a" + twittertags);
#endif
    }

    public void SetTwitterInfo(string vTuber, float similarity){
        this.vtuber = vTuber;
        this.similiarity = similarity;

    }

    [DllImport("__Internal")]
    private static extern void OpenTab(string url);
    
}

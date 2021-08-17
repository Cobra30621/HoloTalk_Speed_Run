using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class WebOpenManager : MonoBehaviour
{

    // 推特用
    private float score ;
    private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";

    // 換行用 %0a
    private string twitterMainText ;
    private string GAME_LINK = "https://cky2433.itch.io/dd-shooter-enhanced-edition";
    private string twittertags = "&hashtags=hololive%2CDDshooter";

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
        // score = Manager.current.GetScore(); 
        twitterMainText = $"DD Send SC!%0aDonated {score} Super Chats!";
#if !UNITY_EDITOR
        OpenTab(TWITTER_ADDRESS + "?text=" + twitterMainText + "%0a" +
        GAME_LINK + "%0a%0a" + twittertags); 
#else
        Application.OpenURL(TWITTER_ADDRESS + "?text=" + twitterMainText + "%0a" +
        GAME_LINK + "%0a%0a" + twittertags);
#endif
    }

    [DllImport("__Internal")]
    private static extern void OpenTab(string url);
    
}

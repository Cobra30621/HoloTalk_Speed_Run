using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Kiara kiara;
    public string info = "Kiara/Intro1";
    public string anime = "exciting";

    [ContextMenu("Say")]
    public void Say(){
        kiara.SetKiaraText(info);
    }

    [ContextMenu("SetAnime")]
    public void SetAnime(){
        kiara.SetKiaraAnime(anime);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //控制全螢幕
    public void OnClick(bool IsOn)
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
}

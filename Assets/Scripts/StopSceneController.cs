using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StopSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //按下空白鍵返回Game
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //直接複製並修改即可按下空白鍵即暫停
            SceneManager.LoadScene("Game");
        }
    }
}

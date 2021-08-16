using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetRecorder : MonoBehaviour
{
    public static int id = 23232;
    public static string similiarVTuber = "CoCo";
    public static int similiarity = 88;

    private string url = "https://script.google.com/macros/s/AKfycbwiNp9MsH6uz0gWol70Ge5Pn9dORf96RnrgpxLffnOu9bdZDbUNMknfKmXRgzc2r7lj/exec";


    public bool isEditMode;
    // Start is called before the first frame update
    void Start()
    {
        SetUserID();
        // RecordOutcome("JOJO", 32);
    }

    private void SetUserID(){
        id = PlayerPrefs.GetInt("userID", -1);

        if(id == -1){
            id = Random.Range(1, 1000000); // 隨機產生ID
            PlayerPrefs.SetInt("userID", id);
        }
    }



    public void RecordOutcome(string in_similiarVTuber, int in_similiarity){
        if(isEditMode){return;}

        similiarVTuber = in_similiarVTuber;
        similiarity = in_similiarity;
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("similiarVtuber", similiarVTuber);
        form.AddField("similiarity", similiarity);
        form.AddField("time", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}

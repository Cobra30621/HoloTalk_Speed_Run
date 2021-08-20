using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BoardPanel : MonoBehaviour
{
    public GameObject panel;

    public Transform startPos;
    public Transform endPos;
    public Ease moveEase;
    public float panelAnchorMinY;

    void Start(){
        // 設置到開始處
        endPos.position = panel.GetComponent<RectTransform>().position;
        // Test();
    }

    [ContextMenu("ShowPanel")]
    public void ShowPanel(){
        StartCoroutine(Show());
    }

    [ContextMenu("ClosePanel")]
    public void ClosePanel(){
        StartCoroutine(Close());
    }

    public IEnumerator Show(){
        panel.SetActive(true);
        panel.GetComponent<RectTransform>().position = new Vector3(startPos.position.x, endPos.position.y, endPos.position.z);
        panel.GetComponent<RectTransform>().DOAnchorMin(new Vector2(0, panelAnchorMinY), 0, false);

        // 移動面板
        // yield return new WaitForSeconds(1f);
        // panel.GetComponent<RectTransform>().position = new Vector3(vec_outcome.x, vec_outcome.y, vec_outcome.z);
        panel.transform.DOMoveX(endPos.position.x, 0.5f).SetEase(moveEase);
        yield return new WaitForSeconds(0.7f);
        panel.GetComponent<RectTransform>().DOAnchorMin(new Vector3(0, 0f), 0.5f, false).SetEase(moveEase);
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator Close(){
        
        panel.GetComponent<RectTransform>().DOAnchorMin(new Vector2(0, panelAnchorMinY), 0.5f, false).SetEase(moveEase);
        yield return new WaitForSeconds(0.7f);
        panel.transform.DOMoveX(startPos.position.x, 0.5f).SetEase(moveEase);
        yield return new WaitForSeconds(1f);
        panel.SetActive(false);
    }

}

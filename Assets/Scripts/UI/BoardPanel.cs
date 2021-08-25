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
    public GameObject[] showObjects;
    public GameObject GOMask; // 開啟介面的過程中，無法點擊其他東西

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
        SetShowPanelActive(false);
        GOMask.SetActive(true);

        panel.GetComponent<RectTransform>().position = new Vector3(startPos.position.x, endPos.position.y, endPos.position.z);
        panel.GetComponent<RectTransform>().DOAnchorMin(new Vector2(0, panelAnchorMinY), 0, false);

        // 移動面板
        // yield return new WaitForSeconds(1f);
        // panel.GetComponent<RectTransform>().position = new Vector3(vec_outcome.x, vec_outcome.y, vec_outcome.z);
        panel.transform.DOMoveX(endPos.position.x, 0.5f).SetEase(moveEase);
        yield return new WaitForSeconds(0.7f);
        panel.GetComponent<RectTransform>().DOAnchorMin(new Vector3(0, 0f), 0.5f, false).SetEase(moveEase);
        yield return new WaitForSeconds(0.5f);

        SetShowPanelActive(true);
    }

    public IEnumerator Close(){
        SetShowPanelActive(false);
        
        panel.GetComponent<RectTransform>().DOAnchorMin(new Vector2(0, panelAnchorMinY), 0.5f, false).SetEase(moveEase);
        yield return new WaitForSeconds(0.7f);
        panel.transform.DOMoveX(startPos.position.x, 0.5f).SetEase(moveEase);
        yield return new WaitForSeconds(0.5f);
        panel.SetActive(false);
        GOMask.SetActive(false);
    }

    private void SetShowPanelActive(bool bo){
        foreach (GameObject item in showObjects)
        {
            item.SetActive(bo);
        }
    }

}

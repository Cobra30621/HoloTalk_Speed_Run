using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KiaraToucher : MonoBehaviour
{
    
    public Kiara kiara;
    public SFXManager sFX;
    public KiaraAction[] kiaraActions;

    public void OnClick(){
        int f = Random.Range(0, kiaraActions.Length);
        sFX.PlaySFX((int) kiaraActions[f].kiaraSFX);
        kiara.SetKiaraAnime(kiaraActions[f].kiaraState);
        StartCoroutine(BackToNormal());
    }

    IEnumerator BackToNormal(){
        yield return new WaitForSeconds(1f);
        kiara.SetKiaraAnime(KiaraState.Idle);
    }

}

[System.Serializable]
public struct KiaraAction{
    public KiaraState kiaraState;
    public KiaraSFX kiaraSFX;
}

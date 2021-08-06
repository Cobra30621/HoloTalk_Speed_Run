using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnClick(){
        Scale();

        
    }

    public void Scale(){
        if(animator == null){return;}
        animator.SetTrigger("scale");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uitranform : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void onUIOpen()
    {
        animator.SetBool("Open", true);
    }
    
    public void onUiClose()
    {
        animator.SetBool("Open", false);
    }

}

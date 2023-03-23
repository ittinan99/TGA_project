using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Outline outline;
    private bool isMouseOver;
    private bool isShowing;

    public float showTime;

    [SerializeField]
    private GameObject vCam;

    [SerializeField]
    private Animator canvasAnim;

    public delegate void OnInteractWithObject();
    public OnInteractWithObject OnInteractWithObjectCallback;

    private Coroutine showCanvasCoroutine;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    void Start()
    {
        outline.OutlineWidth = 0;
    }

    void Update()
    {
        if(isMouseOver && Input.GetMouseButton(0))
        {
            vCam.SetActive(true);
            isShowing = true;
            outline.OutlineWidth = 0;
            showCanvas();

            OnInteractWithObjectCallback?.Invoke();
        }
        else if(Input.GetMouseButton(1))
        {
            DisableVCam();
            hideCanvas();
        }
    }

    private void showCanvas()
    {
        canvasAnim.SetBool("show", true);
    }

    private void hideCanvas()
    {
        canvasAnim.SetBool("show", false);
    }

    private void DisableVCam()
    {
        vCam.SetActive(false);
        isShowing = false;
    }

    private void OnMouseOver()
    {
        if (isShowing) { return; }

        outline.OutlineWidth = 3;
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        outline.OutlineWidth = 0;
        isMouseOver = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Outline outline;
    private bool isMouseOver;

    [SerializeField]
    private GameObject vCam;

    public delegate void OnInteractWithObject();
    public OnInteractWithObject OnInteractWithObjectCallback;

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
            OnInteractWithObjectCallback?.Invoke();
        }
        else if(Input.GetMouseButton(1))
        {
            DisableVCam();
        }
    }

    private void DisableVCam()
    {
        vCam.SetActive(false);
    }

    private void OnMouseOver()
    {
        outline.OutlineWidth = 3;
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        outline.OutlineWidth = 0;
        isMouseOver = false;
    }
}

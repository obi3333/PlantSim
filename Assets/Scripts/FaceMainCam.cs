using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMainCam : MonoBehaviour
{
    public Camera mainCameraTransform;
    private Transform ooo;

    private void Start()
    {
        ooo = mainCameraTransform.transform;
    }

    void Update()
    {
        if (mainCameraTransform != null)
        {
            transform.LookAt(ooo);
        }
    }
}

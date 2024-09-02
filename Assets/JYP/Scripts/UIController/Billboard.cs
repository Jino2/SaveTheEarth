using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera targetCamera; 

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main; 
        }
    }

    void LateUpdate()
    {
        transform.forward = targetCamera.transform.forward;
    }
}
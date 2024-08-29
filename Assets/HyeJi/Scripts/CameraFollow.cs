using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public Transform lookAtTarget;
    public bool hasDelay = false;
    public float delaySpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lookAtTarget != null)
        {
            if (hasDelay)
            {
                // 살짝 느리게 쫓아간다
                transform.position = Vector3.Lerp(transform.position, followTarget.position, Time.deltaTime * delaySpeed);
            }
            else
            {
                transform.position = followTarget.position;
            }
        }

        if (lookAtTarget != null)
        {
            transform.rotation = lookAtTarget.rotation;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CameraFollow followCam;

    public Vector3 nearOffset;
    public Vector3 farOffset;
    public float distance = 5;
    public float rotSpeed = 300;

    Vector3 minLocalPos;
    Vector3 maxLocalPos;

    // 회전 값
    float rotY;
    float rotX;
    public bool useRotY;
    public bool useRotX;


    // 내가 다시 씀

    public Transform player;

    public float xSpeed = 120f;
    public float ySpeed = 120f;
    public float yminLimit = -20f;
    public float xminLimit = 80f;

    public float x = 0f;
    public float y = 0f;


    void Start()
    {
        // 메인 카메라
        followCam = Camera.main.transform.GetComponent<CameraFollow>();

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;


    }

    void Update()
    {
        // 마우스 움직임 값 받아오기
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // 회전 각도를 누적
        if (useRotY) rotY += mx * rotSpeed * Time.deltaTime;
        if (useRotX) rotX += my * rotSpeed * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -30, 30);
        rotY = Mathf.Clamp(rotY, -180, 180);

        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        transform.localEulerAngles = new Vector3(-rotX, rotY, 0);

    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}

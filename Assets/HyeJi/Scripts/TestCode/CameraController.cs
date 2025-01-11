using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    CameraFollow followCam;

    public Vector3 nearOffset;
    public Vector3 farOffset;
    public float distance = 5;
    public float rotSpeed = 300;


    // 내가 다시 씀
    public Transform player;

    public float xSpeed = 120f;
    public float ySpeed = 120f;
    public float yminLimit = -20f;
    public float ymaxLimit = 80f;

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
        if(player && EventSystem.current.currentSelectedGameObject == null)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

            y = ClampAngle(y, yminLimit, ymaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0, 0, -distance) + player.position;

            transform.rotation = rotation;
            transform.position = position;
        }
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

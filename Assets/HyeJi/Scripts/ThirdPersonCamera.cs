using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float rotSpeed = 200f;
    public Transform player;
    private CinemachineVirtualCamera virtualCamera;
    private float mouseX;
    private float mouseY;

    public float minDistance = 1.5f;
    public float maxDistance = 4f;
    public float currentDistance;

    public LayerMask collisionLayers;


    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if( virtualCamera == null)
        {
            Debug.LogError("버츄얼카메라내놔");
        }

        currentDistance = maxDistance;
    }

    void Update()
    {
        if(player == null)
        {
            return;
        }

        mouseX += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
        // 카메라 상하 각도 제한
        mouseY = Mathf.Clamp(mouseY, -35, 60);

        // RayCasting



        virtualCamera.transform.position = player.position;
        virtualCamera.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
    }
}

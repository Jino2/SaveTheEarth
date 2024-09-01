using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    public float rotSpeed = 300;

    private Transform CameraArm;

    // 플레이어 이동 입력 결과값 저장
    private Vector3 moveInput;


    public CinemachineVirtualCamera virtualCamera;
    public float rotationSpeed = 1f;
    private Transform cameraTransform;
    private CinemachineTransposer transposer;


    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        //offset = transform.position - player.position;     
    }

    // Update is called once per frame
    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X");
        cameraTransform.Rotate(0, mouseX * rotationSpeed, 0);
        transposer.m_FollowOffset = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * transposer.m_FollowOffset;


        //// 마우스의 입력 받기
        //moveInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //LookAround();

        //transform.position = player.position + offset;
        //transform.LookAt(player);
    }

    //void LookAround()
    //{
    //    Vector3 mouseDelta = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    //    Vector3 camAngle = CameraArm.rotation.eulerAngles;

    //    float x = camAngle.x - mouseDelta.y;
    //    if( x < 180f)
    //    {
    //        x = Mathf.Clamp(x, -1f, 45f);
    //    }
    //    else
    //    {
    //        x = Mathf.Clamp(x, 320f, 361f);
    //    }

    //    CameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    //}
}

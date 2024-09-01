using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerStateBase
{
    CharacterController cc;

    public Transform cameraTransform;

    // 회전 속도 조절할 변수
    public float rotationSpeed = 10f;

    // 점프에 관한 변수 
    public float jumpPower = 2f;
    private float gravity = -9.81f;
    private float yVelocity;
    public int jumpMaxCnt = 2;
    int jumpCurrCnt;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        //Vector3 dir = new Vector3(h, 0, v);

        // 카메라의 방향을 기준으로 이동 방향을 설정
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // 수평 방향만 고려하기
        forward.y = 0;
        right.y = 0;

        // 입력에 따라 이동할 방향을 계산
        Vector3 moveDir = forward * v + right * h;
        moveDir.Normalize();


        // 방향 벡터의 크기가 0이 아닐때만 회전
        if(moveDir.magnitude > 0.1f)
        {
            //Vector3 lookDir = dir.normalized;

            // 이동 방향으로 플레이어를 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 점프 로직
        // 땅에 있다면 yVelocity 를 0으로 초기화
        if(cc.isGrounded)
        {
            yVelocity = 0;
            jumpCurrCnt = 0;
        }
        // 스페이스바 누르면 점프
        // 스페이스바를 누르면 점프한다
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(jumpCurrCnt < jumpMaxCnt)
            {
                yVelocity = jumpPower;
                jumpCurrCnt++;
            }
        }
        // yVelocity를 중력값을 이용해서 감소시킨다.
        yVelocity += gravity * Time.deltaTime;
        // moveDir.y 값에 0 셋팅
        moveDir.y = yVelocity;

        // 이동
        cc.Move(moveDir * moveSpeed * Time.deltaTime);

        // 뛰는 함수
        WalkRun();

    }

    // 뛰자
    void WalkRun()
    {
        // 왼쪽 Shift 키를 누르면
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            // 뛰어
            SetWalkRun(true);
        }
        // 왼쪽 Shift 키를 떼면
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            // 걷자
            SetWalkRun(false);
        }
    }

    void SetWalkRun(bool isRun)
    {
        // isRun 이 true 이면 runSpeed, false 이면 walkSpeed
        moveSpeed = isRun ? runSpeed : walkSpeed;
    }
}

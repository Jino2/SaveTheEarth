using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : PlayerStateBase, IPunObservable
{
    // PhotonView
    PhotonView pv;

    // CharacterController
    CharacterController cc;
    // 카메라 위치 Transform
    public Transform cameraTransform;
    // 버츄얼 카메라
    private CinemachineVirtualCamera virtualCamera;
    // Animator
    Animator anim;

    // 회전 속도 조절할 변수
    public float rotationSpeed = 10f;

    // 점프에 관한 변수 
    public float jumpPower = 2f;
    private float gravity = -9.81f;
    private float yVelocity;
    public int jumpMaxCnt = 2;
    int jumpCurrCnt;

    // 움직이는 방향
    public Vector3 moveDir;

    // 뛰는 속도 주기
    float speedValue = 1;
    private bool isRunning = false;
    private float runningTime = 0;

    // 도착 위치
    Vector3 receivePos;
    // 회전해야하는 값
    Quaternion receiveRot;

    float inputValue;

    bool requestLoadLevel = false;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        pv = GetComponent<PhotonView>();
        print("111111");

        if(pv.IsMine)
        {
            // 메인 카메라 찾기
            print("545454545454545454");
            Camera mainCamera = Camera.main;
            if(mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
                print("메인 카메라 있다");
            }
            else
            {
                print("메인 카메라 내놔");
            }

            // 버츄얼 카메라 찾기
            var virtualCamera = FindObjectOfType<ThirdPersonCamera>();
            if(virtualCamera != null)
            {
                virtualCamera.SetPlayer(this.transform);
            }
            else
            {
                print("버츄얼 카메라 내놔");
            }
        }
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        //Vector3 dir = new Vector3(h, 0, v);

        // 카메라의 방향을 기준으로 이동 방향을 설정
        if (pv.IsMine)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // 수평 방향만 고려하기
            forward.y = 0;
            right.y = 0;

            // 입력에 따라 이동할 방향을 계산
            moveDir = forward * v + right * h;
            moveDir.Normalize();

            // 방향 벡터의 크기가 0이 아닐때만 회전
            if (moveDir.magnitude > 0.1f)
            {
                // 이동 방향으로 플레이어를 회전
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            WalkRun();

            // 점프 로직
            // 땅에 있다면 yVelocity 를 0으로 초기화
            if (cc.isGrounded)
            {
                yVelocity = 0;
                jumpCurrCnt = 0;
            }
            // 스페이스바 누르면 점프
            // 스페이스바를 누르면 점프한다
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpCurrCnt < jumpMaxCnt)
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

            // 애니메이션 연동처리
            if (anim != null)
            {
                anim.SetFloat("speed", moveDir.magnitude * speedValue);
            }

            // 뛰는 함수
            WalkRun();


        }
        // isMine 아닐때도 위치 동기화
        else
        {
            transform.position = receivePos;
            transform.rotation = receiveRot;

            // 애니메이션 연동처리
            if (anim != null)
            {
                anim.SetFloat("speed", inputValue);
            }
        }

        // 씬 이동
        if(!requestLoadLevel && SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(GoToMainy());
        }
    }
   

    // 뛰자
    void WalkRun()
    {
        // 왼쪽 Shift 키를 누르면
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            // 뛰어
            SetWalkRun(true);
            speedValue = 2f;
            isRunning = true;
            print("출력확인");
        }
        // 왼쪽 Shift 키를 떼면
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            // 걷자
            SetWalkRun(false);
            speedValue = 1f;
            isRunning = false;
            runningTime = 0f;
        }

        anim.SetFloat("speed", moveDir.magnitude * speedValue);
    }

    void SetWalkRun(bool isRun)
    {
        // isRun 이 true 이면 runSpeed, false 이면 walkSpeed
        moveSpeed = isRun ? runSpeed : walkSpeed;
    }

    IEnumerator GoToMainy()
    {
        // 방장이 z 버튼 활성화 시 씬 이동을 한다
        if(PhotonNetwork.IsMasterClient && pv.IsMine && Input.GetKeyDown(KeyCode.Z))
        {
            yield return new WaitForSeconds(2.0f);

            // 로비 씬으로 이동하자
            PhotonNetwork.LoadLevel(2);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // isWriting -> isMine 이 나 일때 주겠다
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            stream.SendNext(moveDir.magnitude * speedValue);
        }
        // isReading -> isMine 이 내가 아닐 때 주겠다
        else if(stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();

            inputValue = (float)stream.ReceiveNext();
        }
    }
}

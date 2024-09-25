using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mark : MonoBehaviourPun
{
    public TextMeshProUGUI textToToggle;  // TextMeshPro UI 요소
    public float triggerDistance = 3f;    // 플레이어와의 거리 조건
    public Transform playerTransform;     // 로컬 플레이어의 Transform
    private bool hasBeenToggledOff = false;  // 텍스트가 비활성화된 적이 있는지 체크
    private PhotonView pv;  // PhotonView 객체

    void Start()
    {
        // 처음엔 텍스트를 활성화 상태로 시작
        textToToggle.gameObject.SetActive(true);
        StartCoroutine(FindLocalPlayer());  // 로컬 플레이어 찾기 시작
    }

    // UI가 호출되면 이 함수를 통해 오브젝트 비활성화
    public void DeactivateMarkObject()
    {
        if (!hasBeenToggledOff && pv.IsMine)
        {
            // 텍스트 비활성화
            textToToggle.gameObject.SetActive(false);
            hasBeenToggledOff = true;

            // 오브젝트를 비활성화하거나 삭제
            PhotonNetwork.Destroy(gameObject); // 네트워크 상에서 삭제
        }
    }

    // 로컬 플레이어를 찾아서 playerTransform에 할당하는 코루틴
    private IEnumerator FindLocalPlayer()
    {
        while (playerTransform == null)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                PhotonView localPv = player.GetComponent<PhotonView>();
                if (localPv != null && localPv.IsMine)
                {
                    playerTransform = player.transform;  // 로컬 플레이어의 Transform 할당
                    pv = localPv;  // PhotonView를 저장하여 나중에 사용
                    break;
                }
            }
            yield return null;  // 다음 프레임까지 대기
        }
    }
}
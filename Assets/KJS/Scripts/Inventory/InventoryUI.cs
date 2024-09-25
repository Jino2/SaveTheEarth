using UnityEngine;
using TMPro;
using System.Collections.Generic;
using static GoodsInfo;
using Photon.Pun;  // GoodsInfo의 enum GoodsType을 사용하기 위해 static import

using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;

public class InventoryUI : MonoBehaviourPun
{
    public List<GameObject> prefabList;  // 드랍할 프리팹 리스트
    public Transform playerTransform;    // 플레이어의 Transform
    public Transform spawnPoint;         // 아이템이 생성될 스폰 포인트 위치
    public TextMeshProUGUI[] itemCountTexts; // 아이템 수량을 표시할 TextMeshPro 배열

    private bool isCountReduced = false;

    private void Start()
    {
        // 플레이어 Transform을 GameManager에서 가져오는 코루틴 시작
        StartCoroutine(AssignPlayerTransform());
    }

    // GameManager에서 플레이어의 Transform을 찾는 코루틴
    private IEnumerator AssignPlayerTransform()
    {
        // GameManager에서 플레이어 오브젝트를 찾을 때까지 반복
        while (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");  // 태그로 플레이어 오브젝트를 찾음
            if (player != null)
            {
                playerTransform = player.transform;  // 플레이어의 Transform을 할당
                Debug.Log("Player Transform assigned in InventoryUI.");
            }

            // 0.5초 간격으로 플레이어를 다시 찾음
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        // 숫자 키 입력에 따른 프리팹 생성
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateObject("Chest", GoodsType.Chest);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CreateObject("Sword", GoodsType.Sword);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CreateObject("Rock 1", GoodsType.Rock);
        }
    }

    // 프리팹을 생성하는 메서드 (숫자키 입력에 따른 아이템 생성)
    public void CreateObject(string prefabName, GoodsType goodsType)
    {
        Debug.Log($"Trying to create object: {prefabName}, GoodsType: {goodsType}");

        // 현재 아이템 수량 확인
        int currentCount = Inventory_KJS.instance.CurrentGoodsCount(goodsType);
        if (currentCount > 0 && prefabName != null && playerTransform != null)
        {
            // 인벤토리에서 아이템 사용 (아이템 삭제)
            UserApi.DeleteItemFromUserInventory(
                UserCache.GetInstance().Id,
                (int)goodsType,
                (res) => { }
            );

            // 스폰 포인트 또는 플레이어 앞에 아이템 생성
            Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 2f;
            spawnPosition.y -= 1f;

            // 네트워크 상에서 객체 생성
            GameObject spawnedPrefab = PhotonNetwork.Instantiate(prefabName, spawnPosition, Quaternion.identity);

            // PhotonView를 통해 로컬 플레이어가 소유한 객체인지 확인
            PhotonView photonView = spawnedPrefab.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                // 로컬 플레이어가 소유한 객체만 스케일 조정
                spawnedPrefab.transform.localScale *= 1f;

                // 아이템 수량 감소
                MinusCount(goodsType);
                isCountReduced = true;
                Invoke(nameof(ResetCountReduced), 0.1f);
            }
        }
        else
        {
            Debug.LogWarning("Insufficient item count or invalid player transform.");
        }
    }

    // 플래그 초기화 메서드
    private void ResetCountReduced()
    {
        isCountReduced = false;
    }

    // 수량을 1씩 감소시키고 UI를 업데이트하는 메서드
    public void MinusCount(GoodsType goodsType)
    {
        int currentCount = Inventory_KJS.instance.CurrentGoodsCount(goodsType);

        if (currentCount > 0)
        {
            Inventory_KJS.instance.SetGoodsCount(goodsType, currentCount - 1);
            UpdateText(goodsType);
        }
    }

    // 특정 GoodsType의 수량을 TextMeshPro에 업데이트하는 함수
    public void UpdateText(GoodsType goodsType)
    {
        int index = (int)goodsType - 6;

        if (index >= 0 && index < itemCountTexts.Length)
        {
            int count = Inventory_KJS.instance.CurrentGoodsCount(goodsType);
            itemCountTexts[index].text = $"{count}";
        }
    }
}
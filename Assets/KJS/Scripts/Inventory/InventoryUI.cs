﻿using UnityEngine;
using TMPro;
using System.Collections.Generic;
using static GoodsInfo;
using Photon.Pun;  // GoodsInfo의 enum GoodsType을 사용하기 위해 static import
using System.Collections;

public class InventoryUI : MonoBehaviourPun
{
    public List<GameObject> prefabList;
    public Transform spawnPoint;
    public TextMeshProUGUI[] itemCountTexts;  // GoodsType별로 수량을 표시할 TextMeshPro 배열
    private Transform playerTransform;  // PLAYER 오브젝트의 Transform 저장용 변수
    private bool isCountReduced = false;  // 수량 감소 중복 방지를 위한 플래그

    private Vector3 initialSpawnOffset;  // 최초 스폰 위치의 플레이어와의 오프셋
    private bool isFirstSpawn = true;    // 첫 번째 스폰 여부 체크

    void Awake()
    {
        // 비활성화된 상태에서도 로컬 Player 오브젝트의 Transform을 찾아 할당
        FindLocalPlayerTransform();

        // 이 스크립트가 붙어있는 오브젝트가 씬 변경 시 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);

        // 최초 스폰 위치 설정
        if (isFirstSpawn && spawnPoint != null)
        {
            initialSpawnOffset = spawnPoint.forward * 2f;
            isFirstSpawn = false;  // 최초 스폰 이후로는 false로 설정
        }
    }

    void FindLocalPlayerTransform()
    {
        // 모든 Player 오브젝트를 찾음
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObject in playerObjects)
        {
            PhotonView photonView = playerObject.GetComponent<PhotonView>();

            // PhotonView가 있고, 현재 로컬 플레이어가 소유한 객체라면
            if (photonView != null && photonView.IsMine)
            {
                playerTransform = playerObject.transform;

                // 로컬 플레이어의 Transform을 spawnPoint에 할당
                spawnPoint = playerTransform;
                break;  // 로컬 플레이어를 찾았으므로 반복문 종료
            }
        }
    }

    void OnEnable()
    {
        // 스크립트가 활성화될 때마다 로컬 Player Transform을 확인
        if (playerTransform == null)
        {
            FindLocalPlayerTransform();
        }
    }

    void Update()
    {
        if (playerTransform == null)
        {
            FindLocalPlayerTransform();
        }

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
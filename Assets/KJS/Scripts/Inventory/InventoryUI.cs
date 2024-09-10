using UnityEngine;
using TMPro;
using System.Collections.Generic;
using static GoodsInfo;  // GoodsInfo의 enum GoodsType을 사용하기 위해 static import

public class InventoryUI : MonoBehaviour
{
    public List<GameObject> prefabList;
    public Transform spawnPoint;
    public TextMeshProUGUI[] itemCountTexts;  // GoodsType별로 수량을 표시할 TextMeshPro 배열
    private Transform playerTransform;  // PLAYER 오브젝트의 Transform 저장용 변수
    private bool isCountReduced = false;  // 수량 감소 중복 방지를 위한 플래그

    private Vector3 initialSpawnOffset;  // 최초 스폰 위치의 플레이어와의 오프셋
    private bool isFirstSpawn = true;    // 첫 번째 스폰 여부 체크

    void Start()
    {
        // PLAYER 오브젝트의 Transform을 찾음 (인스펙터에서 할당하지 않고 코드에서 찾기)
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        // 이 스크립트가 붙어있는 오브젝트가 씬 변경 시 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);

        // 최초 스폰 위치 설정
        if (isFirstSpawn && playerTransform != null)
        {
            initialSpawnOffset = playerTransform.forward * 2f;
            isFirstSpawn = false;  // 최초 스폰 이후로는 false로 설정
        }
    }

    void Update()
    {
        // 숫자 키 입력에 따른 프리팹 생성
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateObjectByKey(0, GoodsType.Chest);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CreateObjectByKey(1, GoodsType.Sword);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CreateObjectByKey(2, GoodsType.Rock);
        }
    }

    // 숫자 키 입력에 따른 프리팹 생성 메서드
    public void CreateObjectByKey(int prefabIndex, GoodsType goodsType)
    {
        if (prefabIndex >= 0 && prefabIndex < prefabList.Count)
        {
            CreateObject(prefabList[prefabIndex], goodsType, false);  // false: 버튼 클릭 아님
        }
    }

    // OnClick 이벤트에 따른 프리팹 생성 메서드
    public void OnChestButtonClicked()
    {
        CreateObject(prefabList[0], GoodsType.Chest, true);  // true: 버튼 클릭
    }

    public void OnSwordButtonClicked()
    {
        CreateObject(prefabList[1], GoodsType.Sword, true);  // true: 버튼 클릭
    }

    public void OnRockButtonClicked()
    {
        CreateObject(prefabList[2], GoodsType.Rock, true);  // true: 버튼 클릭
    }

    // 프리팹을 생성하는 메서드
    void CreateObject(GameObject prefab, GoodsType goodsType, bool isButtonClick)
    {
        int currentCount = Inventory_KJS.instance.CurrentGoodsCount(goodsType);

        if (currentCount > 0 && prefab != null && playerTransform != null)
        {
            Vector3 spawnPosition;

            if (isFirstSpawn)
            {
                initialSpawnOffset = playerTransform.forward * 2f;
                spawnPosition = playerTransform.position + initialSpawnOffset;
                isFirstSpawn = false;
            }
            else
            {
                spawnPosition = playerTransform.position + (playerTransform.rotation * initialSpawnOffset);
            }

            GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);
            spawnedPrefab.transform.localScale *= 0.5f;

            if (isButtonClick)
            {
                MinusCount(goodsType);
                isCountReduced = true;
                Invoke(nameof(ResetCountReduced), 0.1f);
            }
            else
            {
                MinusCountByKey(goodsType);
            }
        }
    }

    // 플래그 초기화 메서드 (일정 시간이 지난 후 수량 감소 플래그를 초기화)
    private void ResetCountReduced()
    {
        isCountReduced = false;
    }

    // 버튼 클릭 시 호출되는 GoodsType의 수량을 1씩 감소시키고 UI를 업데이트하는 메서드
    public void MinusCount(GoodsType goodsType)
    {
        int currentCount = Inventory_KJS.instance.CurrentGoodsCount(goodsType);

        if (!isCountReduced)
        {
            if (currentCount > 0)
            {
                Inventory_KJS.instance.SetGoodsCount(goodsType, currentCount - 1);

                // UI 업데이트
                UpdateText(goodsType);
            }
        }
    }

    // 숫자 키 입력 시 호출되는 GoodsType의 수량을 1씩 감소시키고 UI를 업데이트하는 메서드
    public void MinusCountByKey(GoodsType goodsType)
    {
        int currentCount = Inventory_KJS.instance.CurrentGoodsCount(goodsType);

        if (currentCount > 0)
        {
            Inventory_KJS.instance.SetGoodsCount(goodsType, currentCount - 1);

            // UI 업데이트
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
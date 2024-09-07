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
    public void CreateObject(GameObject prefab, GoodsType goodsType, bool isButtonClick)
    {
        int currentCount = Inventory_KJS.instance.CurrentGoodsCount(goodsType);

        // 수량이 0보다 클 경우에만 생성
        if (currentCount > 0)
        {
            if (prefab != null && playerTransform != null)  // playerTransform이 null이 아닌지 확인
            {
                Vector3 spawnPosition;

                // 최초 스폰일 경우: 플레이어의 정면 2유닛 앞에 스폰하고, 그 오프셋을 기억함
                if (isFirstSpawn)
                {
                    initialSpawnOffset = playerTransform.forward * 2f;  // 최초 스폰 위치를 정면 2유닛 앞에 설정
                    spawnPosition = playerTransform.position + initialSpawnOffset;  // Y축 띄우지 않고 바로 스폰
                    isFirstSpawn = false;  // 최초 스폰 이후로는 false로 설정
                }
                else
                {
                    // 회전만 고려한 새로운 위치 계산: 플레이어의 회전에 따라 초기 오프셋을 회전시킴
                    spawnPosition = playerTransform.position + (playerTransform.rotation * initialSpawnOffset);
                }

                // 프리팹 생성
                GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);

                // 스폰된 프리팹의 크기를 줄임 (50%)
                spawnedPrefab.transform.localScale *= 0.5f;

                // 키 입력과 버튼 클릭에 따라 다른 메서드로 수량 감소 처리
                if (isButtonClick)
                {
                    MinusCount(goodsType);
                    isCountReduced = true;  // 수량 감소 후 플래그 설정
                    Invoke(nameof(ResetCountReduced), 0.1f);  // 일정 시간 후에 플래그 초기화
                }
                else
                {
                    MinusCountByKey(goodsType);  // 숫자 키 입력 시 별도 처리
                }
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
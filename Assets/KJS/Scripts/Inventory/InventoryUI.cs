using UnityEngine;
using TMPro;
using System.Collections.Generic;
using static GoodsInfo;
using Photon.Pun;  // GoodsInfo의 enum GoodsType을 사용하기 위해 static import

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
            // prefabList[prefabIndex]에서 GameObject를 가져오는 대신 해당 프리팹의 이름을 전달합니다.
            string prefabName = prefabList[prefabIndex].name;
            CreateObject(prefabName, goodsType, false);  // false: 버튼 클릭 아님
        }
    }

    // OnClick 이벤트에 따른 프리팹 생성 메서드
    public void OnChestButtonClicked()
    {
        CreateObject("Chest", GoodsType.Chest, true);  // true: 버튼 클릭
    }

    public void OnSwordButtonClicked()
    {
        CreateObject("Sword", GoodsType.Sword, true);  // true: 버튼 클릭
    }

    public void OnRockButtonClicked()
    {
        CreateObject("Rock 1", GoodsType.Rock, true);  // true: 버튼 클릭
    }

    // 프리팹을 생성하는 메서드
    void CreateObject(string prefabName, GoodsType goodsType, bool isButtonClick)
    {
        int currentCount = Inventory_KJS.instance.CurrentGoodsCount(goodsType);
        // 현재 아이템이 남아있는지, prefabName과 spawnPoint가 null이 아닌지 확인
        if (currentCount > 0 && prefabName != null && playerTransform != null)
        {
            UserApi.DeleteItemFromUserInventory(
                UserCache.GetInstance().Id,
                (int)goodsType,
                (res) => { }
                );
            Vector3 spawnPosition;

            // playerTransform을 기준으로 오브젝트 스폰 위치를 계산
            initialSpawnOffset = playerTransform.forward * 2f;
            spawnPosition = playerTransform.position + initialSpawnOffset;

            // 네트워크 상에서 객체 생성
            GameObject spawnedPrefab = PhotonNetwork.Instantiate(prefabName, spawnPosition, Quaternion.identity);

            // PhotonView를 가져와서 로컬 플레이어가 소유한 객체인지 확인
            PhotonView photonView = spawnedPrefab.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                // 로컬 플레이어가 소유한 객체만 스케일 조정
                spawnedPrefab.transform.localScale *= 1f;

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
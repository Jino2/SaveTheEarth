using Photon.Pun;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update

    // [SerializeField] private GameObject[] shopItemPrefabs; //deleted because PhtonNetwork.Instantiate is used

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SoldItem(int itemId)
    {
        var type = (GoodsType)itemId;
        if (type == GoodsType.None)
            return;

        string resourcePath = "";
        switch (type)
        {
            case GoodsType.Chest:
                resourcePath = "Chest";
                break;
            case GoodsType.Sword:
                resourcePath = "Sword";
                break;
            case GoodsType.Rock:
                resourcePath = "Rock";
                break;
            case GoodsType.None:
            default:
                break;
        }

        if (resourcePath == "")
            return;
        var go = PhotonNetwork.Instantiate(resourcePath, transform.position + Vector3.up, transform.rotation);
        print(!go ? "생성 실패...." : "생성!@");
        print(go.transform.position);
        go.TryGetComponent<IDroppable>(out var item);
        item?.Drop();
    }
}
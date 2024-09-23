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

        int price = 0;
        string resourcePath = "";
        switch (type)
        {
            case GoodsType.Chest:
                resourcePath = "Chest";
                price = -100;
                break;
            case GoodsType.Sword:
                resourcePath = "Sword";
                price = -200;
                break;
            case GoodsType.Rock:
                resourcePath = "Rock 1";
                price = -300;
                break;
            case GoodsType.None:
            default:
                break;
        }

        if (resourcePath == "")
            return;
        var go = PhotonNetwork.Instantiate(resourcePath, transform.position + Vector3.up, transform.rotation);
        go.TryGetComponent<IDroppable>(out var item);
        UserApi.AddPoint(UserCache.GetInstance().Id, price, (t) => { });
        item?.Drop();

    }
}
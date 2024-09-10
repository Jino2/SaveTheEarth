using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject[] shopItemPrefabs;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SoldItem(int index)
    {
        index = Mathf.Max(index - 6, 0);
        var go = Instantiate(shopItemPrefabs[index], transform.position, transform.rotation);
        go.TryGetComponent<IDroppable>(out var item);
        item?.Drop();
    }
}
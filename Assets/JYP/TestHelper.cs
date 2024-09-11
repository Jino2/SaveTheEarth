using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHelper : MonoBehaviour
{
    public GameObject dropTestItemPrefab;
    
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DropItemTest();
        }
    }

    public void DropItemTest()
    {
        var go = Instantiate(dropTestItemPrefab, transform.position, transform.rotation);
        go.GetComponent<IDroppable>()?.Drop();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingItemFetcher
{
    public List<SellingItem> GetAllSellingItems()
    {
        
        //TODO: Implement this method
        return new List<SellingItem>()
        {
            new SellingItem("식물 1", 100),
            new SellingItem("식물 2", 200),
            new SellingItem("돌 `1", 300),
            new SellingItem("돌 2", 400),
            new SellingItem("돌 3", 500),
            new SellingItem("돌 4", 600),
            new SellingItem("돌 5", 700),
            new SellingItem("돌 6", 800),
            new SellingItem("돌 7", 900),
            new SellingItem("돌 8", 1000),
            new SellingItem("돌 9", 1100),
            new SellingItem("돌 10", 1200),
            new SellingItem("돌 11", 1300),
            new SellingItem("돌 12", 1400),
            new SellingItem("돌 13", 1500),
        };
    }

}

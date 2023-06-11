using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter_Item : Item
{
    public override void TakeItem()
    {
        UseItem();
    }

    private void UseItem()
    {
        InventoryManager.IM.key++;
        InventoryManager.IM.GetKey();
        Destroy(gameObject);
    }
}

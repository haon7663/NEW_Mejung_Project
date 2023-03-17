using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Item : Item
{
    public override void TakeItem()
    {
        UseItem();
    }

    private void UseItem()
    {
        InventoryManager.IM.key++;
        Destroy(gameObject);
    }
}

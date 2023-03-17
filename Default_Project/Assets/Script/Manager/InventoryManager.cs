using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager IM;

    public int key;

    private void Awake()
    {
        IM = this;
    }
}

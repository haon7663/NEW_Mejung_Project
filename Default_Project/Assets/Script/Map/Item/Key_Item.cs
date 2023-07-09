using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Item : Item
{
    private AudioSource m_AudioSource;
    private bool isCalled = false;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
    public override void TakeItem()
    {
        UseItem();
    }

    private void UseItem()
    {
        if(!isCalled)
        {
            isCalled = true;
            m_AudioSource.Play();
            InventoryManager.IM.key++;
            InventoryManager.IM.GetKey();
            transform.position = new Vector3(1000, 1000);
            Destroy(gameObject, 1f);
        }
    }
}

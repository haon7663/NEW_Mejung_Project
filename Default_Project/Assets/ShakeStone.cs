using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeStone : MonoBehaviour
{
    public GameObject m_WreckEffect;
    private bool isCalled = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isCalled && collision.transform.CompareTag("Platform"))
        {
            isCalled = true;
            CinemachineShake.Instance.ShakeCamera(10, 0.45f);
            Instantiate(m_WreckEffect, new Vector3(transform.position.x, -9), Quaternion.identity);
        }
    }
}

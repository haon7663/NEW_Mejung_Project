using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeStone : MonoBehaviour
{
    private bool isCalled = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isCalled && collision.transform.CompareTag("Platform"))
        {
            isCalled = true;
            CinemachineShake.Instance.ShakeCamera(10, 1f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesCamera : MonoBehaviour
{
    private static DesCamera instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

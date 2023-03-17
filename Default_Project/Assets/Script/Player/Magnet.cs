using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private GameObject mPlayer;

    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player");
    }
}

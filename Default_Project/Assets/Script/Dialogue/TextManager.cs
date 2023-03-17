using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public GameObject mTextBar;
    public int mCount;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeySetting.keys[KeyAction.INTERACTION]))
        {
            GameObject textbar = Instantiate(mTextBar);
            textbar.transform.SetParent(transform);

            textbar.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);
            textbar.GetComponent<Text_Bar>().mCount = ++mCount;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOnOff : MonoBehaviour
{
    [SerializeField] private float mTime_On;
    [SerializeField] private float mTime_Off;
    [SerializeField] private bool isOn;
    [SerializeField] private float Timer;

    private LineRenderer diagLine;
    private PolygonCollider2D PCOL;

    private void Start()
    {
        diagLine = GetComponent<LineRenderer>();
        PCOL = GetComponent<PolygonCollider2D>();
    }
    private void Update()
    {
        Timer += Time.deltaTime;
        if(Timer > (isOn ? mTime_Off : mTime_On))
        {
            isOn = !isOn;
            diagLine.enabled = isOn;
            PCOL.enabled = isOn;
            Timer = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioBox : SceneEvent
{
    private RadioDialogue m_RadioDialogue;
    public string eventName;

    private void Start()
    {
        m_RadioDialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<RadioDialogue>();
    }

    public override void Event()
    {
        StartRadio();
    }
    public void StartRadio()
    {
        m_RadioDialogue.StartDialogue(eventName);
        Destroy(gameObject);
    }
}

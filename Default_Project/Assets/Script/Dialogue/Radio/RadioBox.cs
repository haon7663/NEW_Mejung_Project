using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioBox : MonoBehaviour
{
    private RadioDialogue m_RadioDialogue;

    public string eventName;

    private void Start()
    {
        m_RadioDialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<RadioDialogue>();
    }
    public void StartRadio()
    {
        m_RadioDialogue.StartDialogue(eventName);
    }
}

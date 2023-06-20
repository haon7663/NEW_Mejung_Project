using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public ButtonTutorial m_Key;
    public ButtonTutorial[] m_FrontKeys;

    private void Awake()
    {
        instance = this;
    }
}

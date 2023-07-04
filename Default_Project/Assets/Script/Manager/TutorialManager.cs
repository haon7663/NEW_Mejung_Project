using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public ButtonTutorial[] m_FrontKeys;
    public PlusTutorial m_PlusKey;

    private void Awake()
    {
        instance = this;
    }
}

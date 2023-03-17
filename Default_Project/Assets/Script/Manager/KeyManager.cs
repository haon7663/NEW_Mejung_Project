using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum KeyAction { LEFT, RIGHT, DOWN, UP, JUMP, DASH, STEAM, INTERACTION, KEYCOUNT }

public static class KeySetting { public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>(); }


public class KeyManager : MonoBehaviour
{
    KeyCode[] defaultKeys = new KeyCode[] { KeyCode.A, KeyCode.D, KeyCode.S, KeyCode.W, KeyCode.Space, KeyCode.J, KeyCode.K, KeyCode.E };

    public Text[] mSetText = new Text[8];

    private void Awake()
    {
        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        {
            KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);
            mSetText[i].text = defaultKeys[i].ToString();
        }
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if(keyEvent.isKey)
        {
            KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;
            for (int i = 0; i < mSetText.Length; i++)
            {
                mSetText[i].text = KeySetting.keys[(KeyAction)i].ToString();
            }
            key = -1;
        }
    }
    int key = -1;
    public void ChangeKey(int num)
    {
        Debug.Log(num);
        key = num;
    }
}

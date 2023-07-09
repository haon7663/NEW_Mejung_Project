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
    public int key = -1;
    public bool isKeySetting;

    public static KeyManager instance;

    private void Start()
    {
        instance = this;
    }

    public void SetKey()
    {
        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        {
            KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);
            mSetText[i].text = defaultKeys[i].ToString();
        }
        if (!PlayerPrefs.HasKey("LEFT"))
        {
            SetKeys();
        }
        else
        {
            GetKeys();
        }
    }
    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if(keyEvent.isKey && keyEvent.keyCode.ToString() != "Return" && keyEvent.keyCode.ToString() != "Escape")
        {
            KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;
            for (int i = 0; i < mSetText.Length; i++)
            {
                mSetText[i].text = KeySetting.keys[(KeyAction)i].ToString();
            }
            key = -1;
            SetKeys();
            isKeySetting = false;
        }
    }
    public void ChangeKey(int num)
    {
        Debug.Log(num);
        key = num;
        isKeySetting = true;
    }

    private void SetKeys()
    {
        PlayerPrefs.SetString("LEFT", mSetText[0].text);
        PlayerPrefs.SetString("RIGHT", mSetText[1].text);
        PlayerPrefs.SetString("DOWN", mSetText[2].text);
        PlayerPrefs.SetString("UP", mSetText[3].text);
        PlayerPrefs.SetString("JUMP", mSetText[4].text);
        PlayerPrefs.SetString("DASH", mSetText[5].text);
        PlayerPrefs.SetString("STEAM", mSetText[6].text);
        PlayerPrefs.SetString("INTERACTION", mSetText[7].text);
    }
    private void GetKeys()
    {
        mSetText[0].text = PlayerPrefs.GetString("LEFT").ToString();
        mSetText[1].text = PlayerPrefs.GetString("RIGHT").ToString();
        mSetText[2].text = PlayerPrefs.GetString("DOWN").ToString();
        mSetText[3].text = PlayerPrefs.GetString("UP").ToString();
        mSetText[4].text = PlayerPrefs.GetString("JUMP").ToString();
        mSetText[5].text = PlayerPrefs.GetString("DASH").ToString();
        mSetText[6].text = PlayerPrefs.GetString("STEAM").ToString();
        mSetText[7].text = PlayerPrefs.GetString("INTERACTION").ToString();
        for (int i = 0; i < mSetText.Length; i++)
        {
            KeySetting.keys[(KeyAction)i] = (KeyCode)System.Enum.Parse(typeof(KeyCode), mSetText[i].text);
        }
    }
}

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SaveData
{
    public int savePoint;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    string path;
    public bool isAwake = true;

    public AudioMixer audioMixer;

    void Awake()
    {
        instance = this;
        if (isAwake)
        {
            path = Path.Combine(Application.dataPath + "/Data/", "database.json");
            JsonLoad();
        }
    }
    void Start()
    {
        if (!isAwake)
        {
            path = Path.Combine(Application.dataPath + "/Data/", "database.json");
            JsonLoad();
        }
    }
    public void JsonLoad()
    {
        if(PlayerPrefs.HasKey("savePoint"))
        {
            GameManager.GM.savePoint = PlayerPrefs.GetInt("savePoint");
        }
        else
        {
            GameManager.GM.savePoint = -1;
            JsonSave();
            audioMixer.SetFloat("BGM", -20);
            audioMixer.SetFloat("Master", -15);
            audioMixer.SetFloat("SFX", -15);
            PlayerPrefs.SetFloat("BGM", -20);
            PlayerPrefs.SetFloat("Master", -15);
            PlayerPrefs.SetFloat("SFX", -15);
        }
        /*SaveData saveData = new SaveData();

        if (!File.Exists(path))
        {
            GameManager.GM.savePoint = -1;
            JsonSave();
        }
        else
        {
            var loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(CryptoDebug.Decrypt(loadJson, "we"));
            Debug.Log("decrypt: " + CryptoDebug.Decrypt(loadJson, "we"));

            if (saveData != null)
            {
                GameManager.GM.savePoint = saveData.savePoint;
            }
        }*/
    }

    public void JsonSave()
    {
        PlayerPrefs.SetInt("savePoint", GameManager.GM.savePoint);
        /*SaveData saveData = new SaveData();

        saveData.savePoint = GameManager.GM.savePoint;

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path, CryptoDebug.Encrypt(json, "we"));
        Debug.Log("encrypt: " + json);*/
    }
}

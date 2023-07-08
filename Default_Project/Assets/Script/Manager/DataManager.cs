using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int savePoint;
}

public class DataManager : MonoBehaviour
{
    string path;
    public bool isAwake = true;

    void Awake()
    {
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

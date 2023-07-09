using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private bool isCalled = false;
    
    public void StartGame()
    {
        if (!isCalled)
        {
            isCalled = true;
            Fade.instance.FadeIn(0.5f);
            Invoke(nameof(MoveScene), 0.5f);
        }
    }
    private void MoveScene()
    {
        if (GameManager.GM.savePoint < 0)
        {
            SceneManager.LoadScene("MeetingScene");
        }
        else if (GameManager.GM.savePoint <= 1)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else if (GameManager.GM.savePoint <= 7)
        {
            SceneManager.LoadScene("Map_1");
        }
        else if (GameManager.GM.savePoint <= 12)
        {
            SceneManager.LoadScene("Map_2");
        }
        else if (GameManager.GM.savePoint <= 20)
        {
            SceneManager.LoadScene("Map_3");
        }
    }
}

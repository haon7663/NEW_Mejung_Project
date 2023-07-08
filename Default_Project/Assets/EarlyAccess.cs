using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EarlyAccess : MonoBehaviour
{
    public Image m_EarlyAccessImage;
    public Text m_EarlyAccessText;
    public bool isCalled = false;

    private Move m_Player;

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        isCalled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCalled)
        {
            m_Player.isCutScene = true;
            m_EarlyAccessImage.gameObject.SetActive(true);
            m_EarlyAccessText.gameObject.SetActive(true);

            m_EarlyAccessImage.DOColor(new Color(0, 0, 0, 0.7f), 0.4f);
            m_EarlyAccessText.DOColor(new Color(1, 1, 1, 1f), 0.5f);
            isCalled = true;

            Invoke(nameof(GoMain), 15);
        }
    }

    private void GoMain()
    {
        GameManager.GM.savePoint  = -1;
        GameManager.GM.gameObject.GetComponent<DataManager>().JsonSave();
        GameManager.GM.Main();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dash_Item : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;
    private CircleCollider2D m_CircleCollider2D;
    public float m_RespawnTime;

    public GameObject m_BackGroundLight;
    public GameObject m_PlatformLight;
    private AudioSource m_AudioSource;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_CircleCollider2D = GetComponent<CircleCollider2D>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void TakeDash()
    {
        m_AudioSource.Play();
        if (m_SpriteRenderer.color.a != 0) StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        m_SpriteRenderer.color = new Color(1, 1, 1, 0);
        m_CircleCollider2D.enabled = false;
        m_BackGroundLight.SetActive(false);
        m_PlatformLight.SetActive(false);
        yield return new WaitForSeconds(m_RespawnTime);
        m_SpriteRenderer.DOColor(Color.white, 1).SetEase(Ease.InQuint);
        m_CircleCollider2D.enabled = true;
        m_BackGroundLight.SetActive(true);
        m_PlatformLight.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainManual : MonoBehaviour
{
    public RectTransform m_Explain;
    private Image m_ExplainImage;
    private Text m_ExplainText;

    private Camera m_Camera;

    public Vector3 size;
    public Vector3 offset;
    public LayerMask m_PlayerLayer;

    private void Start()
    {
        m_Camera = Camera.main;
        m_ExplainImage = m_Explain.GetComponentInChildren<Image>();
        m_ExplainText = m_ExplainImage.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        m_Explain.transform.position = m_Camera.WorldToScreenPoint(transform.position);
        var isHit = Physics2D.OverlapBox(transform.position + offset, size, 0, m_PlayerLayer);
        m_ExplainImage.enabled = isHit;
        m_ExplainText.enabled = isHit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, size);
    }
}

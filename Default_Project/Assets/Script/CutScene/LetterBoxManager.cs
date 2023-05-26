using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LetterBoxManager : MonoBehaviour
{
    public RectTransform m_LetterBox_Up;
    public RectTransform m_LetterBox_Down;

    private Image m_LetterBoxImage_Up;
    private Image m_LetterBoxImage_Down;

    private void Start()
    {
        m_LetterBoxImage_Up = m_LetterBox_Up.GetComponent<Image>();
        m_LetterBoxImage_Down = m_LetterBox_Down.GetComponent<Image>();
        m_LetterBoxImage_Up.enabled = true;
        m_LetterBoxImage_Down.enabled = true;
    }

    public void LetterIn()
    {
        m_LetterBox_Up.DOAnchorPosY(-75, 1.5f).SetEase(Ease.OutCirc);
        m_LetterBox_Down.DOAnchorPosY(75, 1.5f).SetEase(Ease.OutCirc);
    }

    public void LetterOut()
    {
        m_LetterBox_Up.DOAnchorPosY(75, 1.5f).SetEase(Ease.OutCirc);
        m_LetterBox_Down.DOAnchorPosY(-75, 1.5f).SetEase(Ease.OutCirc);
    }
}

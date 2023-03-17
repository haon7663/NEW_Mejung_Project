using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Explain : MonoBehaviour
{
    public Interaction mInteraction;
    private Camera MainCamera;

    public Image mImage;
    public Text mText;

    public bool isOneTime = false;

    private void Start()
    {
        MainCamera = Camera.main;
        mInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<Interaction>();
    }

    private void Update()
    {
        //mText.enabled = mInteraction.isExplain;
        if(mText.text != "") transform.position = MainCamera.WorldToScreenPoint(mInteraction.mExplainObject.position + new Vector3(0, mInteraction.ExplainRange));
        if (mInteraction.isExplain)
        {
            if(!isOneTime)
            {
                mImage.DOFade(1, 0.25f).SetEase(Ease.InCirc);
                mText.DOFade(1, 0.25f).SetEase(Ease.InCirc);
                isOneTime = true;
            }
            mText.text = mInteraction.InteractionExplain;
        }
        else if(!mInteraction.isExplain)
        {
            if (isOneTime)
            {
                mImage.DOFade(0, 0.25f).SetEase(Ease.OutCirc);
                mText.DOFade(0, 0.25f).SetEase(Ease.OutCirc);
                isOneTime = false;
            }
        }
    }
}

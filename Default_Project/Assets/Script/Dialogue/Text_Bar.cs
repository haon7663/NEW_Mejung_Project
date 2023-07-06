using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Text_Bar : MonoBehaviour
{
    [TextArea]
    public string mSpeech;

    public string mName;

    public int mCount;

    private GameObject mDialog;

    private Text mText;
    private Text mNameText;
    private Image mImage;
    private RectTransform mRectTransform;
    private Dialogue mTextManager;

    private void Start()
    {
        mDialog = transform.GetChild(0).gameObject;
        mText = mDialog.GetComponent<Text>();
        mNameText = transform.GetChild(1).GetComponent<Text>();
        mImage = GetComponent<Image>();
        mRectTransform = GetComponent<RectTransform>();
        mTextManager = transform.parent.GetComponent<Dialogue>();
        Tweener tweener = mText.DOText(mSpeech, mSpeech.Length * 0.05f).SetEase(Ease.Linear);
        mNameText.text = mName;
        mImage.transform.localScale = new Vector2(mName == "³ë¿¤" ? -1 : 1, 1);
        mText.transform.localScale = new Vector2(mName == "³ë¿¤" ? -1 : 1, 1);
        mNameText.transform.localScale = new Vector2(mName == "³ë¿¤" ? -1 : 1, 1);
    }
    private void Update()
    {
        int distance = mTextManager.mCount - mCount;
        if (distance > 2) Destroy(gameObject, 1.5f);

        float setPos = distance * 360 + 555;
        float setColor = distance == 0 ? 1 : 0.4f;
        mRectTransform.position = Vector2.Lerp(mRectTransform.position, new Vector2(mRectTransform.position.x, setPos), Time.deltaTime * 4);
        mImage.color = Color.Lerp(mImage.color, new Color(setColor, setColor, setColor, 1), Time.deltaTime * 4);
    }
}

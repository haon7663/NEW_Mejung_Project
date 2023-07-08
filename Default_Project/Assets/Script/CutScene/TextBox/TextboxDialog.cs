using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextboxDialog : MonoBehaviour
{
    [SerializeField] TalkData[] talkDatas;

    private Camera m_Camera;
    public GameObject mTextBar;
    private Image mImage;
    private Text mText;
    private Image mTailImage;
    private string saveEventName;
    public bool onText = false;

    public int m_ImageKinds;
    public Sprite[] m_BarImage;
    public Sprite[] m_TailImage;

    private void Start()
    {
        mImage = mTextBar.transform.GetChild(0).GetComponent<Image>();
        mText = mTextBar.GetComponentInChildren<Text>();
        mTailImage = mText.transform.GetChild(0).GetComponent<Image>();
        m_Camera = Camera.main;
    }

    public void StartDialogue(string eventName)
    {
        if (!mTextBar.activeSelf || saveEventName != eventName)
        {
            saveEventName = eventName;
            talkDatas = DialogueParse.DialogueDictionary[eventName];
            StartCoroutine(DebugRadioDialogue(talkDatas));
        }
    }

    internal static class YieldInstructionCache
    {
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
        private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            WaitForSeconds wfs;
            if (!waitForSeconds.TryGetValue(seconds, out wfs))
                waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
            return wfs;
        }
    }
    IEnumerator DebugRadioDialogue(TalkData[] talkdatas)
    {
        mText.text = "";
        for (float j = 0; j < 1f; j += Time.deltaTime)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        for (int i = 0; i < talkdatas.Length; i++)
        {
            foreach (string context in talkdatas[i].contexts)
            {
                while (!onText)
                {
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
                mImage.sprite = m_BarImage[m_ImageKinds];
                mTailImage.sprite = m_TailImage[m_ImageKinds];
                mTextBar.SetActive(true);
                DOTween.Kill(transform);
                mText.text = "";
                mText.DOText(context, context.Length * 0.05f).SetEase(Ease.Linear);

                AbleTextBox[] name = FindObjectsOfType<AbleTextBox>();

                Transform getTransform = null;
                foreach(AbleTextBox takename in name)
                {
                    if (takename.name == talkdatas[i].name)
                    {
                        getTransform = takename.transform;
                        break;
                    }
                }
                Debug.Log(getTransform.name);
                while (onText)
                {
                    mTextBar.transform.position = m_Camera.WorldToScreenPoint(getTransform.position + new Vector3(0, 1.25f));
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
                mTextBar.SetActive(false);
            }
        }
        yield return null;
    }
}

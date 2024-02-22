using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using DG.Tweening;

public class RadioDialogue : MonoBehaviour
{
    [SerializeField] TalkData[] talkDatas;

    private DialogueParse mDialogueParse;

    private AudioSource m_AudioSource;
    private AudioSource m_RadioAudioSource;
    public GameObject mTextBar;
    public Sprite[] m_Portrait;
    private Animator mTextBarAnimator;
    private Image m_RadioPortrait;
    private Text mText;
    private Text mInteractionText;
    private string saveEventName;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_RadioAudioSource = mTextBar.GetComponent<AudioSource>();
        mTextBarAnimator = mTextBar.GetComponent<Animator>();
        mText = mTextBar.transform.GetChild(1).GetComponent<Text>();
        if(mTextBar.transform.GetChild(2).TryGetComponent(out Text text))
            mInteractionText = text;
        m_RadioPortrait = mTextBar.transform.GetChild(0).GetComponent<Image>();

        //talkDatas = DialogueParse.DialogueDictionary["엄"];
    }

    public void StartDialogue(string eventName)
    {
        if(!mTextBar.activeSelf || saveEventName != eventName)
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
        mInteractionText.text = KeySetting.keys[KeyAction.INTERACTION].ToString() + "키를 눌러 넘어가기";
        mText.text = "";
        mTextBar.SetActive(true);
        m_RadioAudioSource.Play();
        mTextBarAnimator.SetBool("isRadio", true);
        GameManager.GM.onRadio = true;
        if (talkdatas[0].name == "노엘") m_RadioPortrait.sprite = m_Portrait[0];
        else if (talkdatas[0].name == "카렌") m_RadioPortrait.sprite = m_Portrait[1];
        else if (talkdatas[0].name == "세이지아") m_RadioPortrait.sprite = m_Portrait[2];
        for (float j = 0; j < 1f; j += Time.deltaTime)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        for (int i = 0; i < talkdatas.Length; i++)
        {
            foreach (string context in talkdatas[i].contexts)
            {
                DOTween.Kill(transform);
                if (talkdatas[i].name == "노엘") m_RadioPortrait.sprite = m_Portrait[0];
                else if (talkdatas[i].name == "카렌") m_RadioPortrait.sprite = m_Portrait[1];
                else if (talkdatas[i].name == "세이지아") m_RadioPortrait.sprite = m_Portrait[2];
                mText.text = "";
                mText.DOText(context, context.Length * 0.05f).SetEase(Ease.Linear);

                for (float j = 0; j < context.Length; j++)
                {
                    m_AudioSource.Play();
                    yield return YieldInstructionCache.WaitForSeconds(0.05f - Time.deltaTime);
                }
                yield return YieldInstructionCache.WaitForSeconds(context.Length * 0.02f);
                while (!Input.GetKeyDown(KeySetting.keys[KeyAction.INTERACTION]))
                {
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
            }
        }
        mTextBarAnimator.SetBool("isRadio", false);
        for (float j = 0; j < 0.5f; j += Time.deltaTime)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        GameManager.GM.onRadio = false;
        mTextBar.SetActive(false);
        yield return null;
    }

    public void OffPortrait(bool onOff)
    {
        m_RadioPortrait.enabled = onOff;
    }
}
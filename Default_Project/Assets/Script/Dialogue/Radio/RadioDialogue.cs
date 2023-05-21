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

    public GameObject mTextBar;
    private Animator mTextBarAnimator;
    private Text mText;
    private string saveEventName;

    private void Start()
    {
        mTextBarAnimator = mTextBar.GetComponent<Animator>();
        mText = mTextBar.GetComponentInChildren<Text>();

        talkDatas = DialogueParse.DialogueDictionary["¾ö"];
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
        mTextBar.SetActive(true);
        mTextBarAnimator.SetBool("isRadio", true);
        for (int i = 0; i < talkdatas.Length; i++)
        {
            foreach (string context in talkdatas[i].contexts)
            {
                for (float j = 0; j < 1; j += Time.deltaTime)
                {
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }

                mText.text = "";
                mText.DOText(context, context.Length * 0.05f).SetEase(Ease.Linear);

                yield return YieldInstructionCache.WaitForSeconds(context.Length * 0.05f);
            }
        }
        mTextBarAnimator.SetBool("isRadio", false);
        for (float j = 0; j < 0.5f; j += Time.deltaTime)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        mTextBar.SetActive(false);
        yield return null;
    }
}
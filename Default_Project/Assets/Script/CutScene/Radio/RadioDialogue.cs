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

        //talkDatas = DialogueParse.DialogueDictionary["��"];
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
        mText.text = "";
        mTextBar.SetActive(true);
        mTextBarAnimator.SetBool("isRadio", true);
        GameManager.GM.onRadio = true;
        for (float j = 0; j < 1f; j += Time.deltaTime)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        for (int i = 0; i < talkdatas.Length; i++)
        {
            foreach (string context in talkdatas[i].contexts)
            {
                DOTween.Kill(transform);
                mText.text = "";
                mText.DOText(context, context.Length * 0.05f).SetEase(Ease.Linear);

                yield return YieldInstructionCache.WaitForSeconds(context.Length * 0.05f);
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using DG.Tweening;


[System.Serializable]
public struct TalkData
{
    public string name;
    public string[] contexts;
}

public class Dialogue : MonoBehaviour
{
    [SerializeField] TalkData[] talkDatas;

    private DialogueParse mDialogueParse;

    public GameObject mTextBar;
    public int mCount;

    public Move mPlayer;

    private Image mBackGround;
    private Image mPlayerStand;

    private float setColor;
    private void Start()
    {
        mBackGround = transform.GetChild(0).gameObject.GetComponent<Image>();
        mPlayerStand = transform.GetChild(1).gameObject.GetComponent<Image>();
    }

    public void CallDialogue(string eventName)
    {
        talkDatas = DialogueParse.DialogueDictionary[eventName];
        StartCoroutine(DebugDialogue(talkDatas));
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
    IEnumerator DebugDialogue(TalkData[] talkdatas)
    {
        SetImage(true);
        mCount = 0;
        mPlayer.isInteraction = true;
        yield return YieldInstructionCache.WaitForSeconds(0.25f);
        for (int i = 0; i < talkdatas.Length; i++)
        {
            foreach (string context in talkdatas[i].contexts)
            {
                while (true)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                        break;
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
                while (true)
                {
                    if (!Input.GetKeyDown(KeyCode.E))
                        break;
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
                GameObject textbar = Instantiate(mTextBar);
                textbar.transform.SetParent(transform);
                if(talkdatas[i].name == "ÁÖÀÎ°ø")
                {
                    mPlayerStand.DOColor(new Color(1, 1, 1, 1), 0.2f);
                    textbar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-90, 0);
                    textbar.transform.localScale = new Vector3(-1, 1, 1);
                    textbar.transform.GetChild(0).transform.localScale = new Vector3(-1, 1, 1);
                    textbar.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(184, 74);
                    textbar.transform.GetChild(1).transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    mPlayerStand.DOColor(new Color(0.4f, 0.4f, 0.4f, 1), 0.2f);
                    setColor = 0.4f;
                    textbar.GetComponent<RectTransform>().anchoredPosition = new Vector2(70, 0);
                }
                textbar.GetComponent<Text_Bar>().mSpeech = context;
                textbar.GetComponent<Text_Bar>().mName = talkdatas[i].name;
                textbar.GetComponent<Text_Bar>().mCount = ++mCount;
            }
        }
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.E))
                break;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        mCount += 3;
        mPlayer.isInteraction = false;
        SetImage(false);
        yield return null;
    }
    private void SetImage(bool Bool)
    {
        if (Bool)
        {
            mBackGround.enabled = true;
            mPlayerStand.enabled = true;
            mBackGround.color = new Color(0, 0, 0, 0);
            mPlayerStand.color = new Color(1, 1, 1, 0);
            mBackGround.DOColor(new Color(0, 0, 0, 0.35f), 0.25f);
            mPlayerStand.DOColor(new Color(1, 1, 1, 1), 0.25f);
        }
        else if(!Bool)
        {
            mBackGround.DOColor(new Color(0, 0, 0, 0), 0.25f);
            mPlayerStand.DOColor(new Color(1, 1, 1, 0), 0.25f);
            Invoke(nameof(InvokeImage), 0.25f);
        }
    }
    private void InvokeImage()
    {
        setColor = 0.4f;
        mBackGround.enabled = false;
        mPlayerStand.enabled = false;
    }
}

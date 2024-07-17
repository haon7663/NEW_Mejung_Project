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
    private Image mNPCStand;
    private Text mInteractionText;
    public Sprite[] m_NPCStand;

    private float setColor;
    private int setDot;
    private float dotTimer;
    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        mBackGround = transform.GetChild(0).gameObject.GetComponent<Image>();
        mPlayerStand = transform.GetChild(1).gameObject.GetComponent<Image>();
        mNPCStand = transform.GetChild(2).gameObject.GetComponent<Image>();
        mInteractionText = transform.GetChild(3).gameObject.GetComponent<Text>();
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
        var saveNpc = -1;
        for (int i = 0; i < talkdatas.Length; i++)
        {
            foreach (string context in talkdatas[i].contexts)
            {
                if (talkdatas[i].name != "노엘" && talkdatas[i].name != "")
                {
                    if (talkdatas[i].name == "세이지아") saveNpc = 0;
                    else if (talkdatas[i].name == "이안") saveNpc = 1;
                    else if (talkdatas[i].name == "데릭") saveNpc = 2;
                    else if (talkdatas[i].name == "카렌") saveNpc = 3;
                    else saveNpc = 4;
                }
                if (saveNpc != -1) break;
            }
        }
        mNPCStand.sprite = m_NPCStand[saveNpc];
        setDot = 0;
        mInteractionText.text = KeySetting.keys[KeyAction.INTERACTION].ToString() + "키를 눌러 넘어가기";
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
                    dotTimer += Time.deltaTime;
                    if(dotTimer > 0.3f)
                    {
                        dotTimer = 0;
                        mInteractionText.text += ".";
                        setDot++;
                        if (setDot >= 4)
                        {
                            setDot = 0;
                            mInteractionText.text = KeySetting.keys[KeyAction.INTERACTION].ToString() + "키를 눌러 넘어가기";
                        }
                    }
                    if (Input.GetKeyDown(KeySetting.keys[KeyAction.INTERACTION]))
                        break;
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
                while (true)
                {
                    dotTimer += Time.deltaTime;
                    if (dotTimer > 0.3f)
                    {
                        dotTimer = 0;
                        mInteractionText.text += ".";
                        setDot++;
                        if (setDot >= 4)
                        {
                            setDot = 0;
                            mInteractionText.text = KeySetting.keys[KeyAction.INTERACTION].ToString() + "키를 눌러 넘어가기";
                        }
                    }
                    if (!Input.GetKeyDown(KeySetting.keys[KeyAction.INTERACTION]))
                        break;
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
                GameObject textbar = Instantiate(mTextBar);
                textbar.transform.SetParent(transform);
                if(talkdatas[i].name == "노엘")
                {
                    mPlayerStand.DOColor(new Color(1, 1, 1, 1), 0.2f);
                    mNPCStand.DOColor(new Color(0.4f, 0.4f, 0.4f, 1), 0.2f);
                    textbar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-90, 0);
                }
                else
                {
                    mPlayerStand.DOColor(new Color(0.4f, 0.4f, 0.4f, 1), 0.2f);
                    mNPCStand.DOColor(new Color(1, 1, 1, 1), 0.2f);
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
            if (Input.GetKeyDown(KeySetting.keys[KeyAction.INTERACTION]))
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
            mNPCStand.enabled = true;
            mInteractionText.enabled = true;
            mBackGround.color = new Color(0, 0, 0, 0);
            mPlayerStand.color = new Color(1, 1, 1, 0);
            mNPCStand.color = new Color(1, 1, 1, 0);
            mBackGround.DOColor(new Color(0, 0, 0, 0.35f), 0.25f);
            mPlayerStand.DOColor(new Color(1, 1, 1, 1), 0.25f);
            mNPCStand.DOColor(new Color(1, 1, 1, 1), 0.25f);
            mInteractionText.DOColor(new Color(1, 1, 1, 1), 0.25f);
        }
        else if(!Bool)
        {
            mBackGround.DOColor(new Color(0, 0, 0, 0), 0.25f);
            mPlayerStand.DOColor(new Color(1, 1, 1, 0), 0.25f);
            mNPCStand.DOColor(new Color(1, 1, 1, 0), 0.25f);
            mInteractionText.DOColor(new Color(1, 1, 1, 0), 0.25f);
            Invoke(nameof(InvokeImage), 0.25f);
        }
    }
    private void InvokeImage()
    {
        setColor = 0.4f;
        mBackGround.enabled = false;
        mPlayerStand.enabled = false;
        mNPCStand.enabled = false;
        mInteractionText.enabled = false;
    }
}

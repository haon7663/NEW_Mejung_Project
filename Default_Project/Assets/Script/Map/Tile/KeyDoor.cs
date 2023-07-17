using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KeyDoor : Interaction_Object
{
    public int mNeedKey;
    public Transform mUp;
    public Transform mDown;

    private Interaction mInteraction;
    private BoxCollider2D mBoxCollider2D;
    private AudioSource m_AudioSource;

    private void Start()
    {
        mInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<Interaction>();
        mBoxCollider2D = GetComponent<BoxCollider2D>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    public override void Interactions()
    {
        if (InventoryManager.IM.key >= mNeedKey)
        {
            m_AudioSource.Play();
            mBoxCollider2D.enabled = false;
            Open();
        }
    }
    public override void Explain()
    {
        if(InventoryManager.IM.key >= mNeedKey)
        {
            mInteraction.ExplainRange = 3f;
            mInteraction.InteractionExplain = KeySetting.keys[KeyAction.INTERACTION].ToString();
        }
        else
        {
            mInteraction.ExplainRange = 3f;
            mInteraction.InteractionExplain = "ø≠ºË ∫Œ¡∑";
        }
    }

    public void Open()
    {
        InventoryManager.IM.OpenDoor();
        mUp.DOLocalMove(new Vector2(0, 3f), 2f).SetEase(Ease.Linear);
        mDown.DOLocalMove(new Vector2(0, -3f), 2f).SetEase(Ease.Linear);
        CinemachineShake.Instance.ShakeCamera(1.75f, 2f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interaction_Object
{
    private Interaction mInteraction;
    private Dialogue mDialogue;
    public string EventName;

    private void Start()
    {
        mInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<Interaction>();
        mDialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Dialogue>();
    }

    public void Dialogue()
    {
        mDialogue.CallDialogue(EventName);
    }
    public override void Interactions()
    {
        Dialogue();
    }
    public override void Explain()
    {
        mInteraction.ExplainRange = 1.7f;
        mInteraction.InteractionExplain = "대화한다.";
    }
}

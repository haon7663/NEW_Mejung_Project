using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public LayerMask InteractionLayer;
    public LayerMask SavePointLayer;
    public LayerMask ItemPointLayer;
    public Collider2D hit;
    public Collider2D savepoint;
    public Collider2D item;

    private bool isDialogue;

    private Move mPlayer;

    public Transform mExplainObject;
    public string InteractionExplain;
    public bool isExplain;
    public float ExplainRange;

    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
    }

    private void Update()
    {
        savepoint = Physics2D.OverlapBox(transform.position, new Vector2(3f, 3f), 0, SavePointLayer);
        hit = Physics2D.OverlapBox(transform.position, new Vector2(2f, 2f), 0, InteractionLayer);
        item = Physics2D.OverlapCircle(transform.position, 1.3f, ItemPointLayer);

        if (savepoint && GameManager.GM.savePoint != savepoint.transform.GetComponent<SavePoint>().PointCount)
        {
            savepoint.transform.GetComponent<SavePoint>().Save();
        }
        if (!GetComponent<Move>().isInteraction && !hit) isDialogue = true;
        if (hit && !mPlayer.isANDash)
        {
            if (Input.GetKey(KeySetting.keys[KeyAction.INTERACTION]) && isDialogue)
            {
                isDialogue = false;
                isExplain = false;
                hit.GetComponent<Interaction_Object>().Interactions();
            }
            else if(isDialogue)
            {
                isExplain = true;
                mExplainObject = hit.gameObject.transform;
                hit.GetComponent<Interaction_Object>().Explain();
            }
            else isExplain = false;
        }
        else isExplain = false;
        if(item)
        {
            item.GetComponent<Item>().TakeItem();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector2(2f, 2f));
    }
}

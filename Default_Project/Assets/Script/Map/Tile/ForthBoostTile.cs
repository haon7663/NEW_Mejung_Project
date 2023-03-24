using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForthBoostTile : MonoBehaviour
{
    public GameObject Right;
    public GameObject Left;
    public SpriteRenderer[] RightTile;
    public SpriteRenderer[] LeftTile;

    public float LoopTime;

    private bool isRight;

    private void Start()
    {
        StartCoroutine(LeftRight());
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

    private IEnumerator LeftRight()
    {
        while(true)
        {
            isRight = !isRight;

            for(int i = 0; i < 4; i++)
            {
                Right.layer = !isRight ? 3 : 9;
                Left.layer = !isRight ? 9 : 3;
                Right.GetComponent<Damage_Tile>().enabled = !isRight;
                Left.GetComponent<Damage_Tile>().enabled = isRight;

                RightTile[i].color = isRight ? Color.white : Color.red;
                LeftTile[i].color = isRight ? Color.red : Color.white;
            }
            yield return YieldInstructionCache.WaitForSeconds(LoopTime);
        }
    }
}

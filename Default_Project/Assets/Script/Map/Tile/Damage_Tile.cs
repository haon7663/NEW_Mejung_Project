using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Tile : MonoBehaviour
{
    public bool isInv;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("HitBox"))
        {
            collision.transform.GetComponentInParent<Move>().Death(isInv);
        }
    }
}

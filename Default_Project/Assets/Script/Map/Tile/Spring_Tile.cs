using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring_Tile : MonoBehaviour
{
    public float Power;
    public Vector2 mAngle;

    private Animator m_Animator;
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        Debug.Log(transform.rotation.eulerAngles.z);
        if (transform.rotation.eulerAngles.z < 45) mAngle = new Vector2(0, 1);
        else if (transform.rotation.eulerAngles.z > 45 && transform.rotation.eulerAngles.z < 135) mAngle = new Vector2(-1.15f, 0.25f);
        else if (transform.rotation.eulerAngles.z > 135 && transform.rotation.eulerAngles.z < 225) mAngle = new Vector2(0, -1);
        else if (transform.rotation.eulerAngles.z > 225 && transform.rotation.eulerAngles.z < 315) mAngle = new Vector2(1.15f, 0.25f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            m_Animator.SetTrigger("Spring");
            StartCoroutine(collision.transform.GetComponent<Move>().Spring(mAngle * Power));
        }
    }
}

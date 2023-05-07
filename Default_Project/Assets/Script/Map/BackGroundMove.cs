using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    private Vector3 startPosition;
    private Camera mainCamera;
    public Transform m_Player;

    public float m_Speed;

    private void Start()
    {
        startPosition = transform.localPosition;
        mainCamera = Camera.main;
    }
    private void FixedUpdate()
    {
        transform.localPosition = startPosition + -mainCamera.transform.position * m_Speed + new Vector3(0, 0, 10);
        transform.localScale = new Vector3(mainCamera.orthographicSize / 10, mainCamera.orthographicSize / 10);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerTelescope : MonoBehaviour
{
    private CinemachineVirtualCamera cinevirtual;
    private CinemachineTransposer mCinemachineTransposer;

    private Rigidbody2D m_Rigidbody2D;

    public Transform m_TargetCamera;
    public Telescope m_Telescope;

    public float m_SetCameraSize;
    public Vector3 m_SetStartPosition;
    public Vector3 m_SetLastPosition;
    public Vector3 m_SetSpeed;

    private float lastPosition;
    private bool isX;

    private void Start()
    {
        cinevirtual = Camera.main.GetComponentInChildren<CinemachineVirtualCamera>();
        mCinemachineTransposer = cinevirtual.GetCinemachineComponent<CinemachineTransposer>();

        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        enabled = false;
    }
    private void OnEnable()
    {
        m_TargetCamera.position = m_SetStartPosition;
        isX = m_SetLastPosition.x > m_SetLastPosition.y;
        lastPosition = isX ? m_SetLastPosition.x : m_SetLastPosition.y;

        StartCoroutine(StartTelescope());
    }
    IEnumerator StartTelescope()
    {
        while(true)
        {
            m_TargetCamera.position += m_SetSpeed * Time.deltaTime;
            cinevirtual.m_Lens.OrthographicSize = m_SetCameraSize;
            m_Rigidbody2D.velocity = Vector2.zero;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        m_Telescope.ChangeActive(false);
    }
}

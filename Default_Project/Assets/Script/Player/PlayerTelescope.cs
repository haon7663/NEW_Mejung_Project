using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerTelescope : MonoBehaviour
{
    private Camera m_MainCamera;
    private CinemachineVirtualCamera cinevirtual;
    private CinemachineTransposer mCinemachineTransposer;
    private GameObject m_TelescopeLight;

    private Rigidbody2D m_Rigidbody2D;

    public Transform m_TargetCamera;
    public Telescope m_Telescope;

    public float m_SetCameraSize;
    public Vector3 m_SetStartPosition;
    public Vector3 m_SetLastPosition;
    public Vector3 m_SetSpeed;

    public float lastPosition;
    public bool isFlip;
    private bool isX;

    private void Start()
    {
        m_MainCamera = Camera.main;
        m_TelescopeLight = m_MainCamera.transform.GetChild(3).gameObject;
        cinevirtual = m_MainCamera.GetComponentInChildren<CinemachineVirtualCamera>();
        mCinemachineTransposer = cinevirtual.GetCinemachineComponent<CinemachineTransposer>();

        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        enabled = false;
    }
    public IEnumerator StartTelescope()
    {
        m_Rigidbody2D.velocity = Vector2.zero;
        yield return YieldInstructionCache.WaitForSeconds(0.31f);
        m_TargetCamera.position = m_SetStartPosition;
        Vector3 distancePos = isFlip ? (m_SetStartPosition - m_SetLastPosition) : (m_SetLastPosition - m_SetStartPosition);
        isX = distancePos.x > distancePos.y;
        lastPosition = isX ? m_SetLastPosition.x : m_SetLastPosition.y;
        cinevirtual.m_Lens.OrthographicSize = m_SetCameraSize;
        m_TelescopeLight.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(0.4f);
        Fade.instance.FadeOut(0.3f);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);

        while (true)
        {
            m_TargetCamera.position += (isFlip ? -m_SetSpeed : m_SetSpeed) * Time.deltaTime;
            cinevirtual.m_Lens.OrthographicSize = m_SetCameraSize;
            m_Rigidbody2D.velocity = Vector2.zero;

            if(Input.GetKeyDown(KeySetting.keys[KeyAction.INTERACTION]))
            {
                break;
            }


            float savePos = isX ? m_MainCamera.transform.position.x : m_MainCamera.transform.position.y;
            Debug.Log("save" + savePos);
            Debug.Log("last" + lastPosition);
            if (!isFlip && savePos > lastPosition)
            {
                for(float i = 0; i < 0.5f; i+=Time.deltaTime)
                {
                    if (Input.GetKeyDown(KeySetting.keys[KeyAction.INTERACTION]))
                    {
                        break;
                    }
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
                break;
            }
            else if (isFlip && savePos < lastPosition)
            {
                for (float i = 0; i < 0.5f; i += Time.deltaTime)
                {
                    if (Input.GetKeyDown(KeySetting.keys[KeyAction.INTERACTION]))
                    {
                        break;
                    }
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
                break;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        Fade.instance.FadeIn(0.3f);
        yield return YieldInstructionCache.WaitForSeconds(0.3f);
        m_Telescope.ChangeActive(false);
        m_TelescopeLight.SetActive(false);
        yield return YieldInstructionCache.WaitForSeconds(0.4f);
        Fade.instance.FadeOut(0.3f);
    }
}

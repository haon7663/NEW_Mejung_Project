using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telescope : Interaction_Object
{
    private GameObject m_Player;
    private Move m_PlayerMove;
    private PlayerTelescope m_Telescope;
    private Interaction mInteraction;
    private AudioSource m_AudioSource;

    private GameObject m_CameraConfiner;

    public float m_SetCameraSize;
    public Vector3 m_SetStartPosition;
    public Vector3 m_SetLastPosition;
    public Vector3 m_SetSpeed;
    public bool isFlip;

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_PlayerMove = m_Player.GetComponent<Move>();
        m_Telescope = m_Player.GetComponent<PlayerTelescope>();
        mInteraction = m_Player.GetComponent<Interaction>();
        m_AudioSource = GetComponent<AudioSource>();

        m_CameraConfiner = GameObject.Find("Camera_Confiner");
    }

    public void ChangeActive(bool isTelescope)
    {
        m_CameraConfiner.SetActive(!isTelescope);
        m_PlayerMove.enabled = !isTelescope;
        m_AudioSource.Play();
        if (isTelescope)
        {
            m_Telescope.m_Telescope = this;
            m_Telescope.m_SetCameraSize = m_SetCameraSize;
            m_Telescope.m_SetStartPosition = m_SetStartPosition;
            m_Telescope.m_SetLastPosition = m_SetLastPosition;
            m_Telescope.m_SetSpeed = m_SetSpeed;
            m_Telescope.isFlip = isFlip;
            StartCoroutine(m_Telescope.StartTelescope());
        }
        m_Telescope.enabled = isTelescope;
    }
    public override void Interactions()
    {
        ChangeActive(true);
        Fade.instance.FadeIn(0.3f);
    }
    public override void Explain()
    {
        mInteraction.ExplainRange = 1.7f;
        mInteraction.InteractionExplain = KeySetting.keys[KeyAction.INTERACTION].ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SavePoint : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D mLight;

    private AudioSource m_AudioSource;
    private Move mPlayerMove;
    public int PointCount;

    private Interaction mInteraction;
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        mPlayerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        mInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<Interaction>();

        if (GameManager.GM.savePoint == PointCount)
        {
            m_AudioSource.Play();
            mPlayerMove.mCheckPoint = transform;
            mPlayerMove.gameObject.transform.position = transform.position;
            DOVirtual.Float(0, 11, 1.25f, LightRadius).SetEase(Ease.OutCirc);
            DOVirtual.Float(1, 3f, 1.25f, LightIntensity).SetEase(Ease.OutCirc);
            Invoke(nameof(IntensityDown), 1.25f);
        }

        if (GameManager.GM.savePoint >= PointCount)
        {
            mLight.pointLightOuterRadius = 11;
        }
    }

    public void Save()
    {
        if(GameManager.GM.savePoint < PointCount)
        {
            m_AudioSource.Play();
            mPlayerMove.mCheckPoint = transform;
            GameManager.GM.savePoint = PointCount;
            GameManager.GM.gameObject.GetComponent<DataManager>().JsonSave();
            DOVirtual.Float(0, 11, 1.25f, LightRadius).SetEase(Ease.OutCirc);
            DOVirtual.Float(1, 3f, 1.25f, LightIntensity).SetEase(Ease.OutCirc);
            Invoke(nameof(IntensityDown), 1.25f);
        }
    }

    private void IntensityDown()
    {
        DOVirtual.Float(3f, 1, 0.8f, LightIntensity).SetEase(Ease.InCirc);
    }
    private void LightRadius(float x)
    {
        mLight.pointLightOuterRadius = x;
    }
    private void LightIntensity(float x)
    {
        mLight.intensity = x;
    }
}

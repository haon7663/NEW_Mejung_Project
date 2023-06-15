using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ElectricEffectManager : MonoBehaviour
{
    public GameObject E_Effect1;
    public GameObject E_Effect2;

    [Serializable]
    public struct ElectricStruct
    {
        public Vector2 EPos;
        public Vector2 ERange;
        public float ETime;
        public float ETimer;
        public bool EReflect;
    }
    List<GameObject>[] Electric_pools;

    public ElectricStruct[] m_ElectricStruct;

    private void Awake()
    {
        Electric_pools = new List<GameObject>[m_ElectricStruct.Length];
        for (int i = 0; i < m_ElectricStruct.Length; i++)
        {
            Electric_pools[i] = new List<GameObject>();
        }
    }

    private void Update()
    {
        for (int i = 0; i < m_ElectricStruct.Length; i++)
        {
            m_ElectricStruct[i].ETimer -= Time.deltaTime;
            if(m_ElectricStruct[i].ETimer < 0)
            {
                Get(i);
                m_ElectricStruct[i].ETimer = m_ElectricStruct[i].ETime;
            }
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        ElectricStruct elec = m_ElectricStruct[index];

        foreach (GameObject item in Electric_pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(Random.Range(0, 2) == 0 ? E_Effect1 : E_Effect2);
            Electric_pools[index].Add(select);
        }

        StartCoroutine(InvokeDisable(select));
        select.transform.SetParent(transform);
        var CompareX = elec.ERange.x > elec.ERange.y;
        select.transform.localScale = new Vector3(Random.Range(0, 2) == 0 ? 1 : -1, 1);
        elec.EPos += new Vector2(elec.EReflect ? -0.6875f : 0.6875f, elec.EReflect ? -0.6875f : 0.6875f);
        select.transform.SetPositionAndRotation(elec.EPos + new Vector2(Random.Range(elec.ERange.x / 2, elec.ERange.x / -2), Random.Range(elec.ERange.y / 2, elec.ERange.y / -2)), Quaternion.Euler(0, 0, CompareX ? elec.EReflect ? 180 : 0 : elec.EReflect ? 90 : 270));

        return select;
    }

    private IEnumerator InvokeDisable(GameObject select)
    {
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        select.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for(int i = 0; i < m_ElectricStruct.Length; i++)
        {
            ElectricStruct elec = m_ElectricStruct[i];
            Gizmos.DrawWireCube(elec.EPos, new Vector2(elec.ERange.x == 0 ? 1 : elec.ERange.x, elec.ERange.y == 0 ? 1 : elec.ERange.y));
        }
    }
}

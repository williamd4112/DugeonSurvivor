using UnityEngine;
using System.Collections;

[System.Serializable]
public enum DamageType
{
    Fire,
    Ice,
    Normal
}

[System.Serializable]
public class Damage
{
    [SerializeField]
    private int m_DamageAmount;
    [SerializeField]
    private DamageType m_Type = DamageType.Normal;
    [SerializeField, Range(0.0f, 1.0f)]
    private float m_CriticalHitRate = 0.65f;
    [SerializeField, Range(0.0f, 1.0f)]
    private float m_CriticalHitBuff = 0.5f;

    public int DamageAmount { get { return m_DamageAmount; } }
    public DamageType Type { get { return m_Type; } }
    public bool CriticalHit { get { return Random.value < m_CriticalHitRate; } }
    public float CriticalHitBuff { get { return m_CriticalHitBuff; } }

}

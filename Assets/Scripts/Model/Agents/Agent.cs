using UnityEngine;
using System.Collections;
using System;

public abstract class Agent : MonoBehaviour
{
    private float m_startTime;
    public float StartTime { get => m_startTime; }
    
    private Guid m_uniqueIdentifier;
    public Guid GetUniqueIdentifier { get => m_uniqueIdentifier; }

    protected virtual void Start()
    {
        m_startTime = Time.time;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildColliderCtrl : MonoBehaviour
{
    public CircleCollider2D m_Collider2D = null;

    public Action<Collider2D> m_OnTriggerEnter2D = null;
    public Action<Collider2D> m_OnTriggerExit2D = null;
    public Action<Collider2D> m_OnTriggerStay2D = null;

    bool m_bInitialized = false;

    public void Init()
    {
        if (m_bInitialized)
            return;

        m_Collider2D = this.GetComponent<CircleCollider2D>();

        m_bInitialized = true;
    }
    private void Start()
    {
        Init();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_OnTriggerEnter2D != null)
            m_OnTriggerEnter2D(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_OnTriggerExit2D != null)
            m_OnTriggerExit2D(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_OnTriggerStay2D != null)
            m_OnTriggerStay2D(collision);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] float m_fInputTime = 0f;
    [SerializeField] float m_fCheckInputTime = 5 * 60f;
    [SerializeField] bool m_bCheckInputTime = true;

    bool m_bInitialized = false;
    public void Init()
    {
        if (m_bInitialized)
            return;

        m_fInputTime = 0f;
        m_fCheckInputTime = 5 * 60f;
        m_bCheckInputTime = true;

        m_bInitialized = true;
    }

    private void Update()
    {
        if (m_bCheckInputTime)
        {
            m_fInputTime += Time.deltaTime * Time.timeScale;

            if (m_fInputTime >= m_fCheckInputTime)
            {
                m_bCheckInputTime = false;
                m_fInputTime = 0;
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            m_fInputTime = 0;
            m_bCheckInputTime = true;
        }
    }
}

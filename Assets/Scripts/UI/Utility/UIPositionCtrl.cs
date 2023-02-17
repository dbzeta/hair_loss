using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPositionCtrl : MonoBehaviour
{
    [SerializeField] RectTransform m_Target = null;

    [SerializeField] Vector3 m_StartPos = Vector3.zero;
    [SerializeField] Vector3 m_EndPos = Vector3.zero;
    [SerializeField] float m_fMoveTimeLength = 0.25f;
    [SerializeField] float m_fCurMoveTime = 0f;

    [SerializeField] bool m_bEnableStart = false;
    [SerializeField] bool m_bLoop = false;

    bool m_bStartMove = false;
    System.Action m_OnInitialized = null;

    bool m_bInitialized = false;

    public void Init()
    {
        if (m_bInitialized)
            return;

        if (m_Target == null) m_Target = this.GetComponent<RectTransform>();
        m_fCurMoveTime = 0f;
        m_bStartMove = false;


        m_bInitialized = true;

        if (m_OnInitialized != null)
            m_OnInitialized();
        m_OnInitialized = null;
    }
    private void Start()
    {
        Init();
    }
    public void StartMove()
    {
        if (!m_bInitialized)
            Init();

        m_fCurMoveTime = 0;
        m_bStartMove = true;
        m_Target.anchoredPosition = m_StartPos;
    }

    private void Update()
    {
        if (m_bInitialized == false)
            return;

        if (m_bStartMove == false)
            return;

        if (m_Target == null)
        {
            Release();
            return;
        }

        m_Target.anchoredPosition = Vector3.Lerp(m_StartPos, m_EndPos, (m_fCurMoveTime / m_fMoveTimeLength));

        float fDeltaTime = Time.deltaTime * Time.timeScale;
        m_fCurMoveTime += fDeltaTime;
        if (m_fCurMoveTime >= m_fMoveTimeLength)
        {
            m_fCurMoveTime = m_fMoveTimeLength;
            if (m_bLoop == false)
            {
                m_bStartMove = false;
                m_Target.anchoredPosition = m_EndPos;
            }
            else
            {
                m_Target.anchoredPosition = m_StartPos;
                m_fCurMoveTime = 0f;
            }
            return;
        }
    }

    public void Release()
    {
        m_Target.anchoredPosition = m_StartPos;
        m_fCurMoveTime = 0f;
        m_bStartMove = false;
    }

    private void OnEnable()
    {
        if (m_bEnableStart == false)
            return;

        if (m_bInitialized == false)
        {
            m_OnInitialized += () =>
            {
                StartMove();
            };

            Init();
        }
        else
            StartMove();
    }

    private void OnDisable()
    {
        Release();
    }
}

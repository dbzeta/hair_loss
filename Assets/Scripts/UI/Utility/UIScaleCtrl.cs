using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaleCtrl : MonoBehaviour
{
    public AnimationCurve m_AnimationCurve = null;

    [SerializeField] RectTransform m_TargetRect = null;
    [SerializeField] float m_fOnceCycleTimeLength = 1.5f;
    [SerializeField] float m_fCurOnceCycleTime = 0.0f;

    [SerializeField] Vector3 m_OriginScale = Vector3.one;

    [SerializeField] bool m_bIsLoop = false;
    bool m_bingScaling = false;

    [SerializeField] bool m_bAutoStart = false;
    [SerializeField] bool m_bAutoEnableStart = false;

    [SerializeField] float m_fFristFrameValue = 0.0f;

    public System.Action OnStartScale = null;
    public System.Action OnComplateScale = null;

    public System.Action OnInitialized = null;

    bool m_bInitailized = false;
    void Start()
    {
        if (!m_bInitailized)
            InitCtrl();
    }
    private void OnEnable()
    {
        if (!m_bInitailized)
        {
            OnInitialized += OnEnableAction;
            return;
        }

        OnEnableAction();
    }

    private void OnEnableAction()
    {
        if (m_TargetRect != null)
            m_TargetRect.localScale = m_OriginScale;

        if (m_bAutoEnableStart)
        {
            m_bingScaling = true;
            m_fCurOnceCycleTime = 0.0f;

            m_TargetRect.localScale = m_OriginScale * m_fFristFrameValue;

            if (OnStartScale != null)
                OnStartScale();
        }
    }

    public void InitCtrl()
    {
        if (m_bInitailized == true)
            return;

        if (m_TargetRect == null)
            m_TargetRect = GetComponent<RectTransform>();

        m_OriginScale = m_TargetRect.localScale;

        if (m_bAutoStart == true)
            m_bingScaling = true;

        if (m_AnimationCurve != null)
            m_fFristFrameValue = m_AnimationCurve.Evaluate(0);

        m_bInitailized = true;

        if (OnInitialized != null)
            OnInitialized();
        OnInitialized = null;
    }

    void Update()
    {
        if (m_bInitailized == false)
            return;

        if (m_bingScaling == false)
            return;

        if (m_TargetRect == null)
            return;

        m_TargetRect.localScale = (m_OriginScale * m_AnimationCurve.Evaluate(m_fCurOnceCycleTime / m_fOnceCycleTimeLength));
        m_fCurOnceCycleTime += Time.deltaTime;


        if (m_fCurOnceCycleTime >= m_fOnceCycleTimeLength)
        {
            m_TargetRect.localScale = (m_OriginScale * m_AnimationCurve.Evaluate(1));
            m_fCurOnceCycleTime = 0.0f;

            if (OnComplateScale != null)
                OnComplateScale();

            if (m_bIsLoop == false)
                m_bingScaling = false;
        }
    }
    public void StartLoopScale()
    {
        if (m_bInitailized == false)
            InitCtrl();

        this.enabled = true;
        ReleaseLoopScale();

        if (m_bAutoEnableStart == false)
        {
            m_bingScaling = true;
            m_fCurOnceCycleTime = 0.0f;

            m_TargetRect.localScale = m_OriginScale * m_fFristFrameValue;

            if (OnStartScale != null)
                OnStartScale();
        }
    }
    public void EndLoopScale()
    {
        if (m_bInitailized == false)
            InitCtrl();

        this.enabled = false;

        ReleaseLoopScale();
    }
    private void ReleaseLoopScale()
    {
        m_TargetRect.localScale = m_OriginScale;
        m_fCurOnceCycleTime = 0.0f;
    }
    public void StartLoopScale(bool _isLoop)
    {
        if (m_bInitailized == false)
            InitCtrl();

        m_bIsLoop = _isLoop;
        ReleaseLoopScale();

        this.enabled = true;
        if (m_TargetRect != null)
            m_TargetRect.localScale = m_OriginScale;

        m_bingScaling = true;
        m_fCurOnceCycleTime = 0.0f;
        m_TargetRect.localScale = m_OriginScale * m_fFristFrameValue;

        if (OnStartScale != null)
            OnStartScale();
    }
    public void ReleaseScaleCallback()
    {
        OnComplateScale = null;
        OnStartScale = null;
    }
    public float OnceCycleTimeLength
    {
        get
        {
            return m_fOnceCycleTimeLength;
        }
        set
        {
            m_fOnceCycleTimeLength = value;
        }
    }
}


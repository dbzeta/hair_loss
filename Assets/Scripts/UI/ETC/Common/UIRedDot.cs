using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRedDot : MonoBehaviour
{
    public AnimationCurve m_AnimationCurve = null;

    [SerializeField] Image m_TargetImage = null;

    [SerializeField] float m_fOnceCycleTimeLength = 1.2f;
    [SerializeField] float m_fCurOnceCycleTime = 0.0f;

    [SerializeField] Vector3 m_OriginScale = Vector3.one;

    bool m_bInitialized = false;

    private void Start()
    {
        Init();
    }

    [ContextMenu("Init")]
    public void Init()
    {
        if (m_bInitialized)
            return;

        if (m_TargetImage == null)
        {
            if (transform.childCount > 0)
                m_TargetImage = transform.GetChild(0).GetComponent<Image>();
            else
                m_TargetImage = this.transform.GetComponent<Image>();
        }

        m_OriginScale = m_TargetImage.transform.localScale;

        m_bInitialized = true;
    }

    public void ResetLoop()
    {
        if (m_TargetImage.transform != null)
            m_TargetImage.transform.localScale = m_OriginScale;

        m_fCurOnceCycleTime = 0f;
    }

    private void OnEnable()
    {
        if (!m_bInitialized)
            Init();

        ReleaseLoopScale();
    }

    private void LateUpdate()
    {
        float percent = (m_fCurOnceCycleTime / m_fOnceCycleTimeLength);

        m_TargetImage.transform.localScale = m_OriginScale * m_AnimationCurve.Evaluate(percent);
        m_fCurOnceCycleTime += Time.deltaTime;

        if (m_fCurOnceCycleTime >= m_fOnceCycleTimeLength)
        {
            m_fCurOnceCycleTime = 0.0f;
        }
    }


    public void StartLoopScale()
    {
        if (m_bInitialized == false)
            Init();

        this.enabled = true;
        ReleaseLoopScale();
    }
    public void EndLoopScale()
    {
        if (m_bInitialized == false)
            Init();

        this.enabled = false;
        ReleaseLoopScale();
    }
    private void ReleaseLoopScale()
    {
        m_TargetImage.transform.localScale = m_OriginScale;
        m_fCurOnceCycleTime = 0.0f;
    }
}

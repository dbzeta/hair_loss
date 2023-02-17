using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject m_targetObj = null;
    [SerializeField] Button m_button;
    // [SerializeField] Toggle m_toggle;

    [SerializeField] Vector3 m_defaultScale = Vector3.one;
    [SerializeField] Vector3 m_buttonScale = new Vector3(0.9f, 0.9f, 0.9f);

    private bool bIsPress = false;

    bool m_bInitialized = false;
    
    [ContextMenu("Init")]
    public void Init()
    {
        if (m_bInitialized == true)
            return;

        if (m_button == null) m_button = this.GetComponent<Button>();
        // if (m_toggle == null) m_toggle = this.GetComponent<Toggle>();

        if (m_targetObj == null)
        {
            if (m_button != null)
                m_targetObj = m_button.targetGraphic.gameObject;
            else
                m_targetObj = this.transform.gameObject;
        }

        m_defaultScale = m_targetObj.transform.localScale;

        m_bInitialized = true;
    }
    private void Start()
    {
        if (!m_bInitialized)
            Init();
    }

    void Update()
    {
        if (m_targetObj == null)
            return;

        if (Input.GetMouseButtonDown(0) && bIsPress)
        {
            if (m_button != null && m_button.interactable)
                m_targetObj.transform.localScale = m_buttonScale;
            // else if (m_toggle != null && m_toggle.interactable)
            //    m_targetObj.transform.localScale = m_buttonScale;
        }
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        // if (m_button != null) ((IPointerUpHandler)m_button).OnPointerUp(eventData);
        // if (m_toggle != null) ((IPointerUpHandler)m_toggle).OnPointerUp(eventData);
        bIsPress = false;
        if (m_targetObj != null)
            m_targetObj.transform.localScale = m_defaultScale;
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        // if (m_button != null) ((IPointerDownHandler)m_button).OnPointerDown(eventData);
        // if (m_toggle != null) ((IPointerDownHandler)m_toggle).OnPointerDown(eventData);
        bIsPress = true;
    }
}

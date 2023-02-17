using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOnOffButtonBase : MonoBehaviour
{
    [SerializeField] Button m_Button = null;
    [SerializeField] Transform m_Content = null;
    [SerializeField] Text m_OffLabel = null;
    [SerializeField] Text m_OnLabel = null;
    [SerializeField] Image m_OffImage = null;
    [SerializeField] Image m_OnImage = null;
    [SerializeField] Image m_IconImage = null;


    bool m_bInitialized = false;

    [ContextMenu("Init")]
    public void Init()
    {
        if (m_bInitialized)
            return;

        if (m_Button == null) m_Button = transform.GetComponent<Button>();
        if (m_Content == null) m_Content = transform.Find("Content");
        {
            if (m_OffLabel == null) m_OffLabel = m_Content.Find("OffLabel").GetComponent<Text>();
            if (m_OnLabel == null) m_OnLabel = m_Content.Find("OnLabel").GetComponent<Text>();
            // m_OnLabel.gameObject.SetActive(false);
            if (m_OffImage == null) m_OffImage = m_Content.Find("OffImage").GetComponent<Image>();
            if (m_OnImage == null) m_OnImage = m_Content.Find("OnImage").GetComponent<Image>();
            // m_OnImage.gameObject.SetActive(false);
            if (m_IconImage == null) m_IconImage = m_Content.Find("IconImage").GetComponent<Image>();
        }
        m_bInitialized = true;
    }
    public Button Button { get { return m_Button; } }
    public void OnClickButton(bool bActive)
    {
        if (m_OnLabel != null)
            m_OnLabel.gameObject.SetActive(bActive);
        if (m_OnImage != null)
            m_OnImage.gameObject.SetActive(bActive);
    }

}

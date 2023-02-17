using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMenuButton : MonoBehaviour
{
    [SerializeField] Button m_Button = null;
    public Button Button { get { return m_Button; } }

    [SerializeField] Image m_OffImage = null;
    [SerializeField] Image m_OnImage = null;
    [SerializeField] Image m_IconImage = null;
    [SerializeField] TMP_Text m_Text = null;
    [SerializeField] UIRedDot m_UIRedDot = null;

    bool m_bInitialized = false;

    [ContextMenu("Init")]
    public void Init()
    {
        if (m_bInitialized)
            return;

        if (m_Button == null) m_Button = transform.GetComponent<Button>();
        if (m_OffImage == null) m_OffImage = transform.Find("OffImage").GetComponent<Image>();
        if (m_OnImage == null) m_OnImage = transform.Find("OnImage").GetComponent<Image>();
        if (m_IconImage == null) m_IconImage = transform.Find("IconImage").GetComponent<Image>();
        if (m_Text == null) m_Text = transform.Find("Text").GetComponent<TMP_Text>();
        if (m_UIRedDot == null) m_UIRedDot = transform.Find("UIRedDot").GetComponent<UIRedDot>();
        SetActiveRedDot(false);

        m_bInitialized = true;
    }

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        if (m_bInitialized == false)
            Init();
    }

    public void SetIconImage(Sprite _sprite)
    {
        if (m_IconImage != null)
        {
            m_IconImage.sprite = _sprite;
        }
    }
    public void SetIconImage(E_Resource_Type resource_Type, string spriteName)
    {
        if (m_IconImage != null)
        {
            Sprite sprite = GameManager.Instance.ResourcesManager.GetSprite(resource_Type, spriteName);
            m_IconImage.sprite = sprite;
        }
    }
    public void SetText(string text)
    {
        if (m_Text != null)
        {
            m_Text.text = text;
        }
    }
    public void SetActiveRedDot(bool _bActive)
    {
        if (m_UIRedDot != null && m_UIRedDot.gameObject.activeSelf != _bActive)
            m_UIRedDot.gameObject.SetActive(_bActive);
    }

    public void OnClickButton(bool _bActive)
    {
        m_OnImage.gameObject.SetActive(_bActive);
    }
}

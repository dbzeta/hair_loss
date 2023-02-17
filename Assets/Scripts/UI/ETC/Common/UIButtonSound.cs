using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private Button m_button;
    private Toggle m_toggle;

    [SerializeField] ESOUND_TYPE m_SoundType = ESOUND_TYPE.E_NONE;
    [SerializeField] string m_ClipName = string.Empty;

    private bool bIsPress = false;

    bool m_bInitialized = false;

    public void Init()
    {
        if (m_bInitialized)
            return;

        if (m_button == null) m_button = this.GetComponent<Button>();
        if (m_toggle == null) m_toggle = this.GetComponent<Toggle>();

        m_bInitialized = true;
    }
    private void Start()
    {
        Setup();
    }
    public void Setup()
    {
        if (!m_bInitialized)
            Init();

        if (m_SoundType != ESOUND_TYPE.E_NONE)
        {
            AudioClip audioClip = GameManager.Instance.Sound.GetAudioClip(m_SoundType);
            m_ClipName = audioClip.name;
        }
        else
            m_ClipName = string.Empty;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        // if (m_button != null) ((IPointerUpHandler)m_button).OnPointerUp(eventData);
        // if (m_toggle != null) ((IPointerUpHandler)m_toggle).OnPointerUp(eventData);
        // bIsPress = false;
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        // if (m_button != null) ((IPointerDownHandler)m_button).OnPointerDown(eventData);
        // if (m_toggle != null) ((IPointerDownHandler)m_toggle).OnPointerDown(eventData);
        // bIsPress = true;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(m_ClipName) || m_SoundType == ESOUND_TYPE.E_NONE)
            return;

        GameManager.Instance.Sound.PlayEffectSound(m_SoundType);
    }
}

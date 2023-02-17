using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EPOPUP_TYPE
{
    NONE_POPUP = -1,
    FULL_SCREEN_POPUP,
    NONE_FULL_SCREEN_POPUP,
    ALWAYS_OPEN_POPUP,
}

public class UIPopupBase : UIBase
{
    public EPOPUP_TYPE m_PopupType = EPOPUP_TYPE.NONE_POPUP;

    protected bool m_bInitialized = false;
    protected bool m_bCantClosePopup = false;

    public System.Action m_OnClose = null;
    public bool CantClosePopup
    {
        get
        {
            return m_bCantClosePopup;
        }
    }
    public void SetCantClosePopup(bool bCant)
    {
        m_bCantClosePopup = bCant;
    }

    [SerializeField] protected bool m_bFixedSiblingIndex = false;
    public bool FixedSiblingIndex
    {
        get
        {
            return m_bFixedSiblingIndex;
        }
    }

    virtual protected void Awake() { }
    virtual protected void Start() { if (!m_bInitialized)  Init(); }
    public override void Init()
    {
        //  popupName = transform.GetType().ToString();
        m_bCantClosePopup = false;
    }
    virtual public void Setup()
    {
        if (!m_bInitialized)
            Init();
    }
    virtual public void Release()
    {
        m_OnClose = null;
    }
    virtual public void ShowPopup()
    {
        // if (EPOPUP_TYPE.SYSTEM_POPUP != m_PopupType)
        // {
        //     // GameManager.Instance.UIManager.
        //     // GameManager.Instance.UIManager.AddCurOpenPopupList(this);
        // 
        //     // if (GameManager.Instance.m_GameType != EGAME_TYPE.GT_ARENA)
        //     // {
        //     //     if (this.m_PopupType != EPOPUP_TYPE.ALWAYS_OPEN_POPUP && this.m_bFixedSiblingIndex == false)
        //     //     {
        //     //         // this.transform.SetParent(GameManager.Instance.SubPopupPanel.m_CurrentOpenOpenPopupTR);
        //     //         this.transform.SetAsLastSibling();
        //     //     }
        //     // }
        // }


        this.gameObject.SetActive(true);

        if (Application.isPlaying)
            GameManager.Instance.UIManager.SetCanvas(gameObject, true);
        // this.Setup();
    }
    virtual public void ClosePopup()
    {
        GameManager.Instance.UIManager.ClosePopupUI(this);

        if (m_OnClose != null)
            m_OnClose();
        m_OnClose = null;
        Release();
        // if (this.gameObject.activeSelf)
        //     this.gameObject.SetActive(false);
    }

    virtual public void OnClickBackButton()
    {
        if (m_bCantClosePopup)
            return;

        ClosePopup();
    }
}

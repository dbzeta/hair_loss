using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIPanelBase : MonoBehaviour
{
    protected bool m_bInitialized = false;
    public bool Initialized { get { return m_bInitialized; } }

    public System.Action m_OnClose = null;

    virtual protected void Awake()
    {
    }
    virtual protected void Start()
    {
        if (!m_bInitialized)
            Init();
    }
    abstract public void Init();
    abstract public void Release();
    virtual public void Setup()
    {
        if (m_bInitialized == false)
            Init();
    }


    virtual public void ShowPanel()
    {
        gameObject.SetActive(true);
        Setup();
    }
    virtual public void ClosePanel()
    {
        Release();
        gameObject.SetActive(false);
    }

    virtual public void Refrash()
    {

    }

    virtual public void OnClickBackButton()
    {
        ClosePanel();
    }

}


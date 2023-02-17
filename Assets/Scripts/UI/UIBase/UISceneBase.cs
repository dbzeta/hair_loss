using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneBase : UIBase
{
    protected bool m_bInitialized = false;
    virtual protected void Awake() { }
    virtual protected void Start() { if (!m_bInitialized) Init(); }
    public override void Init()
    {
        if (Application.isPlaying)
            GameManager.Instance.UIManager.SetCanvas(gameObject, false);
    }
    virtual public void Setup()
    {
        if (!m_bInitialized)
            Init();
    }
    virtual public void Release()
    {

    }
    virtual public void ShowPopup()
    {
        this.gameObject.SetActive(true);
        this.Setup();
    }
}

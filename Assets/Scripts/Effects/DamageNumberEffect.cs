using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageNumberEffect : PoolingObject
{
    public override void Init()
    {
        if (string.IsNullOrEmpty(keyName))
            keyName = (typeof(DamageNumberEffect)).Name;


        if (m_Canvas == null) m_Canvas = this.transform.Find("Canvas").GetComponent<Canvas>();
        {
            if (m_Text == null) m_Text = m_Canvas.transform.Find("Text").GetComponent<TMP_Text>();
            m_vDefaultTextPos = m_Text.transform.position;
        }

        m_fShowTimeLength = 0.5f;
        m_fCurShowTime = 0f;

        m_Camera = null;
    }

    [SerializeField] Camera m_Camera = null;

    [SerializeField] Canvas m_Canvas = null;
    [SerializeField] TMP_Text m_Text = null;

    float m_fShowTimeLength = 1.5f;
    float m_fCurShowTime = 0f;
    Vector3 m_vDefaultTextPos;

    [SerializeField] float m_fMoveSpeed = 2.0f;

    public void ShowDamageEffect(double dDamage, bool isCritical = false)
    {
        if (m_bInitialized == false)
            Init();

        if (GameManager.Instance.GameType == eGameType.eInGame)
            // m_Camera = GameManager.Instance.InGameManager.MainCamera;

        m_Text.text =  UtilsClass.ConvertDoubleToInGameUnit(dDamage);
        m_Text.transform.position = m_vDefaultTextPos;
        m_fCurShowTime = 0f;
        m_fMoveSpeed = 2.0f;

        if (isCritical)
        {
            m_Text.color = Color.red;
            m_Text.transform.localScale = Vector3.one * 1.5f;
        }
        else
        {
            m_Text.color = Color.white;
            m_Text.transform.localScale = Vector3.one;
        }
    }


    private void Update()
    {
        float fDeltaTime = Time.deltaTime * Time.timeScale;
        m_Text.transform.position += Vector3.up * fDeltaTime * m_fMoveSpeed;

        m_fCurShowTime += fDeltaTime;
        if (m_fCurShowTime > m_fShowTimeLength)
        {
            m_fCurShowTime = m_fShowTimeLength;
            Release();
        }
    }

    public void Release()
    {
        m_fCurShowTime = 0;
        m_Text.transform.position = m_vDefaultTextPos;
        GameManager.Instance.ObjectPoolManager.ReturnOnject(this);
    }
}

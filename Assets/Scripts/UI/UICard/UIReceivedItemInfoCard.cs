using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIReceivedItemInfoCard : MonoBehaviour
{
    [SerializeField] Image m_BackImage = null;
    [SerializeField] Image m_IconImage = null;
    [SerializeField] TMP_Text m_Text = null;


    Color m_backDefaultColor;
    Color m_iconDefaultColor;
    Color m_textDefaultColor;

    Color m_backColor;
    Color m_iconColor;
    Color m_textColor;

    float m_fShowTimeLength = 1.0f;
    float m_fCurShowTime = 0f;

    bool m_bIngShow = false;

    bool m_bInitialized = false;

    [ContextMenu("Init")]
    public void Init()
    {
        if (m_bInitialized)
            return;

        if (m_BackImage == null) m_BackImage = transform.GetComponent<Image>();
        if (m_IconImage == null) m_IconImage = transform.Find("IconPanel").GetComponent<Image>();
        if (m_Text == null) m_Text = transform.Find("TextPanel").GetComponent<TMP_Text>();

        m_backDefaultColor = m_backColor = m_BackImage.color;
        m_iconDefaultColor = m_iconColor = Color.white;
        m_textDefaultColor = m_textColor = m_Text.color;

        m_bInitialized = true;
    }

    public void Setup(RewardItemData _rewardItemData)
    {
        if (!m_bInitialized)
            Init();

        if (_rewardItemData == null)
            return;

        Setup(_rewardItemData.ItemData, _rewardItemData.Count);
    }
    public void Setup(ItemDataBase itemDataBase, double dCount)
    {
        if (!m_bInitialized)
            Init();

        if (itemDataBase == null)
            return;

        m_IconImage.sprite = GameManager.Instance.ResourcesManager.GetSprite(E_Resource_Type.E_Item, itemDataBase.icon_name);
        m_Text.text = UtilsClass.ConvertDoubleToInGameUnit(dCount);

        m_BackImage.color = m_backColor = m_backDefaultColor;
        m_IconImage.color = m_iconColor = m_iconDefaultColor;
        m_Text.color = m_textColor = m_textDefaultColor;

        m_fCurShowTime = m_fShowTimeLength;
        m_bIngShow = true;
    }
    private void Update()
    {
        if (m_bIngShow == false)
            return;

        float percent = (m_fCurShowTime / m_fShowTimeLength);

        m_backColor.a = (m_backDefaultColor.a * percent);
        m_iconColor.a = percent;
        m_textColor.a = percent;

        m_BackImage.color = m_backColor;
        m_IconImage.color = m_iconColor;
        m_Text.color = m_textColor;

        float fDeltaTime = Time.deltaTime * Time.timeScale;
        m_fCurShowTime -= fDeltaTime;
        if (m_fCurShowTime <= 0)
        {
            m_fCurShowTime = 0f;
            m_bIngShow = false;
            this.gameObject.SetActive(false);
        }
    }


    public void Release()
    {
        m_fCurShowTime = 0f;
    }
}

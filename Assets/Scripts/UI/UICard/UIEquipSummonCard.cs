using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEquipSummonCard : MonoBehaviour
{
    [SerializeField] Transform m_Content = null;

    [SerializeField] Transform m_TitlePanel = null;
    [SerializeField] TMP_Text m_TitleText = null;

    [SerializeField] Transform m_IconPanel = null;
    [SerializeField] Image m_IconImage = null;

    [SerializeField] Transform m_ButtonPanel = null;
    [SerializeField] Button[] m_SummonButtons = null;

    [SerializeField] Transform m_GaugePanel = null;
    [SerializeField] Image m_GaugeImage = null;
    [SerializeField] TMP_Text m_GaugeText = null;

    SummonStoreData m_SummonStoreData = null;
    SummonStorePerLevelData m_SummonStorePerLevelData = null;
    UserDetailSummonStoreData m_UserDetailSummonStoreData = null;


    bool m_bInitialized = false;

    [ContextMenu("Init")]
    public void Init()
    {
        if (m_bInitialized)
            return;

        if (m_Content == null) m_Content = transform.Find("Content");
        {
            if (m_TitlePanel == null) m_TitlePanel = m_Content.Find("TitlePanel");
            {
                if (m_TitleText == null) m_TitleText = m_TitlePanel.transform.Find("Text").GetComponent<TMP_Text>();
            }

            if (m_IconPanel == null) m_IconPanel = m_Content.Find("InfoPanel/IconPanel");
            {
                if (m_IconImage == null) m_IconImage = m_IconPanel.transform.Find("Image").GetComponent<Image>();
            }
            if (m_ButtonPanel == null) m_ButtonPanel = m_Content.Find("InfoPanel/ButtonPanel");
            {
                if (m_SummonButtons == null || m_SummonButtons.Length != m_ButtonPanel.childCount)
                    m_SummonButtons = new Button[m_ButtonPanel.childCount];

                for (int i = 0; i < m_ButtonPanel.childCount; i++)
                {
                    Transform child = m_ButtonPanel.GetChild(i);
                    if (m_SummonButtons[i] == null || m_SummonButtons[i].gameObject != child.gameObject)
                        m_SummonButtons[i] = child.GetComponent<Button>();
                }
            }

            if (m_GaugePanel == null) m_GaugePanel = m_Content.Find("InfoPanel/GaugePanel");
            {
                if (m_GaugeImage == null) m_GaugeImage = m_GaugePanel.Find("FrontImage").GetComponent<Image>();
                if (m_GaugeText == null) m_GaugeText = m_GaugePanel.Find("Text").GetComponent<TMP_Text>();
            }
        }


        m_bInitialized = true;
    }

    public void Setup(SummonStoreData _summonStoreData)
    {
        if (m_bInitialized)
            Init();

        m_SummonStoreData = _summonStoreData;
        if (m_SummonStoreData == null)
            return;

        m_UserDetailSummonStoreData = GameManager.Instance.UserData.m_UserStoreData.GetUserDetailSummonStoreData(m_SummonStoreData.store_id);

        int level = m_UserDetailSummonStoreData != null ? m_UserDetailSummonStoreData.level : 1;
        int maxLevel = m_SummonStoreData.max_level;
        m_SummonStorePerLevelData = GameManager.Instance.Data.serializableSummonStorePerLevelData.datas.Find(x => x.target_level == level);
        if (m_SummonStorePerLevelData == null)
            return;

        m_IconImage.sprite = GameManager.Instance.ResourcesManager.GetSprite(E_Resource_Type.E_Item, m_SummonStoreData.icon_name);

        if (level >= maxLevel)
        {
            m_TitleText.text = string.Format("{0} Lv.MAX", m_SummonStoreData.textcode_name/*GameManager.Instance.LanguageMgr.GetLocalizeText(m_SummonStoreData.textcode_name)*/);
            m_GaugeImage.fillAmount = 1.0f;
            m_GaugeText.text = "MAX";
        }
        else
            m_TitleText.text = string.Format("{0} Lv.{1}", m_SummonStoreData.textcode_name/*GameManager.Instance.LanguageMgr.GetLocalizeText(m_SummonStoreData.textcode_name)*/, level);

        double curExp = m_UserDetailSummonStoreData != null ? m_UserDetailSummonStoreData.exp : 0;
        double maxExp = m_SummonStorePerLevelData.exp_max;

        m_GaugeImage.fillAmount = (float)(curExp / maxExp);
        m_GaugeText.text = string.Format("{0}/{1}", curExp, maxExp);
    }


    public Button[] SummonButtons { get { return m_SummonButtons; } }
    public SummonStoreData SummonStoreData
    {
        get
        {
            return m_SummonStoreData;
        }
    }
    public SummonStorePerLevelData SummonStorePerLevelData
    {
        get
        {
            return m_SummonStorePerLevelData;
        }
    }
    public UserDetailSummonStoreData UserDetailSummonStoreData
    {
        get
        {
            return m_UserDetailSummonStoreData;
        }
    }

}

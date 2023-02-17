using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEquipItemCard : MonoBehaviour
{
    [SerializeField] Transform m_Content = null;

    [SerializeField] Button m_ItemPanel = null;
    [SerializeField] Image m_IconImage = null;
    [SerializeField] TMP_Text m_RarityText = null;
    [SerializeField] TMP_Text m_LevelText = null;
    [SerializeField] TMP_Text m_RankText = null;
    [SerializeField] Image m_FrameImage = null;

    [SerializeField] Transform m_GaugePanel = null;
    [SerializeField] Image m_GaugeImage = null;
    [SerializeField] TMP_Text m_GaugeText = null;

    [SerializeField] Transform m_LockPanel = null;
    [SerializeField] Transform m_EquipPanel = null;

    EquipItemData m_EquipItemData = null;
    UserDetailEquipItemData m_UserDetailEquipItemData = null;

    bool m_bInitialized = false;

    [ContextMenu("Init")]
    public void Init()
    {
        if (m_bInitialized)
            return;

        if (m_Content == null) m_Content = transform.Find("Content");
        {
            if (m_ItemPanel == null) m_ItemPanel = m_Content.Find("ItemPanel").GetComponent<Button>();
            {
                if (m_IconImage == null) m_IconImage = m_ItemPanel.transform.Find("IconImage").GetComponent<Image>();
                if (m_RarityText == null) m_RarityText = m_ItemPanel.transform.Find("RarityText").GetComponent<TMP_Text>();
                if (m_LevelText == null) m_LevelText = m_ItemPanel.transform.Find("LevelText").GetComponent<TMP_Text>();
                if (m_RankText == null) m_RankText = m_ItemPanel.transform.Find("RankPanel/Text").GetComponent<TMP_Text>();
                if (m_FrameImage == null) m_FrameImage = m_ItemPanel.transform.Find("FrameImage").GetComponent<Image>();
            }
            if (m_LockPanel == null) m_LockPanel = m_Content.Find("LockPanel");
            m_LockPanel.gameObject.SetActive(true);
            if (m_EquipPanel == null) m_EquipPanel = m_Content.Find("EquipPanel");
            m_EquipPanel.gameObject.SetActive(false);

            if (m_GaugePanel == null) m_GaugePanel = m_Content.Find("GaugePanel");
            {
                if (m_GaugeImage == null) m_GaugeImage = m_GaugePanel.Find("FrontImage").GetComponent<Image>();
                if (m_GaugeText == null) m_GaugeText = m_GaugePanel.Find("Text").GetComponent<TMP_Text>();
            }
        }


        m_bInitialized = true;
    }

    public void Setup(EquipItemData _equipItemData)
    {
        if (m_bInitialized)
            Init();

        m_EquipItemData = _equipItemData;
        if (m_EquipItemData == null)
            return;

        m_UserDetailEquipItemData = GameManager.Instance.ItemManager.GetUserDetailItemData(m_EquipItemData.item_id) as UserDetailEquipItemData;

        m_IconImage.sprite = GameManager.Instance.ResourcesManager.GetSprite(E_Resource_Type.E_Item, m_EquipItemData.icon_name);

        m_RarityText.text = GameManager.Instance.Data.GetRarityText(m_EquipItemData.rarity_type);

        int level = m_UserDetailEquipItemData != null ? m_UserDetailEquipItemData.level : 0;
        m_LevelText.text = string.Format("Lv.{0}", level);

        m_RankText.text = GameManager.Instance.Data.GetRankText(m_EquipItemData.item_rank);

        double curCount = m_UserDetailEquipItemData != null ? m_UserDetailEquipItemData.count : 0;
        double maxCount = 4;
        if (curCount >= maxCount) m_GaugeImage.fillAmount = 1.0f;
        else m_GaugeImage.fillAmount = (float)(curCount / maxCount);
        m_GaugeText.text = string.Format("{0}/{1}", curCount, maxCount);

        Color rarityColor = GameManager.Instance.ResourcesManager.GetRarityColor(m_EquipItemData.rarity_type);
        m_RarityText.color = rarityColor;
        m_FrameImage.color = rarityColor;

        bool isEquipped = m_UserDetailEquipItemData != null && m_UserDetailEquipItemData.isEquip == true;
        SetActiveEquipPanel(isEquipped);

        bool isLock = m_UserDetailEquipItemData == null;
        SetActiveLockPanel(isLock);
    }

    public void SetActiveLockPanel(bool bActive)
    {
        if (m_LockPanel != null)
            m_LockPanel.gameObject.SetActive(bActive);
    }
    public void SetActiveEquipPanel(bool bActive)
    {
        if (m_EquipPanel != null)
            m_EquipPanel.gameObject.SetActive(bActive);
    }


    public Button GetButton()
    {
        return m_ItemPanel;
    }

    public EquipItemData EquipItemData { get { return m_EquipItemData; } }
    public UserDetailEquipItemData UserDetailEquipItemData { get { return m_UserDetailEquipItemData; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDailyQuestCard : MonoBehaviour
{
    [SerializeField] Transform m_Content = null;

    [SerializeField] Transform m_IconPanel = null;
    [SerializeField] Image m_RewardImage = null;
    [SerializeField] TMP_Text m_RewardText = null;

    [SerializeField] Transform m_InfoPanel = null;
    [SerializeField] TMP_Text m_TitleText = null;
    [SerializeField] Image m_GaugeImage = null;
    [SerializeField] TMP_Text m_GaugeText = null;

    [SerializeField] Transform m_ButtonPanel = null;
    [SerializeField] Button m_ReceiveButton = null;
    [SerializeField] Transform m_ProceedPanel = null;
    [SerializeField] Transform m_CompletePanel = null;

    DailyQuestData m_DailyQuestData = null;
    UserDetailDailyQuestData m_UserDetailDailyQuestData = null;

    QuestManager m_QuestManager = null;

    bool m_bInitialized = false;

    [ContextMenu("Init")]
    public void Init()
    {
        if (m_bInitialized)
            return;

        if (m_Content == null) m_Content = transform.Find("Content");
        {
            if (m_IconPanel == null) m_IconPanel = m_Content.Find("IconPanel");
            {
                if (m_RewardImage == null) m_RewardImage = m_IconPanel.Find("Image").GetComponent<Image>();
                if (m_RewardText == null) m_RewardText = m_IconPanel.Find("Text").GetComponent<TMP_Text>();
            }
            if (m_InfoPanel == null) m_InfoPanel = m_Content.Find("InfoPanel");
            {
                if (m_TitleText == null) m_TitleText = m_InfoPanel.Find("TitlePanel/Text").GetComponent<TMP_Text>();
                if (m_GaugeImage == null) m_GaugeImage = m_InfoPanel.Find("GaugePanel/FrontImage").GetComponent<Image>();
                if (m_GaugeText == null) m_GaugeText = m_InfoPanel.Find("GaugePanel/Text").GetComponent<TMP_Text>();

                m_TitleText.text = m_GaugeText.text = string.Empty;
            }
            if (m_ButtonPanel == null) m_ButtonPanel = m_Content.Find("ButtonPanel");
            {
                if (m_ReceiveButton == null) m_ReceiveButton = m_ButtonPanel.Find("ReceiveButton").GetComponent<Button>();
                m_ReceiveButton.gameObject.SetActive(false);
                if (m_ProceedPanel == null) m_ProceedPanel = m_ButtonPanel.Find("ProceedPanel");
                m_ProceedPanel.gameObject.SetActive(false);
                if (m_CompletePanel == null) m_CompletePanel = m_ButtonPanel.Find("CompletePanel");
                m_CompletePanel.gameObject.SetActive(false);
            }
        }

        m_bInitialized = true;
    }

    public void Setup(DailyQuestData dailyQuestData)
    {
        if (!m_bInitialized)
            Init();

        if (m_QuestManager == null)
        {
            m_QuestManager = GameManager.Instance.QuestManager;
            if (m_QuestManager.m_AddQuestData != null)
                m_QuestManager.m_AddQuestData -= this.HandleAddQuestData;
            m_QuestManager.m_AddQuestData += this.HandleAddQuestData;
        }

        m_DailyQuestData = dailyQuestData;
        m_UserDetailDailyQuestData = GameManager.Instance.UserData.m_UserQuestData.GetUserDetailQuestData(m_DailyQuestData) as UserDetailDailyQuestData;
        if (m_UserDetailDailyQuestData == null)
            m_UserDetailDailyQuestData = GameManager.Instance.UserData.m_UserQuestData.CreateUserDetailQuestDataAndAddList(m_DailyQuestData) as UserDetailDailyQuestData;

        ItemDataBase rewardItemData = GameManager.Instance.ItemManager.GetItemData(m_DailyQuestData.reward_item_id);
        m_RewardImage.sprite = GameManager.Instance.ResourcesManager.GetSprite(E_Resource_Type.E_Item, rewardItemData.icon_name);
        m_RewardText.text = string.Format("x{0}", m_DailyQuestData.reward_item_count.ToString("0"));

        m_TitleText.text = m_DailyQuestData.textcode_name;

        double curClearCount = m_UserDetailDailyQuestData.questCount;
        double maxClearCount = m_DailyQuestData.quest_count;
        bool bIsCleared = curClearCount >= maxClearCount;
        if (bIsCleared)
        {
            bool bCanReceiveReward = m_UserDetailDailyQuestData.received == false;
            // ????
            if (bCanReceiveReward)
            {
                SetActiveReceiveButton(true);
                SetActiveProceedPanel(false);
                SetActiveCompletePanel(false);
            }
            // ????
            else
            {
                SetActiveReceiveButton(false);
                SetActiveProceedPanel(false);
                SetActiveCompletePanel(true);
            }

            m_GaugeImage.fillAmount = 1.0f;
            m_GaugeText.text = string.Format("{0}/{1}", maxClearCount.ToString("0"), maxClearCount.ToString("0"));
        }
        else
        {
            m_GaugeImage.fillAmount = (float)(curClearCount / maxClearCount);
            m_GaugeText.text = string.Format("{0}/{1}", curClearCount.ToString("0"), maxClearCount.ToString("0"));

            SetActiveReceiveButton(false);
            SetActiveProceedPanel(true);
            SetActiveCompletePanel(false);
        }
    }
    public void SetActiveReceiveButton(bool _bActive)
    {
        if (m_ReceiveButton != null && m_ReceiveButton.gameObject.activeSelf != _bActive)
            m_ReceiveButton.gameObject.SetActive(_bActive);
    }

    public void SetActiveProceedPanel(bool _bActive)
    {
        if (m_ProceedPanel != null && m_ProceedPanel.gameObject.activeSelf != _bActive)
            m_ProceedPanel.gameObject.SetActive(_bActive);
    }
    public void SetActiveCompletePanel(bool _bActive)
    {
        if (m_CompletePanel != null && m_CompletePanel.gameObject.activeSelf != _bActive)
            m_CompletePanel.gameObject.SetActive(_bActive);
    }


    public Button ReceiveButton { get { return m_ReceiveButton; } }
    public DailyQuestData DailyQuestData { get { return m_DailyQuestData; } }
    public UserDetailDailyQuestData UserDetailDailyQuestData { get { return m_UserDetailDailyQuestData; } }


    private void HandleAddQuestData(QuestDataBase questDataBase)
    {
        if (m_DailyQuestData == null)
            return;

        if (m_DailyQuestData.id == questDataBase.id)
        {
            Setup(m_DailyQuestData);
        }
    }

    private void OnEnable()
    {
        if (m_QuestManager != null)
        {
            m_QuestManager = GameManager.Instance.QuestManager;
            if (m_QuestManager.m_AddQuestData != null)
                m_QuestManager.m_AddQuestData -= this.HandleAddQuestData;
            m_QuestManager.m_AddQuestData += this.HandleAddQuestData;
        }
    }
    private void OnDisable()
    {
        if (m_QuestManager != null)
        {
            m_QuestManager = GameManager.Instance.QuestManager;
            if (m_QuestManager.m_AddQuestData != null)
                m_QuestManager.m_AddQuestData -= this.HandleAddQuestData;
        }
    }
}

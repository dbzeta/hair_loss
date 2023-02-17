using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRepeatQuestCard : MonoBehaviour
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

    RepeatQuestData m_RepeatQuestData = null;
    UserDetailQuestDataBase m_UserDetailQuestDataBase = null;

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
            }
        }

        m_bInitialized = true;
    }

    public void Setup(RepeatQuestData repeatQuestData)
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

        m_RepeatQuestData = repeatQuestData;
        m_UserDetailQuestDataBase = GameManager.Instance.UserData.m_UserQuestData.GetUserDetailQuestData(m_RepeatQuestData);
        if (m_UserDetailQuestDataBase == null)
            m_UserDetailQuestDataBase = GameManager.Instance.UserData.m_UserQuestData.CreateUserDetailQuestDataAndAddList(m_RepeatQuestData);

        ItemDataBase rewardItemData = GameManager.Instance.ItemManager.GetItemData(m_RepeatQuestData.reward_item_id);
        m_RewardImage.sprite = GameManager.Instance.ResourcesManager.GetSprite(E_Resource_Type.E_Item, rewardItemData.icon_name);
        m_RewardText.text = string.Format("x{0}", m_RepeatQuestData.reward_item_count.ToString("0"));

        double curClearCount = m_UserDetailQuestDataBase.questCount;
        double maxClearCount = m_RepeatQuestData.quest_count;
        bool bIsCleared = curClearCount >= maxClearCount;
        if (bIsCleared)
        {
            SetActiveReceiveButton(true);
            SetActiveProceedPanel(false);

            m_GaugeImage.fillAmount = 1.0f;
            m_GaugeText.text = string.Format("{0}/{1}", maxClearCount.ToString("0"), maxClearCount.ToString("0"));

            double canReceiveCount = curClearCount / maxClearCount;
            m_TitleText.text = string.Format("{0} X{1}", m_RepeatQuestData.textcode_name, canReceiveCount.ToString("0"));
        }
        else
        {
            m_GaugeImage.fillAmount = (float)(curClearCount / maxClearCount);
            m_GaugeText.text = string.Format("{0}/{1}", curClearCount.ToString("0"), maxClearCount.ToString("0"));

            m_TitleText.text = m_RepeatQuestData.textcode_name;

            SetActiveReceiveButton(false);
            SetActiveProceedPanel(true);
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

    public Button ReceiveButton { get { return m_ReceiveButton; } }
    public RepeatQuestData RepeatQuestData { get { return m_RepeatQuestData; } }
    public UserDetailQuestDataBase UserDetailQuestDataBase { get { return m_UserDetailQuestDataBase; } }


    private void HandleAddQuestData(QuestDataBase questDataBase)
    {
        if (m_RepeatQuestData == null)
            return;

        if (m_RepeatQuestData.id == questDataBase.id)
        {
            Setup(m_RepeatQuestData);
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

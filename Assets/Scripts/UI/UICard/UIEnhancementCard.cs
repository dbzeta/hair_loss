using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEnhancementCard : MonoBehaviour
{
    [SerializeField] Transform m_Content  = null;
    [SerializeField] Transform m_IconPanel = null;
    [SerializeField] Image m_IconImage = null;
    [SerializeField] Transform m_InfoPanel = null;
    [SerializeField] TMP_Text m_TitleText = null;
    [SerializeField] TMP_Text m_InfoText = null;
    [SerializeField] Transform m_ButtonPanel = null;
    [SerializeField] Button m_EnhancementButton = null;
    [SerializeField] Image m_EnhancementButtonIconImage = null;
    [SerializeField] TMP_Text m_EnhancementButtonText = null;
    [SerializeField] Transform m_MaxPanel = null;


    CharacterEnhancementData m_CharacterEnhancementData = null;
    UserDetailEnhancementData m_UserDetailEnhancementData = null;

    CharacterEnhancementPerLevelData m_CurLevelData = null;
    CharacterEnhancementPerLevelData m_NextLevelData = null;

    double m_totalPrice = 0;
    int m_iIncreaseLevel = 1;

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
                if (m_IconImage == null) m_IconImage = m_IconPanel.Find("IconImage").GetComponent<Image>();
            }
            if (m_InfoPanel == null) m_InfoPanel = m_Content.Find("InfoPanel");
            {
                if (m_TitleText == null) m_TitleText = m_InfoPanel.Find("TitleText").GetComponent<TMP_Text>();
                if (m_InfoText == null) m_InfoText = m_InfoPanel.Find("InfoText").GetComponent<TMP_Text>();

                m_TitleText.text = m_InfoText.text = string.Empty;
            }
            if (m_ButtonPanel == null) m_ButtonPanel = m_Content.Find("ButtonPanel");
            {
                if (m_MaxPanel == null) m_MaxPanel = m_ButtonPanel.Find("MaxPanel");
                m_MaxPanel.gameObject.SetActive(false);
                if (m_EnhancementButton == null) m_EnhancementButton = m_ButtonPanel.Find("EnhancementButton").GetComponent<Button>();
                if (m_EnhancementButtonIconImage == null) m_EnhancementButtonIconImage = m_ButtonPanel.Find("EnhancementButton/Image").GetComponent<Image>();
                if (m_EnhancementButtonText == null) m_EnhancementButtonText = m_ButtonPanel.Find("EnhancementButton/Text").GetComponent<TMP_Text>();
                m_EnhancementButtonText.text = string.Empty;
            }
        }

        m_bInitialized = true;
    }

    public void Setup(CharacterEnhancementData characterEnhancementData, int increaseLevel = 1)
    {
        if (!m_bInitialized)
            Init();

        m_totalPrice = 0;
        m_iIncreaseLevel = increaseLevel;
        m_CharacterEnhancementData = characterEnhancementData;
        if (m_CharacterEnhancementData == null)
            return;

        int id = characterEnhancementData.enhancement_id;
        m_UserDetailEnhancementData = GameManager.Instance.UserData.m_UserEnhancementData.GetUserDetailEnhancementData(id);
        int curLevel = m_UserDetailEnhancementData != null ? m_UserDetailEnhancementData.level : 0;

        m_NextLevelData = null;
        m_CurLevelData = GameManager.Instance.Data.GetCharacterEnhancementPerLevelData(id, curLevel); 
        if (m_CurLevelData == null)
        {
            m_IconImage.sprite = GameManager.Instance.ResourcesManager.GetSprite(E_Resource_Type.E_Icon, m_CharacterEnhancementData.icon_name);
            m_TitleText.text = string.Format("{0}<size=22><b><color=#828282>MAX Lv.{1}</color></b></size>", m_CharacterEnhancementData.textcode_name, m_CharacterEnhancementData.max_level);
            m_InfoText.text = string.Empty;
            m_MaxPanel.gameObject.SetActive(false);
            m_EnhancementButton.gameObject.SetActive(false);
            return;
        }

        int maxLevel = m_CharacterEnhancementData.max_level;
        if (curLevel >= maxLevel)
        {
            m_IconImage.sprite = GameManager.Instance.ResourcesManager.GetSprite(E_Resource_Type.E_Icon, m_CharacterEnhancementData.icon_name);
            m_TitleText.text = string.Format("{0}<size=22><b><color=#828282>MAX Lv.{1}</color></b></size>", m_CharacterEnhancementData.textcode_name, m_CharacterEnhancementData.max_level);
            m_InfoText.text = string.Format("{0}", m_CurLevelData.increase_status_value);
            m_MaxPanel.gameObject.SetActive(true);
            m_EnhancementButton.gameObject.SetActive(false);
            return;
        }

        int nextLevel = curLevel + increaseLevel;
        if (nextLevel >= maxLevel) nextLevel = maxLevel;
        m_NextLevelData = GameManager.Instance.Data.GetCharacterEnhancementPerLevelData(id, nextLevel);
        if (m_NextLevelData != null)
        {
            m_IconImage.sprite = GameManager.Instance.ResourcesManager.GetSprite(E_Resource_Type.E_Icon, m_CharacterEnhancementData.icon_name);
            m_TitleText.text = string.Format("{0}<size=22><b><color=#828282>MAX Lv.{1}</color></b></size>", m_CharacterEnhancementData.textcode_name, m_CharacterEnhancementData.max_level);

            if (m_CurLevelData.increase_status_type == EINCREASE_STATUS_TYPE.eIncreaseDamage)
                m_InfoText.text = string.Format("{0} > {1}", m_CurLevelData.increase_status_value, m_NextLevelData.increase_status_value);
            else
                m_InfoText.text = string.Format("{0}% > {1}%", m_CurLevelData.increase_status_value, m_NextLevelData.increase_status_value);

            m_MaxPanel.gameObject.SetActive(false);
            m_EnhancementButton.gameObject.SetActive(true);
            SetupButtonPanel();
        }

    }

    private void SetupButtonPanel()
    {
        m_totalPrice = m_CurLevelData.enhancement_price + m_NextLevelData.enhancement_price;
        for (int i = m_CurLevelData.target_level + 1; i < m_NextLevelData.target_level; i++)
        {
            int id = m_CharacterEnhancementData.enhancement_id;
            int targetLevel = i;
            CharacterEnhancementPerLevelData perLevelData = GameManager.Instance.Data.GetCharacterEnhancementPerLevelData(id, targetLevel);
            if (perLevelData != null) m_totalPrice += perLevelData.enhancement_price;
        }
        m_EnhancementButtonText.text = UtilsClass.ConvertDoubleToInGameUnit(m_totalPrice);
    }


    public Button EnhancementButton { get { return m_EnhancementButton; } }
    public CharacterEnhancementData CharacterEnhancementData { get { return m_CharacterEnhancementData; } }
    public UserDetailEnhancementData UserDetailEnhancementData { get { return m_UserDetailEnhancementData; } }
    public CharacterEnhancementPerLevelData CurLevelData { get { return m_CurLevelData; } }
    public CharacterEnhancementPerLevelData NextLevelData { get { return m_NextLevelData; } }
    public double TotalPrice { get { return m_totalPrice; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITotalItemInfo : MonoBehaviour
{
    [SerializeField] Image iconImage = null;
    [SerializeField] TMP_Text valueText = null;

    UserDetailItemDataBase userDetailItemDataBase = null;
    ItemDataBase itemDataBase = null;

    ItemManager itemManager = null;


    bool m_bInitialized = false;
    public void Init()
    {
        if (m_bInitialized)
            return;

        if (iconImage == null) iconImage = transform.Find("Content/IconImage").GetComponent<Image>();
        if (valueText == null) valueText = transform.Find("Content/ValueText").GetComponent<TMP_Text>();
        valueText.text = string.Empty;

        m_bInitialized = true;
    }

    public void Setup(ItemDataBase _itemDataBase)
    {
        itemDataBase = _itemDataBase;
        if (itemDataBase == null || itemDataBase.item_id <= 0)
            return;

        userDetailItemDataBase = GameManager.Instance.ItemManager.GetUserDetailItemData(itemDataBase.item_id);

        if (itemManager == null)
        {
            itemManager = GameManager.Instance.ItemManager;
            if (itemManager.m_OnAddItem != null)
                itemManager.m_OnAddItem -= this.HandleChangedValue;
            itemManager.m_OnAddItem += this.HandleChangedValue;
        }

        iconImage.sprite = GameManager.Instance.ResourcesManager.GetSprite(E_Resource_Type.E_Item, itemDataBase.icon_name);
        SetupValue();
    }

    public void SetupValue()
    {
        if (userDetailItemDataBase == null)
            valueText.text = "0";
        else
        {
            valueText.text = UtilsClass.ConvertDoubleToInGameUnit(userDetailItemDataBase.count);
        }
    }

    private void HandleChangedValue(ItemDataBase _itemDataBase, double _dCount)
    {
        if (_itemDataBase == null || _itemDataBase.item_id <= 0)
            return;

        if (itemDataBase == null || itemDataBase.item_id != _itemDataBase.item_id)
            return;

        double dCount = System.Math.Truncate(_dCount);
        if (dCount == 0)
            return;

        if (userDetailItemDataBase == null)
            userDetailItemDataBase = GameManager.Instance.ItemManager.GetUserDetailItemData(itemDataBase.item_id);
        SetupValue();
    }

    private void OnEnable()
    {
        if (itemManager != null)
        {
            if (itemManager.m_OnAddItem != null)
                itemManager.m_OnAddItem -= this.HandleChangedValue;
            itemManager.m_OnAddItem += this.HandleChangedValue;
        }
    }

    private void OnDisable()
    {
        if (itemManager != null)
        {
            if (itemManager.m_OnAddItem != null)
                itemManager.m_OnAddItem -= this.HandleChangedValue;
        }
    }
}

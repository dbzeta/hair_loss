using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RewardItemData
{
    public ItemDataBase ItemData;
    public double Count;

    public RewardItemData() { }
    public RewardItemData(ItemDataBase _itemData, double _count) { ItemData = _itemData; Count = _count; }
}

public class ItemManager : MonoBehaviour
{
    UserItemData m_UserItemData = null;

    public Action<ItemDataBase, double> m_OnAddItem = null;
    // public Action<UserDetailStageMapData> m_OnReceiveMiniGameStarReward = null;
    // public Action<UserDetailCostumeData> m_OnReceivedCostumeReward = null;

    List<EquipItemData> m_CanAutoEquipItemList = new List<EquipItemData>();

    bool m_bInitialzed = false;

    public void Init()
    {
        if (m_bInitialzed)
            return;

        if (m_UserItemData == null)
            m_UserItemData = GameManager.Instance.UserData.m_UserItemData;

        m_bInitialzed = true;
    }
    public void Setup()
    {
        if (!m_bInitialzed)
            Init();

        if (m_UserItemData == null)
            m_UserItemData = GameManager.Instance.UserData.m_UserItemData;

        bool bSaveData = false;

        if (bSaveData)
            m_UserItemData.SaveData();
    }

    public void AddItem(int itemId, double count, bool bSaveData, bool bCheckAutoSetup = true)
    {
        ItemDataBase itemData = GetItemData(itemId);
        AddItem(itemData, count, bSaveData);
    }
    public void AddItem(ItemDataBase itemData, double count, bool bSaveData, bool bCheckAutoSetup = true)
    {
        if (itemData == null)
            return;

        GameManager.Instance.UserData.m_UserItemData.AddUserDetailItemData(itemData, count, bSaveData);

        if (itemData.item_type == eItemType.eEquipItem)
        {
            if (bCheckAutoSetup)
            {
                CheckAndRefreshAutoEquip(itemData as EquipItemData);
            }
        }

        if (m_OnAddItem != null)
            m_OnAddItem(itemData, count);
    }
    public void AddItem(params RewardItemData[] rewardItemDatas)
    {
        if (rewardItemDatas == null || rewardItemDatas.Length <= 0)
            return;

        for (int i = 0; i < rewardItemDatas.Length; i++)
        {
            ItemDataBase itemData = rewardItemDatas[i].ItemData;
            double dCount = rewardItemDatas[i].Count;
            if (itemData == null || dCount == 0) continue;

            AddItem(itemData, dCount, false);
        }

        m_UserItemData.SaveData();
    }

    public bool EquipItem(EquipItemData _newEquipItemData)
    {
        if (_newEquipItemData == null || _newEquipItemData.item_id <= 0 || _newEquipItemData.equip_item_type == eEquipItemType.eNone)
            return false;

        UserDetailEquipItemData prevEquippedItemData = GetCurEquippedItemData(_newEquipItemData.equip_item_type);
        EquipItemData prevEquipItemData = GetItemData(prevEquippedItemData.id) as EquipItemData;
        if (prevEquippedItemData == null || prevEquipItemData == null)
            return false;

        UserDetailEquipItemData nextEquippedItemData = GetUserDetailItemData(_newEquipItemData.item_id) as UserDetailEquipItemData;
        if (nextEquippedItemData == null)
            return false;
            
        prevEquippedItemData.isEquip = false;
        nextEquippedItemData.isEquip = true;

        int prevItemTierLevel = ((int)(prevEquipItemData.rarity_type) * 10) + prevEquipItemData.item_rank;
        int newItemTierLevel = ((int)(_newEquipItemData.rarity_type) * 10) + _newEquipItemData.item_rank;

        EquipItemData canAutoEquipItemData = m_CanAutoEquipItemList.Find(x => x.equip_item_type == _newEquipItemData.equip_item_type);

        // 장착한 아이템의 티어가 기존 아이템의 티어보다 낮을 경우
        if (prevItemTierLevel > newItemTierLevel)
        {
            if (canAutoEquipItemData != null)
            {
                // 자동장착 리스트 티어보다 이전 아이템 티어가 높을경우
                int canAutoEquipItemTierLevel = ((int)(_newEquipItemData.rarity_type) * 10) + _newEquipItemData.item_rank;
                if (prevItemTierLevel > canAutoEquipItemTierLevel)
                {
                    m_CanAutoEquipItemList.Remove(canAutoEquipItemData);
                    m_CanAutoEquipItemList.Add(prevEquipItemData);
                }
            }
            else
                m_CanAutoEquipItemList.Add(prevEquipItemData);
        }
        // 장착한 아이템의 티어가 기존 아이템의 티어보다 높을 경우
        else
        {
            if (canAutoEquipItemData != null)
            {
                // 자동장착 리스트 티어보다 새 아이템 티어가 높을경우
                int canAutoEquipItemTierLevel = ((int)(_newEquipItemData.rarity_type) * 10) + _newEquipItemData.item_rank;
                if (newItemTierLevel > canAutoEquipItemTierLevel)
                {
                    m_CanAutoEquipItemList.Remove(canAutoEquipItemData);
                }
            }
        }

        return true;
    }
    public bool AutoEquipItem(eEquipItemType eEquipItemType)
    {
        if (m_CanAutoEquipItemList == null || m_CanAutoEquipItemList.Count <= 0)
            return false;

        EquipItemData canAutoEquipItem = m_CanAutoEquipItemList.Find(x => x.equip_item_type == eEquipItemType);
        if (canAutoEquipItem == null)
            return false;

        return EquipItem(canAutoEquipItem);
    }

    public bool MergeItems(EquipItemData _equipItemData)
    {
        if (_equipItemData == null || _equipItemData.item_id <= 0 || _equipItemData.equip_item_type == eEquipItemType.eNone)
            return false;

        UserDetailEquipItemData prevEquipItemData = GetUserDetailItemData(_equipItemData.item_id) as UserDetailEquipItemData;
        if (prevEquipItemData.count < 4)
            return false;

        int newItemId = _equipItemData.item_id + 1;
        EquipItemData newEquipItemData = GetItemData(newItemId) as EquipItemData;
        if (newEquipItemData == null)
            return false;

        AddItem(_equipItemData, -4, false);
        AddItem(newEquipItemData, 1, true);

        return true;
    }
    public bool AutoMergeItems(eEquipItemType eEquipItemType)
    {
        if (eEquipItemType == eEquipItemType.eNone)
            return false;

        bool isMerge = false;
        List<EquipItemData> equipItemDatas = GameManager.Instance.Data.serializableEquipItemData.datas.FindAll(x => x.equip_item_type == eEquipItemType);
        for (int i = 0; i < equipItemDatas.Count; i++)
        {
            EquipItemData equipItemData = equipItemDatas[i];
            UserDetailEquipItemData prevEquipItemData = GetUserDetailItemData(equipItemData.item_id) as UserDetailEquipItemData;
            if (prevEquipItemData == null || prevEquipItemData.count < 4) continue;

            int newItemId = equipItemData.item_id + 1;
            EquipItemData newEquipItemData = GetItemData(newItemId) as EquipItemData;
            if (newEquipItemData == null) break;

            int newItemCount = (int)(prevEquipItemData.count / 4);

            AddItem(equipItemData, -(newItemCount * 4), false);
            AddItem(newEquipItemData, newItemCount, false);

            isMerge = true;
        }

        if (isMerge)
            m_UserItemData.SaveData();

        return isMerge;
    }
    public void CheckAndRefreshAutoEquip(EquipItemData newEquipItem)
    {
        if (newEquipItem == null)
            return;

        if (m_CanAutoEquipItemList != null && m_CanAutoEquipItemList.Count > 0)
        {
            // 자동장착 리스트보다 새 아이템의 티어가 높을경우
            EquipItemData canAutoEquipItemData = m_CanAutoEquipItemList.Find(x => x.equip_item_type == newEquipItem.equip_item_type);
            if (canAutoEquipItemData != null)
            {
                int canAutoEquipItemTierLevel = ((int)(canAutoEquipItemData.rarity_type) * 10) + canAutoEquipItemData.item_rank;
                int newEquipItemTierLevel = ((int)(newEquipItem.rarity_type) * 10) + newEquipItem.item_rank;
                if (canAutoEquipItemTierLevel < newEquipItemTierLevel)
                {
                    m_CanAutoEquipItemList.Remove(canAutoEquipItemData);
                    m_CanAutoEquipItemList.Add(newEquipItem);
                }
            }
            // 기존 장착중인 아이템보다 새 아이템의 티어가 높을경우
            else
            {
                UserDetailEquipItemData curEquippedItemData = GetCurEquippedItemData(newEquipItem.equip_item_type);
                if (curEquippedItemData != null)
                {
                    EquipItemData prevEquipItemData = GetItemData(curEquippedItemData.id) as EquipItemData;
                    if (prevEquipItemData != null)
                    {
                        int prevEquipItemTierLevel = ((int)(prevEquipItemData.rarity_type) * 10) + prevEquipItemData.item_rank;
                        int newEquipItemTierLevel = ((int)(newEquipItem.rarity_type) * 10) + newEquipItem.item_rank;
                        if (prevEquipItemTierLevel < newEquipItemTierLevel)
                        {
                            m_CanAutoEquipItemList.Add(newEquipItem);
                        }
                    }
                }
            }
        }
        else
        {
            // 기존 장착중인 아이템보다 새 아이템의 티어가 높을경우
            UserDetailEquipItemData curEquippedItemData = GetCurEquippedItemData(newEquipItem.equip_item_type);
            if (curEquippedItemData != null)
            {
                EquipItemData prevEquipItemData = GetItemData(curEquippedItemData.id) as EquipItemData;
                if (prevEquipItemData != null)
                {
                    int prevEquipItemTierLevel = ((int)(prevEquipItemData.rarity_type) * 10) + prevEquipItemData.item_rank;
                    int newEquipItemTierLevel = ((int)(newEquipItem.rarity_type) * 10) + newEquipItem.item_rank;
                    if (prevEquipItemTierLevel < newEquipItemTierLevel)
                    {
                        m_CanAutoEquipItemList.Add(newEquipItem);
                    }
                }
            }
        }
    }

    public double GetMonsterDropGold(int _stage, int _killCount = 1)
    {
        double dDropGold = 0;

        // 공격력 증가율 : Pow(stageLevel,2)
        // 타겟 레벨 : (공격력 증가율 - 10 / 10)
        // = 0.25(타겟 레벨 * 타겟 레벨) + 0.00005(타겟 레벨) - 0.128

        float damage_rate = Mathf.Pow(_stage, 2);
        int target_level = (int)((damage_rate) * 0.1);
        target_level = Mathf.Clamp(target_level, 1, 10000000);
         dDropGold = 0.25 * (target_level * target_level) + 0.00005 * target_level - 0.128;
        if (dDropGold < 5) dDropGold = 5;
        return dDropGold;
    }


    public ItemDataBase GetItemData(int itemId)
    {
        return GameManager.Instance.Data.GetItemData(itemId);
    }
    public UserDetailItemDataBase GetUserDetailItemData(int itemId)
    {
        return GameManager.Instance.UserData.m_UserItemData.GetUserDetailItemData(itemId);
    }
    public List<UserDetailEquipItemData> GetCurEquippedItemDatas()
    {
        return GameManager.Instance.UserData.m_UserItemData.equipItemDataList.FindAll(x => x.isEquip == true);
    }
    public UserDetailEquipItemData GetCurEquippedItemData(eEquipItemType _eEquipItemType)
    {
        List<UserDetailEquipItemData> curEquippedItemList = GetCurEquippedItemDatas();
        return curEquippedItemList.Find(x => x.equipItemType == _eEquipItemType);
    }
    public EquipItemData[] GetAutoEquipItemDatas()
    {
        return m_CanAutoEquipItemList.ToArray();
    }

    public void Release()
    {
        m_UserItemData = null;
        m_OnAddItem = null;
        // m_OnReceiveMiniGameStarReward = null;
        // m_OnReceivedCostumeReward = null;

        if (m_CanAutoEquipItemList != null) m_CanAutoEquipItemList.Clear();

        m_bInitialzed = false;
    }

    private void Update()
    {
        if (m_bInitialzed == false)
            return;
    }
}

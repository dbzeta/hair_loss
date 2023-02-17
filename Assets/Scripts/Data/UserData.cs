using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;


[Serializable]
public class UserDataBase
{
    public string tableName = string.Empty;
    public string inDate = string.Empty;
    public bool isLoadComplete = false;
    virtual public void Init()
    {
        tableName = GetType().Name;
    }
    virtual public bool Load(JsonData loadData)
    {
        return false;
    }
    virtual public void SaveData()
    {

    }
}

[Serializable]
public class UserInfoData : UserDataBase
{
    public string nickname = string.Empty;
    public int level = 1;
    public double exp = 0;

    public long lastAccessTimestamp = 0L;
    public override bool Load(JsonData loadData)
    {
        try
        {
            if (loadData.ContainsKey("nickname")) nickname = (loadData["nickname"]).ToString();
            if (loadData.ContainsKey("level")) level = int.Parse((loadData["level"]).ToString());
            if (loadData.ContainsKey("exp")) exp = double.Parse((loadData["exp"]).ToString());
            if (loadData.ContainsKey("lastAccessTimestamp")) lastAccessTimestamp = long.Parse((loadData["lastAccessTimestamp"]).ToString());

            isLoadComplete = true;
        }
        catch (Exception e)
        {
            isLoadComplete = false;
            Debug.LogError(e);
        }

        return isLoadComplete;
    }
}

[Serializable]
public class UserMapData : UserDataBase
{
    public int stage = 1;
    public int maxStage = 1;

    public bool isBossMode = false;

    public Action OnChangeStage = null;
    public Action<bool> OnActiveBossMode = null;

    public override void Init()
    {
        base.Init();

        stage = 1;
        maxStage = 1;

        OnChangeStage = null;
        OnActiveBossMode = null;
    }
    public override bool Load(JsonData loadData)
    {
        try
        {
            if (loadData.ContainsKey("stage")) stage = int.Parse((loadData["stage"]).ToString());
            if (loadData.ContainsKey("maxStage")) stage = int.Parse((loadData["maxStage"]).ToString());

            isLoadComplete = true;
        }
        catch (Exception e)
        {
            isLoadComplete = false;
            Debug.LogError(e);
        }
        return isLoadComplete;
    }

    public void AddStage()
    {
        int curStage = stage;
        int nextStage = curStage + 1;

        if (nextStage > maxStage)
        { 
            maxStage = nextStage;
        }

        ChangeStage(nextStage);
    }
    public void ChangeStage(int iStageLevel)
    {
        stage = iStageLevel;

        if (OnChangeStage != null)
            OnChangeStage();
    }
    public void SetActiveBossMode(bool bActive)
    {
        isBossMode = bActive;

        if (OnActiveBossMode != null)
            OnActiveBossMode(bActive);
    }
}

[Serializable]
public class UserDetailItemDataBase
{
    public int id;
    public double count;
}
[Serializable]
public class UserDetailEquipItemData : UserDetailItemDataBase
{
    public int level;
    public bool isEquip = false;
    public eEquipItemType equipItemType;
}
[Serializable]
public class UserDetailRelicItemData : UserDetailItemDataBase
{
    public int level;
}
[Serializable]
public class UserDetailCostumeItemData : UserDetailItemDataBase
{
    public int level;
}

[Serializable]
public class UserItemData : UserDataBase
{
    public List<UserDetailItemDataBase> goodsItemDataList = new List<UserDetailItemDataBase>();
    public List<UserDetailEquipItemData> equipItemDataList = new List<UserDetailEquipItemData>();
    public List<UserDetailRelicItemData> relicItemDataList = new List<UserDetailRelicItemData>();
    public override void Init()
    {
        base.Init();
    }
    public override bool Load(JsonData loadData)
    {
        try
        {
            if (loadData.ContainsKey("goodsItemDataList"))
            {
                goodsItemDataList = new List<UserDetailItemDataBase>();
                JsonData jGoodsItemDataList = loadData["goodsItemDataList"];
                for (int i = 0; i < jGoodsItemDataList.Count; i++)
                {
                    JsonData jGoodsItemData = jGoodsItemDataList[i];

                    int id = jGoodsItemData.ContainsKey("id") ? int.Parse(jGoodsItemData["id"].ToString()) : 0;
                    double count = jGoodsItemData.ContainsKey("count") ? double.Parse(jGoodsItemData["count"].ToString()) : 0;
                    if (id <= 0) continue;

                    UserDetailItemDataBase _data = goodsItemDataList.Find(x => x.id == id);
                    if (_data == null)
                    {
                        _data = new UserDetailItemDataBase();
                        _data.id = id;
                        _data.count = count;
                        goodsItemDataList.Add(_data);
                    }
                }
            }

            if (loadData.ContainsKey("equipItemDataList"))
            {
                equipItemDataList = new List<UserDetailEquipItemData>();
                JsonData jEquipItemDataList = loadData["equipItemDataList"];
                for (int i = 0; i < jEquipItemDataList.Count; i++)
                {
                    JsonData jEquipItemData = jEquipItemDataList[i];

                    int id = jEquipItemData.ContainsKey("id") ? int.Parse(jEquipItemData["id"].ToString()) : 0;
                    double count = jEquipItemData.ContainsKey("count") ? double.Parse(jEquipItemData["count"].ToString()) : 0;
                    int level = jEquipItemData.ContainsKey("level") ? int.Parse(jEquipItemData["level"].ToString()) : 0;
                    bool isEquip = jEquipItemData.ContainsKey("isEquip") ? bool.Parse(jEquipItemData["isEquip"].ToString()) : false;
                    eEquipItemType equipItemType = eEquipItemType.eWeapon;
                    if (jEquipItemData.ContainsKey("equipItemType"))
                        equipItemType = (eEquipItemType)int.Parse(jEquipItemData["equipItemType"].ToString());


                    if (id <= 0 || level <= 0) continue;

                    UserDetailEquipItemData _data = equipItemDataList.Find(x => x.id == id);
                    if (_data == null)
                    {
                        _data = new UserDetailEquipItemData();
                        _data.id = id;
                        _data.level = level;
                        _data.count = count;
                        _data.isEquip = isEquip;
                        _data.equipItemType = equipItemType;
                        equipItemDataList.Add(_data);
                    }
                    else
                    {
                        if (_data.level < level)
                            _data.level = level;
                    }
                }
            }

            if (loadData.ContainsKey("relicItemDataList"))
            {
                relicItemDataList = new List<UserDetailRelicItemData>();
                JsonData jRelicItemDataList = loadData["relicItemDataList"];
                for (int i = 0; i < jRelicItemDataList.Count; i++)
                {
                    JsonData jRelicItemData = jRelicItemDataList[i];

                    int id = jRelicItemData.ContainsKey("id") ? int.Parse(jRelicItemData["id"].ToString()) : 0;
                    double count = jRelicItemData.ContainsKey("count") ? double.Parse(jRelicItemData["count"].ToString()) : 0;
                    int level = jRelicItemData.ContainsKey("level") ? int.Parse(jRelicItemData["level"].ToString()) : 0;
                    if (id <= 0 || level <= 0) continue;

                    UserDetailRelicItemData _data = relicItemDataList.Find(x => x.id == id);
                    if (_data == null)
                    {
                        _data = new UserDetailRelicItemData();
                        _data.id = id;
                        _data.level = level;
                        _data.count = count;
                        relicItemDataList.Add(_data);
                    }
                    else
                    {
                        if (_data.level < level)
                            _data.level = level;
                    }
                }
            }

            isLoadComplete = true;
        }
        catch (Exception e)
        {
            isLoadComplete = false;
            Debug.LogError(e);
        }

        return isLoadComplete;
    }

    public override void SaveData()
    {
        GameManager.Instance.UserData.SaveUserData(this);
    }

    public void AddUserDetailItemData(ItemDataBase _itemData, double _count, bool _bSaveData)
    {
        if (_itemData == null || _itemData.item_id <= 0 || _itemData.item_type == eItemType.eNone)
            return;

        _count = Math.Truncate(_count);

        UserDetailItemDataBase _prevData = GetUserDetailItemData(_itemData.item_id);
        if (_prevData == null)
        {
            CreateUserDetailItemData(_itemData, _count, _bSaveData);
        }
        else
        {
            _prevData.count += _count;
        }
        
        if (_bSaveData)
            SaveData();

    }
    public void CreateUserDetailItemData(ItemDataBase _itemData, double _count, bool _bSaveData)
    {
        if (_itemData.item_type == eItemType.eGoodsItem)
        {
            UserDetailItemDataBase _newData = new UserDetailItemDataBase();
            _newData.id = _itemData.item_id;
            _newData.count = _count;
            goodsItemDataList.Add(_newData);
        }
        else if (_itemData.item_type == eItemType.eEquipItem)
        {
            EquipItemData equipItemData = _itemData as EquipItemData;
            UserDetailEquipItemData _newData = new UserDetailEquipItemData();
            _newData.id = _itemData.item_id;
            _newData.count = _count;
            _newData.level = equipItemData.start_level;
            _newData.equipItemType = equipItemData.equip_item_type;
            _newData.isEquip = false;

            equipItemDataList.Add(_newData);
        }
        else if (_itemData.item_type == eItemType.eRelicItem)
        {
            RelicItemData relicItemData = _itemData as RelicItemData;
            UserDetailRelicItemData _newData = new UserDetailRelicItemData();
            _newData.id = _itemData.item_id;
            _newData.count = _count;
            _newData.level = relicItemData.start_level;
            relicItemDataList.Add(_newData);
        }

        if (_bSaveData)
            SaveData();
    }

    public UserDetailItemDataBase GetUserDetailItemData(ItemDataBase _itemDataBase)
    {
        if (_itemDataBase == null)
            return null;

        return GetUserDetailItemData(_itemDataBase.item_id);
    }
    public UserDetailItemDataBase GetUserDetailItemData(int _itemID)
    {
        if (_itemID <= 0)
            return null;

        if (_itemID >= (int)E_DATA_ID.eItem_Start && _itemID < (int)E_DATA_ID.eEquipItem_Staff)
            return goodsItemDataList.Find(x => x.id == _itemID);
        else if (_itemID >= (int)E_DATA_ID.eEquipItem_Staff && _itemID < (int)E_DATA_ID.eRelicItem_Start)
            return equipItemDataList.Find(x => x.id == _itemID);
        else if (_itemID >= (int)E_DATA_ID.eRelicItem_Start && _itemID < (int)E_DATA_ID.eItem_End)
            return relicItemDataList.Find(x => x.id == _itemID);

        return null;
    }
}

[Serializable]
public class UserDetailEnhancementData
{
    public int id;
    public int level;
}
[Serializable]
public class UserEnhancementData : UserDataBase
{
    public List<UserDetailEnhancementData> enhancementDataList = new List<UserDetailEnhancementData>();

    public override bool Load(JsonData loadData)
    {
        try
        {
            if (loadData.ContainsKey("enhancementDataList"))
            {
                if (enhancementDataList == null) enhancementDataList = new List<UserDetailEnhancementData>();
                JsonData jEnhancementDataList = loadData["enhancementDataList"];
                for (int i = 0; i < jEnhancementDataList.Count; i++)
                {
                    JsonData jEnhancementData = jEnhancementDataList[i];
                    UserDetailEnhancementData newData = new UserDetailEnhancementData();

                    int id = (jEnhancementData.ContainsKey("id")) ? int.Parse(jEnhancementData["id"].ToString()) : 0;
                    int level = (jEnhancementData.ContainsKey("level")) ? int.Parse(jEnhancementData["level"].ToString()) : 0;
                    if (id <= 0) continue;
                    newData.id = id;
                    newData.level = level;

                    UserDetailEnhancementData prevData = enhancementDataList.Find(x => x.id == id);
                    if (prevData != null) continue;

                    enhancementDataList.Add(newData);
                }
            }

            isLoadComplete = true;
        }
        catch (Exception e)
        {
            isLoadComplete = false;
            Debug.LogError(e);
        }
        return isLoadComplete;
    }

    public void EnhancementData(CharacterEnhancementData characterEnhancementData, int increaseLevel, bool bSaveData = true)
    {
        if (characterEnhancementData == null)
            return;

        UserDetailEnhancementData userDetailEnhancementData = GetUserDetailEnhancementData(characterEnhancementData.enhancement_id);
        if (userDetailEnhancementData == null)
        {
            userDetailEnhancementData = new UserDetailEnhancementData();
            userDetailEnhancementData.id = characterEnhancementData.enhancement_id;
            userDetailEnhancementData.level = characterEnhancementData.start_level;
            enhancementDataList.Add(userDetailEnhancementData);
        }

        userDetailEnhancementData.level += increaseLevel;
        if (bSaveData) SaveData();
    }
    public UserDetailEnhancementData GetUserDetailEnhancementData(int _id)
    {
        return enhancementDataList.Find(x => x.id == _id);
    }
}
[Serializable]
public class UserDetailQuestDataBase
{
    public int id;
    public double questCount;
}
[Serializable]
public class UserDetailDailyQuestData : UserDetailQuestDataBase
{
    public bool received;
}
[Serializable]
public class UserQuestData : UserDataBase
{
    public List<UserDetailDailyQuestData> dailyQuestDataList = new List<UserDetailDailyQuestData>();
    public List<UserDetailQuestDataBase> repeatQuestDataList = new List<UserDetailQuestDataBase>();

    public long resetTimestamp = 0L;

    public override void Init()
    {
        base.Init();

        dailyQuestDataList = new List<UserDetailDailyQuestData>();
        repeatQuestDataList = new List<UserDetailQuestDataBase>();
        resetTimestamp = 0L;
    }
    public override bool Load(JsonData loadData)
    {
        try
        {
            resetTimestamp = loadData.ContainsKey("resetTimestamp") ? long.Parse(loadData["resetTimestamp"].ToString()) : 0L;

            if (loadData.ContainsKey("dailyQuestDataList"))
            {
                if (dailyQuestDataList == null) dailyQuestDataList = new List<UserDetailDailyQuestData>();
                JsonData jDailyQuestDataList = loadData["dailyQuestDataList"];
                for (int i = 0; i < jDailyQuestDataList.Count; i++)
                {
                    JsonData jDailyQuestData = jDailyQuestDataList[i];
                    UserDetailDailyQuestData newData = new UserDetailDailyQuestData();

                    int id = (jDailyQuestData.ContainsKey("id")) ? int.Parse(jDailyQuestData["id"].ToString()) : 0;
                    double questCount = (jDailyQuestData.ContainsKey("questCount")) ? double.Parse(jDailyQuestData["questCount"].ToString()) : 0;
                    bool received = (jDailyQuestData.ContainsKey("received")) ? bool.Parse(jDailyQuestData["received"].ToString()) : false;
                    if (id <= 0) continue;
                    newData.id = id;
                    newData.questCount = questCount;
                    newData.received = received;

                    UserDetailDailyQuestData prevData = dailyQuestDataList.Find(x => x.id == id);
                    if (prevData != null) continue;

                    dailyQuestDataList.Add(newData);
                }
            }

            if (loadData.ContainsKey("repeatQuestDataList"))
            {
                if (repeatQuestDataList == null) repeatQuestDataList = new List<UserDetailQuestDataBase>();
                JsonData jRailyQuestDataList = loadData["repeatQuestDataList"];
                for (int i = 0; i < jRailyQuestDataList.Count; i++)
                {
                    JsonData jRailyQuestData = jRailyQuestDataList[i];
                    UserDetailQuestDataBase newData = new UserDetailQuestDataBase();

                    int id = (jRailyQuestData.ContainsKey("id")) ? int.Parse(jRailyQuestData["id"].ToString()) : 0;
                    double questCount = (jRailyQuestData.ContainsKey("questCount")) ? double.Parse(jRailyQuestData["questCount"].ToString()) : 0;
                    if (id <= 0) continue;
                    newData.id = id;
                    newData.questCount = questCount;

                    UserDetailQuestDataBase prevData = dailyQuestDataList.Find(x => x.id == id);
                    if (prevData != null) continue;

                    repeatQuestDataList.Add(newData);
                }
            }

            isLoadComplete = true;
        }
        catch (Exception e)
        {
            isLoadComplete = false;
            Debug.LogError(e);
        }
        return isLoadComplete;
    }

    public UserDetailQuestDataBase CreateUserDetailQuestDataAndAddList(QuestDataBase _questDataBase)
    {
        if (_questDataBase.quest_type == eQuestType.eDailyQuest)
        {
            UserDetailDailyQuestData userDetailDailyQuestData = new UserDetailDailyQuestData();
            userDetailDailyQuestData.id = _questDataBase.id;
            userDetailDailyQuestData.questCount = 0;
            userDetailDailyQuestData.received = false;
            dailyQuestDataList.Add(userDetailDailyQuestData);

            return userDetailDailyQuestData;
        }
        else if (_questDataBase.quest_type == eQuestType.eRepeatQuest)
        {
            UserDetailQuestDataBase userDetailQuestData = new UserDetailQuestDataBase();
            userDetailQuestData.id = _questDataBase.id;
            userDetailQuestData.questCount = 0;
            repeatQuestDataList.Add(userDetailQuestData);

            return userDetailQuestData;
        }

        return null;
    }

    public UserDetailQuestDataBase GetUserDetailQuestData(QuestDataBase _questDataBase)
    {
        if (_questDataBase == null || _questDataBase.id <= 0 || _questDataBase.quest_type == eQuestType.eNone)
            return null;

        if (_questDataBase.quest_type == eQuestType.eDailyQuest)
            return dailyQuestDataList.Find(x => x.id == _questDataBase.id);
        else if (_questDataBase.quest_type == eQuestType.eRepeatQuest)
            return repeatQuestDataList.Find(x => x.id == _questDataBase.id);

        return null;
    }
}
[Serializable]
public class UserDungeonData : UserDataBase
{

}
[Serializable]
public class UserDetailBuffData
{
    public int id;
    public float remainTime;
}
[Serializable]
public class UserBuffData : UserDataBase
{

}
[Serializable]
public class UserDetailSummonStoreData
{
    public int id;
    public int level;
    public double exp;
    public long resetTimestamp;
}
[Serializable]
public class UserStoreData : UserDataBase
{
    public List<UserDetailSummonStoreData> summonStoreDataList = new List<UserDetailSummonStoreData>();

    public override void Init()
    {
        base.Init();

        summonStoreDataList = new List<UserDetailSummonStoreData>();
        for (int i = 0; i < 3; i ++)
        {
            UserDetailSummonStoreData newData = new UserDetailSummonStoreData();
            newData.id = (int)E_DATA_ID.eSummonStore_Weapon + i;
            newData.level = 1;
            newData.exp = 0;
            newData.resetTimestamp = 0L;
            summonStoreDataList.Add(newData);
        }
    }
    public override bool Load(JsonData loadData)
    {
        try
        {
            if (loadData.ContainsKey("summonStoreDataList"))
            {
                JsonData jSummonStoreDataList = loadData["summonStoreDataList"];
                for (int i = 0; i < jSummonStoreDataList.Count; i++)
                {
                    JsonData jSummonStoreData = jSummonStoreDataList[i];
                    UserDetailSummonStoreData newData = new UserDetailSummonStoreData();

                    int id = (jSummonStoreData.ContainsKey("id")) ? int.Parse(jSummonStoreData["id"].ToString()) : 0;
                    int level = (jSummonStoreData.ContainsKey("level")) ? int.Parse(jSummonStoreData["level"].ToString()) : 0;
                    double exp = (jSummonStoreData.ContainsKey("exp")) ? double.Parse(jSummonStoreData["exp"].ToString()) : 0;
                    long resetTimestamp = (jSummonStoreData.ContainsKey("resetTimestamp")) ? long.Parse(jSummonStoreData["resetTimestamp"].ToString()) : 0;
                    if (id <= 0) continue;
                    newData.id = id;
                    newData.level = level;

                    UserDetailSummonStoreData prevData = summonStoreDataList.Find(x => x.id == id);
                    if (prevData != null)
                    {
                        if (prevData.level > newData.level)
                            prevData.level = newData.level;
                        if (prevData.exp > newData.exp)
                            prevData.exp = newData.exp;
                        continue;
                    }
                    summonStoreDataList.Add(newData);
                }
            }

            isLoadComplete = true;
        }
        catch (Exception e)
        {
            isLoadComplete = false;
            Debug.LogError(e);
        }
        return isLoadComplete;
    }


    public UserDetailSummonStoreData GetUserDetailSummonStoreData(int _targetId)
    {
        return summonStoreDataList.Find(x => x.id == _targetId);
    }
}


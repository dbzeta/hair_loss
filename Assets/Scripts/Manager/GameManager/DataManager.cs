using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


[System.Serializable]
public class DataManager : MonoBehaviour
{

    [Header("캐릭터 강화 데이터")]
    public SerializableCharacterEnhancementData serializableCharacterEnhancementData = new SerializableCharacterEnhancementData();
    public SerializableCharacterEnhancementPerLevelDicListData serializableCharacterEnhancementPerLevelDicListData = new SerializableCharacterEnhancementPerLevelDicListData();

    [Header("퀘스트 데이터")]
    public SerializableDailyQuestData serializableDailyQuestData = new SerializableDailyQuestData();
    public SerializableRepeatQuestData serializableRepeatQuestData = new SerializableRepeatQuestData();

    [Header("아이템 데이터")]
    public SerializableGoodsItemData serializableGoodsItemData = new SerializableGoodsItemData();
    public SerializableEquipItemData serializableEquipItemData = new SerializableEquipItemData();
    public SerializableRelicItemData serializableRelicItemData = new SerializableRelicItemData();

    [Header("BM 데이터")]
    public SerializableSummonStoreData serializableSummonStoreData = new SerializableSummonStoreData();
    public SerializableSummonStorePerLevelData serializableSummonStorePerLevelData = new SerializableSummonStorePerLevelData();
    public SerializableInAppData serializableInAppData = new SerializableInAppData();
    public SerializableStoreData serializableStoreData = new SerializableStoreData();

    [Header("로컬 라이즈 데이터")]
    public SerializableLocalizeData serializableLocalizeData = new SerializableLocalizeData();
    private Dictionary<string, LocalizeData> dicLocalizeData = new Dictionary<string, LocalizeData>();



    public LocalizeData GetLocalizeData(string keyName)
    {
        if (string.IsNullOrEmpty(keyName))
            return null;

        if (dicLocalizeData.ContainsKey(keyName))
            return dicLocalizeData[keyName];
        else
            return null;
    }

    private void Awake()
    {
        // LoadDataFile();
        // SaveQuestData();
    }

    void Start()
    {

    }

    public bool m_bIsLoadInGameData = false;

    bool m_bInitialized = false;

    public void Init()
    {
        if (m_bInitialized)
            return;

        m_bIsLoadInGameData = false;

        try
        {
            // 필수
            LoadBMData();
            LoadLocalizeData();

            // if (GameManager.Instance.m_UserDataType == eUserDataType.eNone)
            {
                LoadInGameDatas();
                m_bIsLoadInGameData = true;
            }

            m_bInitialized = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void LoadBMData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/BMData/SummonStoreData") as TextAsset;
        serializableSummonStoreData = JsonUtility.FromJson<SerializableSummonStoreData>(textAsset.text);
        
        textAsset = Resources.Load<TextAsset>("Data/BMData/SummonStorePerLevelData") as TextAsset;
        serializableSummonStorePerLevelData = JsonUtility.FromJson<SerializableSummonStorePerLevelData>(textAsset.text);
        
        // textAsset = Resources.Load<TextAsset>("Data/BMData/InAppData") as TextAsset;
        // serializableInAppData = JsonUtility.FromJson<SerializableInAppData>(textAsset.text);
    }
    public void LoadLocalizeData()
    {
        return;
        TextAsset textAsset = Resources.Load<TextAsset>("Data/LocalizeData/LocalizeData") as TextAsset;
        SerializableLocalizeData serializableLocalizeData = JsonUtility.FromJson<SerializableLocalizeData>(textAsset.text);
        dicLocalizeData = new Dictionary<string, LocalizeData>();
        for (int i = 0; i < serializableLocalizeData.datas.Count; i++)
        {
            LocalizeData localizeData = serializableLocalizeData.datas[i];
            dicLocalizeData.Add(localizeData.key, localizeData);
        }
    }

    public void LoadInGameDatas()
    {
        LoadInGameData();
        LoadItemData();
    }
    private void LoadInGameData()
    {
        try
        {
            TextAsset textAsset = Resources.Load<TextAsset>("Data/InGameData/CharacterEnhancementData") as TextAsset;
            serializableCharacterEnhancementData = JsonUtility.FromJson<SerializableCharacterEnhancementData>(textAsset.text);
            
            textAsset = Resources.Load<TextAsset>("Data/InGameData/DailyQuestData") as TextAsset;
            serializableDailyQuestData = JsonUtility.FromJson<SerializableDailyQuestData>(textAsset.text);

            textAsset = Resources.Load<TextAsset>("Data/InGameData/RepeatQuestData") as TextAsset;
            serializableRepeatQuestData = JsonUtility.FromJson<SerializableRepeatQuestData>(textAsset.text);



            for (int i = 0; i < serializableCharacterEnhancementData.datas.Count; i++)
            {
                CharacterEnhancementData characterEnhancementData = serializableCharacterEnhancementData.datas[i];

                int target_id = characterEnhancementData.enhancement_id;
                EINCREASE_STATUS_TYPE increase_status_type = characterEnhancementData.increase_status_type;
                double increase_status_default_value = characterEnhancementData.increase_status_default_value;
                double increase_status_level_per_value = characterEnhancementData.increase_status_level_per_value;

                double prev_gold_amount = 0;
                List<CharacterEnhancementPerLevelListData> listDataList = new List<CharacterEnhancementPerLevelListData>();
                for (int k = characterEnhancementData.start_level; k <= characterEnhancementData.max_level; k += 1000)
                {
                    int start_idx = k;
                    int end_idx = start_idx + 1000;
                    CharacterEnhancementPerLevelListData listData = new CharacterEnhancementPerLevelListData();
                    for (int jj = start_idx; jj < end_idx; jj++)
                    {
                        CharacterEnhancementPerLevelData levelData = new CharacterEnhancementPerLevelData();
                        int target_level = jj;
                        double increase_status_value = increase_status_default_value + (increase_status_level_per_value * target_level);

                        // 골드 증가 계수 : (target_level / 2)
                        double gold_weight = target_level / 2;
                        // 골드 증가율 : Ceiling(골드 증가 계수)
                        double increase_gold = Math.Ceiling(gold_weight);
                        // 골드 : 이전 골드 + 골드 증가율
                        double enhancement_price = prev_gold_amount + increase_gold;
                        prev_gold_amount = enhancement_price;

                        levelData.target_id = target_id;
                        levelData.target_level = target_level;
                        levelData.increase_status_type = increase_status_type;
                        levelData.increase_status_value = increase_status_value;
                        levelData.enhancement_price = enhancement_price;

                        listData.datas.Add(levelData);
                    }
                    listDataList.Add(listData);
                }
                serializableCharacterEnhancementPerLevelDicListData.datas.Add(target_id, listDataList);
            }

        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    private void LoadItemData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/ItemData/GoodsItemData") as TextAsset;
        serializableGoodsItemData = JsonUtility.FromJson<SerializableGoodsItemData>(textAsset.text);
        
        textAsset = Resources.Load<TextAsset>("Data/ItemData/EquipItemData") as TextAsset;
        serializableEquipItemData = JsonUtility.FromJson<SerializableEquipItemData>(textAsset.text);
        
        textAsset = Resources.Load<TextAsset>("Data/ItemData/RelicItemData") as TextAsset;
        serializableRelicItemData = JsonUtility.FromJson<SerializableRelicItemData>(textAsset.text);
        // 
        // textAsset = Resources.Load<TextAsset>("Data/ItemData/RelicItemData") as TextAsset;
        // serializableRelicItemData = JsonUtility.FromJson<SerializableRelicItemData>(textAsset.text);
    }



    public ItemDataBase GetItemData(int _itemID)
    {
        if (_itemID <= 0)
            return null;

        if (_itemID >= (int)E_DATA_ID.eItem_Start && _itemID < (int)E_DATA_ID.eEquipItem_Staff)
            return serializableGoodsItemData.datas.Find(x => x.item_id == _itemID);
        else if (_itemID >= (int)E_DATA_ID.eEquipItem_Staff && _itemID < (int)E_DATA_ID.eRelicItem_Start)
            return serializableEquipItemData.datas.Find(x => x.item_id == _itemID);
        else if (_itemID >= (int)E_DATA_ID.eRelicItem_Start && _itemID < (int)E_DATA_ID.eItem_End)
            return serializableRelicItemData.datas.Find(x => x.item_id == _itemID);

        return null;
    }
    public CharacterEnhancementPerLevelData GetCharacterEnhancementPerLevelData(int _targetID, int _targetLevel)
    {
        if (serializableCharacterEnhancementPerLevelDicListData.datas.ContainsKey(_targetID) == false)
            return null;

        List<CharacterEnhancementPerLevelListData> listDataList = serializableCharacterEnhancementPerLevelDicListData.datas[_targetID];
        if (listDataList == null)
            return null;

        int arrIdx = _targetLevel / 1000;
        if (arrIdx < 0 || arrIdx >= listDataList.Count)
            return null;

        CharacterEnhancementPerLevelListData listData = listDataList[arrIdx];
        if (listData == null)
            return null;

        return listData.datas.Find(x => x.target_level == _targetLevel);
    }



    public string GetRarityText(eRarityType eRarityType)
    {
        switch (eRarityType)
        {
            case eRarityType.eNormal:
                return "노말";
            case eRarityType.eRare:
                return "레어";
            case eRarityType.eEpic:
                return "에픽";
            case eRarityType.eUnique:
                return "유니크";
            case eRarityType.eLegend:
                return "레전드";
            case eRarityType.eMyth:
                return "신화";
            default:
                break;
        }

        return string.Empty;
    }
    public string GetRankText(int rank)
    {
        if (rank == 1) return "Ⅰ";
        if (rank == 2) return "Ⅱ";
        if (rank == 3) return "Ⅲ";
        if (rank == 4) return "Ⅳ";
        if (rank == 5) return "Ⅴ";

        return string.Empty;
    }
}



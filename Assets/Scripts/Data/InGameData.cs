using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

#region Base
[Serializable]
public class InGameData { }
#endregion

#region InGameData

[Serializable]
public class CharacterDataBase : InGameData
{
    public int id;
    public eCharacterType character_type;
}
[Serializable]
public class MonsterData : CharacterDataBase
{
    public bool isBossMonster = false;
}


[Serializable]
public class StageData : InGameData
{
    public int difficult_level;
    public int target_stage_level;
    public double weight_damage;
    public double weight_hp;
    public string textcode_name;
    public string textcode_explain;
}

[Serializable]
public class ItemDataBase : InGameData
{
    public int item_id;
    public eItemType item_type;
    public eRarityType rarity_type;
    public string icon_name;
    public string textcode_name;
    public string textcode_explain;
}
[Serializable]
public class GoodsItemData : ItemDataBase
{
    public eGoodsItemType goods_item_type;
}
[Serializable] 
public class EquipItemData : ItemDataBase
{
    public eEquipItemType equip_item_type;

    public int item_rank;

    public int start_level;
    public int max_level;

    public EINCREASE_STATUS_TYPE[] retention_effect_types;
    public EINCREASE_STATUS_TYPE[] equip_effect_types;
    public double[] retention_effect_values;
    public double[] equip_effect_values;
}
[Serializable]
public class RelicItemData : ItemDataBase
{
    public int start_level;
    public int max_level;

    public EINCREASE_STATUS_TYPE increase_status_type;
    public double increase_status_value;
}

[Serializable]
public class CharacterEnhancementData : InGameData
{
    public int enhancement_id;
    public int start_level;
    public int max_level;
    public EINCREASE_STATUS_TYPE increase_status_type;
    public double increase_status_default_value;
    public double increase_status_level_per_value;
    public string textcode_name;
    public string textcode_explain;
    public string icon_name;
}
[Serializable]
public class CharacterEnhancementPerLevelListData : InGameData
{
    public List<CharacterEnhancementPerLevelData> datas = new List<CharacterEnhancementPerLevelData>();
}
[Serializable]
public class CharacterEnhancementPerLevelData : InGameData
{
    public int target_id;
    public int target_level;
    public EINCREASE_STATUS_TYPE increase_status_type;
    public double increase_status_value;
    public double enhancement_price;
}

[Serializable]
public class StoreData : InGameData
{
    public int store_id;
    public int sale_item_id;
    public double sale_item_count;
    public int price_item_id;
    public double price;
}

[Serializable]
public class SummonStoreData : InGameData
{
    public int store_id;
    public eSummonStoreType summon_store_type;
    public int start_level;
    public int max_level;

    public double[] summon_item_counts;
    public int[] price_item_ids;
    public double[] price_item_counts;

    public string textcode_name;
    public string textcode_explain;
    public string icon_name;
}
[Serializable]
public class SummonStorePerLevelData
{
    public int target_level;
    public double exp_min;
    public double exp_max;

    public List<float> rarity_probabilities;
    public List<float> rank_probabilities;
}
[Serializable]
public class InAppData : InGameData
{
    public int target_store_id;
    public string textcode_name;
    public string textcode_explain;
    public string icon_name;
    public int[] target_item_ids;
    public double[] target_item_counts;
    // public UnityEngine.Purchasing.ProductType product_type;
    public double price;
    public string inapp_product_id;
    public string inapp_google_store_id;
    public string inapp_ios_store_id;
}

[Serializable]
public class QuestDataBase
{
    public int id;
    public eQuestType quest_type;

    public double quest_count;
    public int reward_item_id;
    public double reward_item_count;
    public string textcode_name;
    public eQuestConditionType quest_condition_type;
}

[Serializable]
public class DailyQuestData : QuestDataBase { }
[Serializable]
public class RepeatQuestData : QuestDataBase { }


[Serializable]
public class LocalizeData : InGameData
{
    public string key;
    public string text_kor;
    public string text_eng;
    public string text_jpn;
}

#endregion

#region Serializable
[Serializable]
public class SerializableGoodsItemData
{
    public List<GoodsItemData> datas = new List<GoodsItemData>();
}
[Serializable]
public class SerializableEquipItemData
{
    public List<EquipItemData> datas = new List<EquipItemData>();
}
[Serializable]
public class SerializableRelicItemData
{
    public List<RelicItemData> datas = new List<RelicItemData>();
}

[Serializable]
public class SerializableCharacterEnhancementData
{
    public List<CharacterEnhancementData> datas = new List<CharacterEnhancementData>();
}
[Serializable]
public class SerializableCharacterEnhancementPerLevelListData
{
    public List<CharacterEnhancementPerLevelListData> datas = new List<CharacterEnhancementPerLevelListData>();
}
[Serializable]
public class SerializableCharacterEnhancementPerLevelDicListData
{
    public Dictionary<int, List<CharacterEnhancementPerLevelListData>> datas = new Dictionary<int, List<CharacterEnhancementPerLevelListData>>();
}
[Serializable]
public class SerializableStoreData
{
    public List<StoreData> datas = new List<StoreData>();
}
[Serializable]
public class SerializableSummonStoreData
{
    public List<SummonStoreData> datas = new List<SummonStoreData>();
}
[Serializable]
public class SerializableSummonStorePerLevelData
{
    public List<SummonStorePerLevelData> datas = new List<SummonStorePerLevelData>();
}
[Serializable]
public class SerializableInAppData
{
    public List<InAppData> datas = new List<InAppData>();
}
[Serializable]
public class SerializableDailyQuestData
{
    public List<DailyQuestData> datas = new List<DailyQuestData>();
}
[Serializable]
public class SerializableRepeatQuestData
{
    public List<RepeatQuestData> datas = new List<RepeatQuestData>();
}
[Serializable]
public class SerializableLocalizeData
{
    public List<LocalizeData> datas = new List<LocalizeData>();
}
#endregion

[Serializable]
public class UIPopupBaseData
{
    public int index;
    public string folder_name;
    public string popup_name;
}
[Serializable]
public class SerializableUIPopupBaseData
{
    public List<UIPopupBaseData> datas = new List<UIPopupBaseData>();
}



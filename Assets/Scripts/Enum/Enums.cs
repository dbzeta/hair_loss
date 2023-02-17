public enum EQUEST_TYPE
{

    eNone = -1,
}

public enum ESOUND_TYPE
{
    E_NONE = 1,

    E_BGM_INGAME = 10,
    E_BGM_TOWN,

    E_EFFECT_SOUND_BTN_ALERT = 100,
    E_EFFECT_SOUND_BTN_BUS,
    E_EFFECT_SOUND_BTN_CANCEL,
    E_EFFECT_SOUND_BTN_COMMON,
    E_EFFECT_SOUND_BTN_CONFIRM,
    E_EFFECT_SOUND_BTN_EXPLODE,
    E_EFFECT_SOUND_BTN_FALL,
    E_EFFECT_SOUND_BTN_LEFT,
    E_EFFECT_SOUND_BTN_RIGHT,
    E_EFFECT_SOUND_BTN_INTRO,

}

public enum E_DATA_ID
{
    eCharacter_Start = 100000,
    eCharacter_Player = 100000,
    eCharacter_End = 199999,

    eItem_Start = 200000,
    eGoodsItem_Diamond = 200000,
    eGoodsItem_Gold,
    eGoodsItem_Ruby,

    eEquipItem_Staff = 201000,
    eEquipItem_Necklace = 202000,
    eEquipItem_Ring = 203000,
    eEquipItem_Costume = 204000,

    eRelicItem_Start = 205000,

    eItem_End = 299999,

    eEnhance_Damage = 300000,
    eEnhance_Critical,
    eEnhance_CriticalDamage,

    eSummonStore_Weapon = 400000,
    eSummonStore_Necklace,
    eSummonStore_Ring,

    eDailyQuest_ClearDailyQuest = 500000,
    eDailyQuest_Enhancement_Chracter,
    eDailyQuest_Enhancement_Equipment,
    eDailyQuest_Merge_Equipment,
    eDailyQuest_Enter_Dungeon,
    eDailyQuest_UseSkill,
    eDailyQuest_Summon,
    eDailyQuest_Kill_Monster,
    eDailyQuest_PlayTime,
    eDailyQuest_ShowAds,

    eRepeatQuest_UseSkill_Buff = 510000,
    eRepeatQuest_UseSkill_Active,
    eRepeatQuest_Kill_Monster,
    eRepeatQuest_PlayTime,
    eRepeatQuest_Enhancement_Equipment,
    eRepeatQuest_Merge_Equipment,
    eRepeatQuest_Summon,


    eNone = -1,
}
public enum eCharacterType
{
    ePlayer = 0,
    eEnemy,
    ePet,

    eNone = -1,
}
public enum eItemType
{ 
    eGoodsItem = 0,
    eEquipItem,
    eRelicItem,
    eCostumeItem,
    eNone = -1,
}

public enum eRarityType
{
    eNormal = 0,
    eRare,
    eEpic,
    eUnique,
    eLegend,
    eMyth,
    eNone = -1,
}

public enum eBuffType
{
    eBuff_Damage,
    eBuff_Gold,
    eBuff_Exp,
    eNone = -1,
}
public enum EINCREASE_STATUS_TYPE
{
    // 전투
    eIncreaseDamage = 0,
    eIncreaseDamageRate,
    eIncreaseFinalDamageRate,
    eIncreaseAttackSpeed,
    eIncreaseMoveSpeed,
    eIncreaseCriticalRate,
    eIncreaseCriticalDamage,
    eIncreaseSuperCriticalRate,
    eIncreaseSuperCriticalDamage,

    // 지원
    eIncreaseGoldAcquisition = 1000,
    eIncreaseExpAcquisition,

    // 
    eDecreaseNormalMonsterHP,
    eDecreaseBossMonsterHP,
    eDecreaseDungeonMonsterHP,

}

public enum eGoodsItemType
{
    eDiamond,
    eGold,
    eRuby,
    eNone = -1,
}
public enum eEquipItemType
{
    eWeapon,
    eNecklace,
    eRing,
    eCostume,
    eNone = -1,
}
public enum eRelicItemType
{
    e,
    eNone = -1,
}


public enum eSummonStoreType
{
    eWeapon,
    eNecklace,
    eRing,
    eNone  = -1,
}


public enum eQuestType
{
    eDailyQuest,
    eRepeatQuest,
    eNone = -1,
}

public enum eQuestConditionType
{
    eClearDailyQuest,
    eEnhancement_Chracter,
    eEnhancement_Equipment,
    eMerge_Equipment,
    eEnter_Dungeon,
    eUseSkill,
    eUseSkill_Buff,
    eUseSkill_Active,
    eSummon,
    eKill_Monster,
    ePlayTime,
    eShowAds,
    eNone = -1,
}
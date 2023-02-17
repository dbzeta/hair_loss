using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    UserQuestData m_UserQuestData = null;

    public Action<QuestDataBase> m_AddQuestData = null;

    bool m_bInitialzed = false;
    public void Init()
    {
        if (m_bInitialzed)
            return;

        if (m_UserQuestData == null)
            m_UserQuestData = GameManager.Instance.UserData.m_UserQuestData;

        m_bInitialzed = true;
    }

    public void Setup()
    {
        if (!m_bInitialzed)
            Init();

        if (m_UserQuestData == null)
            m_UserQuestData = GameManager.Instance.UserData.m_UserQuestData;

        long curTimestamp = GameManager.Instance.TimeMgr.GetCurrentTimeStamp();
        long resetTimestamp = m_UserQuestData.resetTimestamp;

        bool isResetDailyQuest = curTimestamp > resetTimestamp;
        if (isResetDailyQuest) 
            ResetDailyQuest();

    }

    public void AddQuestData(eQuestConditionType _questConditionType, double _addQuestCount, bool bSaveData = true)
    {
        switch (_questConditionType)
        {
            case eQuestConditionType.eClearDailyQuest:
                {
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eDailyQuest_ClearDailyQuest), _addQuestCount);
                    break;
                }
            case eQuestConditionType.eEnhancement_Chracter:
                {
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eDailyQuest_Enhancement_Chracter), _addQuestCount);
                    break;
                }
            case eQuestConditionType.eEnhancement_Equipment:
                {
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eDailyQuest_Enhancement_Equipment), _addQuestCount);
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eRepeatQuest_Enhancement_Equipment), _addQuestCount);
                    break;
                }
            case eQuestConditionType.eMerge_Equipment:
                {
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eDailyQuest_Merge_Equipment), _addQuestCount);
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eRepeatQuest_Merge_Equipment), _addQuestCount);
                    break;
                }
            case eQuestConditionType.eEnter_Dungeon:
                {
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eDailyQuest_Enter_Dungeon), _addQuestCount);
                    break;
                }
            case eQuestConditionType.eUseSkill:
                {
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eDailyQuest_UseSkill), _addQuestCount);
                    break;
                }
            case eQuestConditionType.eUseSkill_Buff:
                {
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eDailyQuest_UseSkill), _addQuestCount);
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eRepeatQuest_UseSkill_Buff), _addQuestCount);
                    break;
                }
            case eQuestConditionType.eUseSkill_Active:
                {
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eDailyQuest_UseSkill), _addQuestCount);
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eRepeatQuest_UseSkill_Active), _addQuestCount);
                    break;
                }
            case eQuestConditionType.eSummon:
                {
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eDailyQuest_Summon), _addQuestCount);
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eRepeatQuest_Summon), _addQuestCount);
                    break;
                }
            case eQuestConditionType.eKill_Monster:
                {
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eDailyQuest_Kill_Monster), _addQuestCount);
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eRepeatQuest_Kill_Monster), _addQuestCount);
                    break;
                }
            case eQuestConditionType.ePlayTime:
                {
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eDailyQuest_PlayTime), _addQuestCount);
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eRepeatQuest_PlayTime), _addQuestCount);
                    break;
                }
            case eQuestConditionType.eShowAds:
                {
                    AddQuestData(GetQuestDataBase((int)E_DATA_ID.eDailyQuest_ShowAds), _addQuestCount);
                    break;
                }
            default:
                break;
        }
    }
    public void AddQuestData(QuestDataBase _questDataBase, double _addQuestCount, bool bSaveData = true)
    {
        if (_questDataBase == null || _questDataBase.id <= 0)
            return;

        UserDetailQuestDataBase userDetailQuestData = GetUserDetailQuestData(_questDataBase.id);
        if (userDetailQuestData == null)
            userDetailQuestData = m_UserQuestData.CreateUserDetailQuestDataAndAddList(_questDataBase);

        if (_questDataBase.quest_type == eQuestType.eDailyQuest)
        {
            UserDetailDailyQuestData userDetailDailyQuestData = userDetailQuestData as UserDetailDailyQuestData;
            if (userDetailDailyQuestData.received)
                return;

            userDetailDailyQuestData.questCount += _addQuestCount;
            if (userDetailDailyQuestData.questCount >= _questDataBase.quest_count)
            {
                userDetailDailyQuestData.questCount = _questDataBase.quest_count;
            }
        }
        else if (_questDataBase.quest_type == eQuestType.eRepeatQuest)
        {
            userDetailQuestData.questCount += _addQuestCount;
        }

        if (m_AddQuestData != null)
            m_AddQuestData(_questDataBase);

        if (bSaveData)
            m_UserQuestData.SaveData();
    }

    public void SetQuestData(QuestDataBase _questDataBase, double _questCount, bool bSaveData = true)
    {
        if (_questDataBase == null || _questDataBase.id <= 0)
            return;

        UserDetailQuestDataBase userDetailQuestDataBase = GetUserDetailQuestData(_questDataBase.id);
        if (userDetailQuestDataBase == null)
            userDetailQuestDataBase = m_UserQuestData.CreateUserDetailQuestDataAndAddList(_questDataBase);

        userDetailQuestDataBase.questCount = _questCount;

        if (bSaveData) 
            m_UserQuestData.SaveData();
    }
    public void ClearDailyQuest(DailyQuestData _dailyQuestData, bool bSaveData = true)
    {
        if (_dailyQuestData == null || _dailyQuestData.quest_type != eQuestType.eDailyQuest)
            return;

        UserDetailQuestDataBase userDetailQuestData = GetUserDetailQuestData(_dailyQuestData.id);
        if (userDetailQuestData == null)
            userDetailQuestData = m_UserQuestData.CreateUserDetailQuestDataAndAddList(_dailyQuestData);

        UserDetailDailyQuestData userDetailDailyQuestData = userDetailQuestData as UserDetailDailyQuestData;
        userDetailDailyQuestData.questCount = _dailyQuestData.quest_count;
        userDetailDailyQuestData.received = true;

        if (bSaveData) 
            m_UserQuestData.SaveData();
    }

    public void ResetDailyQuest()
    {
        m_UserQuestData.resetTimestamp = GameManager.Instance.TimeMgr.GetTimeStampAddDays(1);

        List<DailyQuestData> dailyQuestDatas = GameManager.Instance.Data.serializableDailyQuestData.datas;
        for (int i = 0; i < dailyQuestDatas.Count; i++)
        {
            DailyQuestData dailyQuestData = dailyQuestDatas[i];
            UserDetailDailyQuestData userDetailDailyQuestData =
                m_UserQuestData.GetUserDetailQuestData(dailyQuestData) as UserDetailDailyQuestData;

            if (userDetailDailyQuestData == null)
                m_UserQuestData.CreateUserDetailQuestDataAndAddList(dailyQuestData);
            else
            {
                userDetailDailyQuestData.received = false;
                userDetailDailyQuestData.questCount = 0;
            }
        }
    }


    public QuestDataBase GetQuestDataBase(int _questId)
    {
        if (_questId >= (int)E_DATA_ID.eDailyQuest_ClearDailyQuest && _questId < (int)E_DATA_ID.eRepeatQuest_UseSkill_Buff)
            return GameManager.Instance.Data.serializableDailyQuestData.datas.Find(x => x.id == _questId);
        else
            return GameManager.Instance.Data.serializableRepeatQuestData.datas.Find(x => x.id == _questId);
    }
    public UserDetailQuestDataBase GetUserDetailQuestData(int _questId)
    {
        QuestDataBase questDataBase = GetQuestDataBase(_questId);
        if (questDataBase == null)
            return null;

        return m_UserQuestData.GetUserDetailQuestData(questDataBase);
    }

    public void Release()
    {
        m_AddQuestData = null;
    }
}

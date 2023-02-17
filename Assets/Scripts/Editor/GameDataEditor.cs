using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Data;
// using Newtonsoft.Json;


#if UNITY_EDITOR

public class GameDataEditor : EditorWindow
{

    [SerializeField] SerializableGoodsItemData serializableGoodsItemData = new SerializableGoodsItemData();
    [SerializeField] SerializableEquipItemData serializableEquipItemData = new SerializableEquipItemData();
    [SerializeField] SerializableRelicItemData serializableRelicItemData = new SerializableRelicItemData();

    [SerializeField] SerializableCharacterEnhancementData serializableCharacterEnhancementData = new SerializableCharacterEnhancementData();

    [SerializeField] SerializableDailyQuestData serializableDailyQuestData = new SerializableDailyQuestData();
    [SerializeField] SerializableRepeatQuestData serializableRepeatQuestData = new SerializableRepeatQuestData();

    [SerializeField] SerializableSummonStoreData serializableSummonStoreData = new SerializableSummonStoreData();
    [SerializeField] SerializableSummonStorePerLevelData serializableSummonStorePerLevelData = new SerializableSummonStorePerLevelData();
    [SerializeField] SerializableInAppData serializableInAppData = new SerializableInAppData();

    [SerializeField] SerializableLocalizeData serializableLocalizeData = new SerializableLocalizeData();


    private readonly string[] m_MainToolBarNames = { "마을", "인게임", "아이템", "BM", "언어" };
    private readonly string[] m_TownToolBarNames = { "동물", "빌딩", "오브젝트(b)","오브젝트", "대화", "퀘스트" };
    private readonly string[] m_InGameToolBarNames = { "퀘스트", "맵" };
    private readonly string[] m_ItemToolBarNames = { "재화", "인게임", "소모품", "사진", "코스튬" };
    private readonly string[] m_BMToolBarNames = { "소환 상점" };


    private int m_MainToolBarIndex = 0;
    private int m_InGameToolBarIndex = 0;
    private int m_TownToolBarIndex = 0;
    private int m_ItemToolBarIndex = 0;
    private int m_BMToolBarIndex = 0;

    [MenuItem("Window/Game Data Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(GameDataEditor)).Show();
    }
    private Vector2 scrollPos = Vector2.zero;

    #region _OnGUI
    private void OnGUI()
    {
        m_MainToolBarIndex = GUILayout.Toolbar(m_MainToolBarIndex, m_MainToolBarNames);
        switch (m_MainToolBarIndex)
        {
            case 0:
                {
                    m_TownToolBarIndex = GUILayout.Toolbar(m_TownToolBarIndex, m_TownToolBarNames);
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, true, GUILayout.Width(GetWindow(typeof(GameDataEditor)).position.width), GUILayout.Height(GetWindow(typeof(GameDataEditor)).position.height));
                    OnGUI_Town();
                    break;
                }
            case 1:
                {
                    m_InGameToolBarIndex = GUILayout.Toolbar(m_InGameToolBarIndex, m_InGameToolBarNames);
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, true, GUILayout.Width(GetWindow(typeof(GameDataEditor)).position.width), GUILayout.Height(GetWindow(typeof(GameDataEditor)).position.height));
                    OnGUI_InGame();
                    break;
                }
            case 2:
                {
                    m_ItemToolBarIndex = GUILayout.Toolbar(m_ItemToolBarIndex, m_ItemToolBarNames);
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, true, GUILayout.Width(GetWindow(typeof(GameDataEditor)).position.width), GUILayout.Height(GetWindow(typeof(GameDataEditor)).position.height));
                    OnGUI_Item();
                    break;
                }
            case 3:
                {
                    m_BMToolBarIndex = GUILayout.Toolbar(m_BMToolBarIndex, m_BMToolBarNames);
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, true, GUILayout.Width(GetWindow(typeof(GameDataEditor)).position.width), GUILayout.Height(GetWindow(typeof(GameDataEditor)).position.height));
                    OnGUI_BM();
                    break;
                }
            case 4:
                {
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, true, GUILayout.Width(GetWindow(typeof(GameDataEditor)).position.width), GUILayout.Height(GetWindow(typeof(GameDataEditor)).position.height));
                    OnGUI_Localization();
                    break;
                }
        }


        GUILayout.Label("! 해당 문서가 열려있을 경우 로드가 불가능 합니다.");
        GUILayout.Space(10.0f);

        GUILayout.Button("Refrash Datas");

        EditorGUILayout.EndScrollView();
    }

    private void OnGUI_Town()
    {
        GUILayout.Space(20.0f);
    }
    private void OnGUI_InGame()
    {
        GUILayout.Label("-성장 데이터-");
        GUILayout.Space(5.0f);
        if (serializableCharacterEnhancementData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("serializableCharacterEnhancementData");

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (serializableCharacterEnhancementData.datas.Count == 0)
                GUILayout.Label("데이터가 비어 있습니다.");

            if (GUILayout.Button("Save CharacterEnhancementData"))
            {
                ConvertCharacterEnhancementDataToJson();
            }
        }

        GUILayout.Label("-일일 퀘스트 데이터-");
        GUILayout.Space(5.0f);
        if (serializableDailyQuestData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("serializableDailyQuestData");

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (serializableDailyQuestData.datas.Count == 0)
                GUILayout.Label("데이터가 비어 있습니다.");

            if (GUILayout.Button("Save DailyQuestData"))
            {
                ConvertDailyQuestDataToJson();
            }
        }
        GUILayout.Label("-반복 퀘스트 데이터-");
        GUILayout.Space(5.0f);
        if (serializableRepeatQuestData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("serializableRepeatQuestData");

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (serializableRepeatQuestData.datas.Count == 0)
                GUILayout.Label("데이터가 비어 있습니다.");

            if (GUILayout.Button("Save RepeatQuestData"))
            {
                ConvertRepeatQuestDataToJson();
            }
        }
    }
    private void OnGUI_Item()
    {
        GUILayout.Label("-재화 데이터-");
        GUILayout.Space(5.0f);
        if (serializableGoodsItemData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("serializableGoodsItemData");

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (serializableGoodsItemData.datas.Count == 0)
                GUILayout.Label("데이터가 비어 있습니다.");

            if (GUILayout.Button("Save GoodsItemData"))
            {
                ConvertGoodsItemDataToJson();
            }
        }

        GUILayout.Label("-장비 데이터-");
        GUILayout.Space(5.0f);
        if (serializableEquipItemData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("serializableEquipItemData");

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (serializableEquipItemData.datas.Count == 0)
                GUILayout.Label("데이터가 비어 있습니다.");

            if (GUILayout.Button("Save EquipItemData"))
            {
                ConvertEquipItemDataToJson();
            }
        }

        GUILayout.Label("-유물 데이터-");
        GUILayout.Space(5.0f);
        if (serializableRelicItemData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("serializableRelicItemData");

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (serializableRelicItemData.datas.Count == 0)
                GUILayout.Label("데이터가 비어 있습니다.");

            if (GUILayout.Button("Save RelicItemData"))
            {
                ConvertRelicItemDataToJson();
            }
        }
    }

    private void OnGUI_BM()
    {
        GUILayout.Label("-소환 데이터-");
        GUILayout.Space(5.0f);

        if (serializableSummonStoreData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("serializableSummonStoreData");

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (serializableSummonStoreData.datas.Count == 0)
                GUILayout.Label("데이터가 비어 있습니다.");

            if (GUILayout.Button("Save SummonStoreData"))
            {
                ConvertSummonStoreDataToJson(); 
            }
        }
        if (serializableSummonStorePerLevelData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("serializableSummonStorePerLevelData");

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (serializableSummonStorePerLevelData.datas.Count == 0)
                GUILayout.Label("데이터가 비어 있습니다.");

            if (GUILayout.Button("Save SummonStorePerLevelData"))
            {
                ConvertSummonStorePerLevelDataToJson();
            }
        }

        GUILayout.Label("-인앱 데이터-");
        GUILayout.Space(5.0f);

        if (serializableInAppData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("serializableInAppData");

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (serializableInAppData.datas.Count == 0)
                GUILayout.Label("데이터가 비어 있습니다.");

            if (GUILayout.Button("Save InApp Data"))
            {
                ConvertInAppDataToJson();
            }
        }
    }
    private void OnGUI_Localization()
    {
        GUILayout.Space(20.0f);
        if (serializableLocalizeData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("serializableLocalizeData");

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();
            if (serializableLocalizeData.datas == null || serializableLocalizeData.datas.Count == 0)
                GUILayout.Label("데이터가 비어 있습니다.");

            if (GUILayout.Button("Save Localize Data"))
            {
                ConvertLocalizeDataToJson();
            }
        }
    }

    #endregion


    
    private void ConvertCharacterEnhancementDataToJson()
    {
        string CSVText = LoadCSVFile();
        if (string.IsNullOrEmpty(CSVText))
            return;

        List<Dictionary<string, string>> JsonObjList = UtilsClass.ConvertCSVFileToJsonObject(CSVText);
        if (JsonObjList != null)
        {
            serializableCharacterEnhancementData = new SerializableCharacterEnhancementData();
            for (int i = 0; i < JsonObjList.Count; i++)
            {
                Dictionary<string, string> JsonObjDic = JsonObjList[i];

                CharacterEnhancementData characterEnhancementData = new CharacterEnhancementData();

                characterEnhancementData.enhancement_id = UtilsClass.ParseToType<int>(JsonObjDic["enhancement_id"]);
                characterEnhancementData.start_level = UtilsClass.ParseToType<int>(JsonObjDic["start_level"]);
                characterEnhancementData.max_level = UtilsClass.ParseToType<int>(JsonObjDic["max_level"]);
                characterEnhancementData.increase_status_type = UtilsClass.ParseToType<EINCREASE_STATUS_TYPE>(JsonObjDic["increase_status_type"]);
                characterEnhancementData.increase_status_default_value = UtilsClass.ParseToType<double>(JsonObjDic["increase_status_default_value"]);
                characterEnhancementData.increase_status_level_per_value = UtilsClass.ParseToType<double>(JsonObjDic["increase_status_level_per_value"]);
                characterEnhancementData.textcode_name = UtilsClass.ParseToType<string>(JsonObjDic["textcode_name"]);
                characterEnhancementData.textcode_explain = UtilsClass.ParseToType<string>(JsonObjDic["textcode_explain"]);
                characterEnhancementData.icon_name = UtilsClass.ParseToType<string>(JsonObjDic["icon_name"]);

                serializableCharacterEnhancementData.datas.Add(characterEnhancementData);
            }
            SaveGameData(serializableCharacterEnhancementData);
        }
    }
    private void ConvertDailyQuestDataToJson()
    {
        string CSVText = LoadCSVFile();
        if (string.IsNullOrEmpty(CSVText))
            return;

        List<Dictionary<string, string>> JsonObjList = UtilsClass.ConvertCSVFileToJsonObject(CSVText);
        if (JsonObjList != null)
        {
            serializableDailyQuestData = new SerializableDailyQuestData();
            for (int i = 0; i < JsonObjList.Count; i++)
            {
                Dictionary<string, string> JsonObjDic = JsonObjList[i];

                DailyQuestData dailyQuestData = new DailyQuestData();

                dailyQuestData.id = UtilsClass.ParseToType<int>(JsonObjDic["id"]);
                dailyQuestData.quest_type = UtilsClass.ParseToType<eQuestType>(JsonObjDic["quest_type"]);
                dailyQuestData.quest_count = UtilsClass.ParseToType<double>(JsonObjDic["quest_count"]);
                dailyQuestData.reward_item_id = UtilsClass.ParseToType<int>(JsonObjDic["reward_item_id"]);
                dailyQuestData.reward_item_count = UtilsClass.ParseToType<double>(JsonObjDic["reward_item_count"]);
                dailyQuestData.textcode_name = UtilsClass.ParseToType<string>(JsonObjDic["textcode_name"]);
                dailyQuestData.quest_condition_type = UtilsClass.ParseToType<eQuestConditionType>(JsonObjDic["quest_condition_type"]);
                serializableDailyQuestData.datas.Add(dailyQuestData);
            }
            SaveGameData(serializableDailyQuestData);
        }
    }
    private void ConvertRepeatQuestDataToJson()
    {
        string CSVText = LoadCSVFile();
        if (string.IsNullOrEmpty(CSVText))
            return;

        List<Dictionary<string, string>> JsonObjList = UtilsClass.ConvertCSVFileToJsonObject(CSVText);
        if (JsonObjList != null)
        {
            serializableRepeatQuestData = new SerializableRepeatQuestData();
            for (int i = 0; i < JsonObjList.Count; i++)
            {
                Dictionary<string, string> JsonObjDic = JsonObjList[i];

                RepeatQuestData repeatQuestData = new RepeatQuestData();

                repeatQuestData.id = UtilsClass.ParseToType<int>(JsonObjDic["id"]);
                repeatQuestData.quest_type = UtilsClass.ParseToType<eQuestType>(JsonObjDic["quest_type"]);
                repeatQuestData.quest_count = UtilsClass.ParseToType<double>(JsonObjDic["quest_count"]);
                repeatQuestData.reward_item_id = UtilsClass.ParseToType<int>(JsonObjDic["reward_item_id"]);
                repeatQuestData.reward_item_count = UtilsClass.ParseToType<double>(JsonObjDic["reward_item_count"]);
                repeatQuestData.textcode_name = UtilsClass.ParseToType<string>(JsonObjDic["textcode_name"]);
                repeatQuestData.quest_condition_type = UtilsClass.ParseToType<eQuestConditionType>(JsonObjDic["quest_condition_type"]);
                serializableRepeatQuestData.datas.Add(repeatQuestData);
            }
            SaveGameData(serializableRepeatQuestData);
        }
    }

    private void ConvertGoodsItemDataToJson()
    {
        string CSVText = LoadCSVFile();
        if (string.IsNullOrEmpty(CSVText))
            return;

        List<Dictionary<string, string>> JsonObjList = UtilsClass.ConvertCSVFileToJsonObject(CSVText);
        if (JsonObjList != null)
        {
            serializableGoodsItemData = new SerializableGoodsItemData();
            for (int i = 0; i < JsonObjList.Count; i++)
            {
                Dictionary<string, string> JsonObjDic = JsonObjList[i];

                GoodsItemData goodsItemData = new GoodsItemData();

                goodsItemData.item_id = UtilsClass.ParseToType<int>(JsonObjDic["item_id"]);
                goodsItemData.item_type = UtilsClass.ParseToType<eItemType>(JsonObjDic["item_type"]);
                goodsItemData.rarity_type = UtilsClass.ParseToType<eRarityType>(JsonObjDic["rarity_type"]);
                goodsItemData.icon_name = UtilsClass.ParseToType<string>(JsonObjDic["icon_name"]);
                goodsItemData.textcode_name = UtilsClass.ParseToType<string>(JsonObjDic["textcode_name"]);
                goodsItemData.textcode_explain = UtilsClass.ParseToType<string>(JsonObjDic["textcode_explain"]);

                goodsItemData.goods_item_type = UtilsClass.ParseToType<eGoodsItemType>(JsonObjDic["goods_item_type"]);

                serializableGoodsItemData.datas.Add(goodsItemData);
            }
            SaveGameData(serializableGoodsItemData);
        }
    }
    private void ConvertEquipItemDataToJson()
    {
        string CSVText = LoadCSVFile();
        if (string.IsNullOrEmpty(CSVText))
            return;

        List<Dictionary<string, string>> JsonObjList = UtilsClass.ConvertCSVFileToJsonObject(CSVText);
        if (JsonObjList != null)
        {
            serializableEquipItemData = new SerializableEquipItemData();
            for (int i = 0; i < JsonObjList.Count; i++)
            {
                Dictionary<string, string> JsonObjDic = JsonObjList[i];

                EquipItemData equipItemData = new EquipItemData();

                equipItemData.item_id = UtilsClass.ParseToType<int>(JsonObjDic["item_id"]);
                equipItemData.item_type = UtilsClass.ParseToType<eItemType>(JsonObjDic["item_type"]);
                equipItemData.rarity_type = UtilsClass.ParseToType<eRarityType>(JsonObjDic["rarity_type"]);
                equipItemData.icon_name = UtilsClass.ParseToType<string>(JsonObjDic["icon_name"]);
                equipItemData.textcode_name = UtilsClass.ParseToType<string>(JsonObjDic["textcode_name"]);
                equipItemData.textcode_explain = UtilsClass.ParseToType<string>(JsonObjDic["textcode_explain"]);

                equipItemData.equip_item_type = UtilsClass.ParseToType<eEquipItemType>(JsonObjDic["equip_item_type"]);
                equipItemData.item_rank = UtilsClass.ParseToType<int>(JsonObjDic["item_rank"]);
                equipItemData.start_level = UtilsClass.ParseToType<int>(JsonObjDic["start_level"]);
                equipItemData.max_level = UtilsClass.ParseToType<int>(JsonObjDic["max_level"]);

                string[] equip_effect_types = JsonObjDic["equip_effect_types"].Split('\\');
                equipItemData.equip_effect_types = new EINCREASE_STATUS_TYPE[equip_effect_types.Length];
                for (int k = 0; k < equip_effect_types.Length; k++)
                {
                    equipItemData.equip_effect_types[k] = UtilsClass.ParseToType<EINCREASE_STATUS_TYPE>(equip_effect_types[k]);
                }
                string[] equip_effect_values = JsonObjDic["equip_effect_values"].Split('\\');
                equipItemData.equip_effect_values = new double[equip_effect_values.Length];
                for (int k = 0; k < equip_effect_values.Length; k++)
                {
                    equipItemData.equip_effect_values[k] = UtilsClass.ParseToType<double>(equip_effect_values[k]);
                }

                string[] retention_effect_types = JsonObjDic["retention_effect_types"].Split('\\');
                equipItemData.retention_effect_types = new EINCREASE_STATUS_TYPE[retention_effect_types.Length];
                for (int k = 0; k < retention_effect_types.Length; k++)
                {
                    equipItemData.retention_effect_types[k] = UtilsClass.ParseToType<EINCREASE_STATUS_TYPE>(retention_effect_types[k]);
                }
                string[] retention_effect_values = JsonObjDic["retention_effect_values"].Split('\\');
                equipItemData.retention_effect_values = new double[retention_effect_values.Length];
                for (int k = 0; k < retention_effect_values.Length; k++)
                {
                    equipItemData.retention_effect_values[k] = UtilsClass.ParseToType<double>(retention_effect_values[k]);
                }

                serializableEquipItemData.datas.Add(equipItemData);
            }
            SaveGameData(serializableEquipItemData);
        }
    }
    private void ConvertRelicItemDataToJson()
    {
        string CSVText = LoadCSVFile();
        if (string.IsNullOrEmpty(CSVText))
            return;

        List<Dictionary<string, string>> JsonObjList = UtilsClass.ConvertCSVFileToJsonObject(CSVText);
        if (JsonObjList != null)
        {
            serializableRelicItemData = new SerializableRelicItemData();
            for (int i = 0; i < JsonObjList.Count; i++)
            {
                Dictionary<string, string> JsonObjDic = JsonObjList[i];

                RelicItemData relicItemData = new RelicItemData();

                relicItemData.item_id = UtilsClass.ParseToType<int>(JsonObjDic["item_id"]);
                relicItemData.item_type = UtilsClass.ParseToType<eItemType>(JsonObjDic["item_type"]);
                relicItemData.rarity_type = UtilsClass.ParseToType<eRarityType>(JsonObjDic["rarity_type"]);
                relicItemData.icon_name = UtilsClass.ParseToType<string>(JsonObjDic["icon_name"]);
                relicItemData.textcode_name = UtilsClass.ParseToType<string>(JsonObjDic["textcode_name"]);
                relicItemData.textcode_explain = UtilsClass.ParseToType<string>(JsonObjDic["textcode_explain"]);

                relicItemData.start_level = UtilsClass.ParseToType<int>(JsonObjDic["start_level"]);
                relicItemData.max_level = UtilsClass.ParseToType<int>(JsonObjDic["max_level"]);
                relicItemData.increase_status_type = UtilsClass.ParseToType<EINCREASE_STATUS_TYPE>(JsonObjDic["increase_status_type"]);
                relicItemData.increase_status_value = UtilsClass.ParseToType<double>(JsonObjDic["increase_status_value"]);

                serializableRelicItemData.datas.Add(relicItemData);
            }
            SaveGameData(serializableRelicItemData);
        }
    }
    private void ConvertSummonStoreDataToJson()
    {
        string CSVText = LoadCSVFile();
        if (string.IsNullOrEmpty(CSVText))
            return;

        List<Dictionary<string, string>> JsonObjList = UtilsClass.ConvertCSVFileToJsonObject(CSVText);
        if (JsonObjList != null)
        {
            serializableSummonStoreData = new SerializableSummonStoreData();
            for (int i = 0; i < JsonObjList.Count; i++)
            {
                Dictionary<string, string> JsonObjDic = JsonObjList[i];

                SummonStoreData summonStoreData = new SummonStoreData();

                summonStoreData.store_id = UtilsClass.ParseToType<int>(JsonObjDic["store_id"]);
                summonStoreData.summon_store_type = UtilsClass.ParseToType<eSummonStoreType>(JsonObjDic["summon_store_type"]);
                summonStoreData.start_level = UtilsClass.ParseToType<int>(JsonObjDic["start_level"]);
                summonStoreData.max_level = UtilsClass.ParseToType<int>(JsonObjDic["max_level"]);

                summonStoreData.textcode_name = UtilsClass.ParseToType<string>(JsonObjDic["textcode_name"]);
                summonStoreData.textcode_explain = UtilsClass.ParseToType<string>(JsonObjDic["textcode_explain"]);
                summonStoreData.icon_name = UtilsClass.ParseToType<string>(JsonObjDic["icon_name"]);

                string[] summon_item_counts = (UtilsClass.ParseToType<string>(JsonObjDic["summon_item_counts"])).Split('\\');
                summonStoreData.summon_item_counts = new double[summon_item_counts.Length];
                for (int k = 0; k < summon_item_counts.Length; k++)
                {
                    summonStoreData.summon_item_counts[k] = UtilsClass.ParseToType<double>(summon_item_counts[k]);
                }
                string[] price_item_ids = (UtilsClass.ParseToType<string>(JsonObjDic["price_item_ids"])).Split('\\');
                summonStoreData.price_item_ids = new int[price_item_ids.Length];
                for (int k = 0; k < price_item_ids.Length; k++)
                {
                    summonStoreData.price_item_ids[k] = UtilsClass.ParseToType<int>(price_item_ids[k]);
                }
                string[] price_item_counts = (UtilsClass.ParseToType<string>(JsonObjDic["price_item_counts"])).Split('\\');
                summonStoreData.price_item_counts = new double[price_item_counts.Length];
                for (int k = 0; k < price_item_counts.Length; k++)
                {
                    summonStoreData.price_item_counts[k] = UtilsClass.ParseToType<double>(price_item_counts[k]);
                }
                serializableSummonStoreData.datas.Add(summonStoreData);
            }
            SaveGameData(serializableSummonStoreData);
        }
    }
    private void ConvertSummonStorePerLevelDataToJson()
    {
        string CSVText = LoadCSVFile();
        if (string.IsNullOrEmpty(CSVText))
            return;

        List<Dictionary<string, string>> JsonObjList = UtilsClass.ConvertCSVFileToJsonObject(CSVText);
        if (JsonObjList != null)
        {
            serializableSummonStorePerLevelData = new SerializableSummonStorePerLevelData();
            for (int i = 0; i < JsonObjList.Count; i++)
            {
                Dictionary<string, string> JsonObjDic = JsonObjList[i];

                SummonStorePerLevelData  levelData = new SummonStorePerLevelData();

                levelData.target_level = UtilsClass.ParseToType<int>(JsonObjDic["target_level"]);
                levelData.exp_min = UtilsClass.ParseToType<double>(JsonObjDic["exp_min"]);
                levelData.exp_max = UtilsClass.ParseToType<double>(JsonObjDic["exp_max"]);

                string[] rarity_probabilities = (UtilsClass.ParseToType<string>(JsonObjDic["rarity_probabilities"])).Split('\\');
                levelData.rarity_probabilities = new List<float>();
                for (int k = 0; k < rarity_probabilities.Length; k++)
                {
                    levelData.rarity_probabilities.Add(UtilsClass.ParseToType<float>(rarity_probabilities[k]));
                }

                string[] rank_probabilities = (UtilsClass.ParseToType<string>(JsonObjDic["rank_probabilities"])).Split('\\');
                levelData.rank_probabilities = new List<float>();
                for (int k = 0; k < rank_probabilities.Length; k++)
                {
                    levelData.rank_probabilities.Add(UtilsClass.ParseToType<float>(rank_probabilities[k]));
                }

                serializableSummonStorePerLevelData.datas.Add(levelData);
            }
            SaveGameData(serializableSummonStorePerLevelData);
        }
    }

    private void ConvertInAppDataToJson()
    {
        string CSVText = LoadCSVFile();
        if (string.IsNullOrEmpty(CSVText))
            return;

        List<Dictionary<string, string>> JsonObjList = UtilsClass.ConvertCSVFileToJsonObject(CSVText);
        if (JsonObjList != null)
        {
            serializableInAppData = new SerializableInAppData();
            for (int i = 0; i < JsonObjList.Count; i++)
            {
                Dictionary<string, string> JsonObjDic = JsonObjList[i];

                InAppData inAppData = new InAppData();

                inAppData.target_store_id = UtilsClass.ParseToType<int>(JsonObjDic["target_store_id"]);
                inAppData.textcode_name = UtilsClass.ParseToType<string>(JsonObjDic["textcode_name"]);
                inAppData.textcode_explain = UtilsClass.ParseToType<string>(JsonObjDic["textcode_explain"]);
                inAppData.icon_name = UtilsClass.ParseToType<string>(JsonObjDic["icon_name"]);

                // inAppData.product_type = UtilsClass.ParseToType<UnityEngine.Purchasing.ProductType>(JsonObjDic["product_type"]);
                inAppData.price = UtilsClass.ParseToType<double>(JsonObjDic["price"]);

                string[] target_item_ids = (UtilsClass.ParseToType<string>(JsonObjDic["target_item_ids"])).Split('\\');
                inAppData.target_item_ids = new int[target_item_ids.Length];
                for (int k = 0; k < target_item_ids.Length; k++)
                {
                    inAppData.target_item_ids[k] = UtilsClass.ParseToType<int>(target_item_ids[k]);
                }
                string[] target_item_counts = (UtilsClass.ParseToType<string>(JsonObjDic["target_item_counts"])).Split('\\');
                inAppData.target_item_counts = new double[target_item_ids.Length];
                for (int k = 0; k < target_item_ids.Length; k++)
                {
                    inAppData.target_item_counts[k] = UtilsClass.ParseToType<double>(target_item_counts[k]);
                }

                inAppData.inapp_product_id = UtilsClass.ParseToType<string>(JsonObjDic["inapp_product_id"]);
                inAppData.inapp_google_store_id = UtilsClass.ParseToType<string>(JsonObjDic["inapp_google_store_id"]);
                inAppData.inapp_ios_store_id = UtilsClass.ParseToType<string>(JsonObjDic["inapp_ios_store_id"]);
                serializableInAppData.datas.Add(inAppData);
            }
            SaveGameData(serializableInAppData);
        }
    }
    private void ConvertLocalizeDataToJson()
    {
        string CSVText = LoadCSVFile();
        if (string.IsNullOrEmpty(CSVText))
            return;

        List<Dictionary<string, string>> JsonObjList = UtilsClass.ConvertCSVFileToJsonObject(CSVText);
        if (JsonObjList != null)
        {
            serializableLocalizeData = new SerializableLocalizeData();
            for (int i = 0; i < JsonObjList.Count; i++)
            {
                Dictionary<string, string> JsonObjDic = JsonObjList[i];

                LocalizeData localizeData = new LocalizeData();
                localizeData.key = UtilsClass.ParseToType<string>(JsonObjDic["key"]);
                localizeData.text_kor = UtilsClass.ParseToType<string>(JsonObjDic["text_kor"]);
                localizeData.text_eng = UtilsClass.ParseToType<string>(JsonObjDic["text_eng"]);
                localizeData.text_jpn = UtilsClass.ParseToType<string>(JsonObjDic["text_jpn"]);
                serializableLocalizeData.datas.Add(localizeData);
            }
            SaveGameData(serializableLocalizeData);
        }
    }




    private void SaveGameData(object dest)
    {
        string filePath = UnityEditor.EditorUtility.SaveFilePanel("Save data file", Application.dataPath + "/Resources/Data", "", "json");

        if (!string.IsNullOrEmpty(filePath))
        {
            string dataAsJson = JsonUtility.ToJson(dest, true);
            File.WriteAllText(filePath, dataAsJson);

            UnityEditor.AssetDatabase.Refresh();
        }
    }

    // private void SaveDicGameDataToJson(object dest)
    // {
    //     string filePath = UnityEditor.EditorUtility.SaveFilePanel("Save data file", Application.dataPath + "/Resources/Data", "", "json");
    // 
    //     if (!string.IsNullOrEmpty(filePath))
    //     {
    //         string dataAsJson = JsonConvert.SerializeObject(dest, Formatting.Indented);
    //         File.WriteAllText(filePath, dataAsJson);
    // 
    //         UnityEditor.AssetDatabase.Refresh();
    //     }
    // }
    private string LoadCSVFile()
    {
        string filePath = UnityEditor.EditorUtility.OpenFilePanel("Select data file", Application.dataPath + "/Data/CSV", "csv");

        if (!string.IsNullOrEmpty(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            return dataAsJson;
        }

        return string.Empty;
    }
}

#endif
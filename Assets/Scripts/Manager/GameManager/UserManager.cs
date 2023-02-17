using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public UserInfoData m_UserInfoData = new UserInfoData();
    public UserMapData m_UserMapData = new UserMapData();
    public UserItemData m_UserItemData = new UserItemData();

    public UserStoreData m_UserStoreData = new UserStoreData();

    public UserEnhancementData m_UserEnhancementData = new UserEnhancementData();
    public UserQuestData m_UserQuestData = new UserQuestData();


    List<UserDataBase> m_UserDataList = new List<UserDataBase>();
    
    public bool isClearUserData = false;

    [SerializeField] int m_loadCount = 0;
    public bool m_bIsLoadCompleted = false;

    [SerializeField] float m_fAutoSaveTimeLength = 10 * 60;
    [SerializeField] float m_fCurAutoSaveTime = 0f;

    int m_curSaveCount = 0;
    int m_maxSaveCount = 0;
    public System.Action<bool> m_OnSaveComplete = null;
    public System.Action<bool> m_OnLoadComplete = null;

    bool m_bInitialized = false;
    public void Init()
    {
        if (m_bInitialized)
            return;

        m_UserDataList = new List<UserDataBase>();
        m_UserDataList.Clear();
        m_UserDataList.Add(m_UserInfoData);
        m_UserDataList.Add(m_UserMapData);
        // m_UserDataList.Add(m_UserCharacterData);
        m_UserDataList.Add(m_UserItemData);
        // m_UserDataList.Add(m_UserTownData);
        // m_UserDataList.Add(m_UserQuestData);
        m_UserDataList.Add(m_UserStoreData);
        m_UserDataList.Add(m_UserQuestData);
        // m_UserDataList.Add(m_UserMiniGameData);
        for (int i = 0; i < m_UserDataList.Count; i++)
        {
            m_UserDataList[i].Init();
        }

        m_loadCount = 0;
        m_bIsLoadCompleted = false;

        // try
        // {
        //     LoadAllUserData();
        // }
        // catch (System.Exception e)
        // {
        //     Debug.LogError(e);
        // }

        if (GameManager.Instance.GameType == eGameType.eInGame)
            HandleLoadComplete();

        m_bInitialized = true;
    }


    #region Save & Load
    public void SaveAllUserData()
    {
        if (m_bIsLoadCompleted == false)
            return;

        // m_UserInfoData.LastAccessTimestamp = GameManager.Instance.TimeMgr.GetCurrentTimeStamp();
        for (int i = 0; i < m_UserDataList.Count; i++)
        {
            try
            {
                SaveUserData(m_UserDataList[i]);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        PlayerPrefs.Save();
    }
    public void SaveAllUserDataBySequential()
    {

    }
    public void LoadAllUserData()
    {
        for (int i = 0; i < m_UserDataList.Count; i++)
        {
            LoadUserData(m_UserDataList[i]);
        }
    }

    public void SaveUserData(UserDataBase userDataBase, bool bCheckLoadData = true)
    {
        if (m_bIsLoadCompleted == false && bCheckLoadData)
            return;

        if (userDataBase == null)
            return;

        if (string.IsNullOrEmpty(userDataBase.tableName))
            return;


        string folderKey = "UserData";
        byte[] folderBytes = System.Text.Encoding.UTF8.GetBytes(folderKey);
        string encodedFolderData = System.Convert.ToBase64String(folderBytes);

        string userDataPath = string.Empty;
#if UNITY_EDITOR
        userDataPath = string.Format("{0}/{1}", Application.dataPath, folderKey);
#else
        userDataPath = string.Format("{0}/{1}", Application.persistentDataPath, folderKey);
#endif

        DirectoryInfo di = new DirectoryInfo(userDataPath);

        if (di.Exists == false)
        {
            di.Create();
        }

        string fileName = string.Format("/{0}.json", userDataBase.tableName);
        
        string jsonData = JsonUtility.ToJson(userDataBase, true);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        string encodedJsonData = System.Convert.ToBase64String(bytes);
        File.WriteAllText(userDataPath + fileName, encodedJsonData);

        Debug.Log("Save UserData " + userDataBase.tableName);
    }
    public void LoadUserData(UserDataBase userDataBase)
    {
        if (userDataBase == null)
            return;

        if (string.IsNullOrEmpty(userDataBase.tableName))
            return;

        string folderKey = "UserData";
        byte[] folderBytes = System.Convert.FromBase64String(folderKey);
        string decodedFolderData = System.Text.Encoding.UTF8.GetString(folderBytes);

        string userDataPath;
#if UNITY_EDITOR
        userDataPath = string.Format("{0}/{1}", Application.dataPath, folderKey);
#else
        userDataPath = string.Format("{0}/{1}", Application.persistentDataPath, folderKey);
#endif

        DirectoryInfo di = new DirectoryInfo(userDataPath);

        if (di.Exists == false)
        {
            di.Create();
        }

        string fileName = string.Format("/{0}.json", userDataBase.tableName);
        if (File.Exists(userDataPath + fileName) == false)
        {
            Debug.Log("파일이 없을 경우 생성");
            SaveUserData(userDataBase, false);
        }

        string jsonFromFile = File.ReadAllText(userDataPath + fileName);
        byte[] bytes = System.Convert.FromBase64String(jsonFromFile);
        string decodedJsonData = System.Text.Encoding.UTF8.GetString(bytes);
        LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(decodedJsonData);

        if (userDataBase.Load(jsonData) == true)
        {
            CheckLoadDataCount();
            Debug.Log("Load UserData " + userDataBase.tableName);
        }
    }

    #endregion

    public void CheckLoadDataCount()
    {
        m_loadCount++;
        if (m_loadCount >= m_UserDataList.Count)
        {
            HandleLoadComplete();
        }
    }
    public void HandleLoadComplete()
    {
        if (m_OnLoadComplete != null)
            m_OnLoadComplete(true);
        m_OnLoadComplete = null;

        GameManager.Instance.ItemManager.Setup();
        GameManager.Instance.QuestManager.Setup();

        m_bIsLoadCompleted = true;
    }

    void Update()
    {
        if (m_bIsLoadCompleted == false)
            return;

        float fDeltaTime = Time.deltaTime * Time.timeScale;
        m_fCurAutoSaveTime += fDeltaTime;
        if (m_fCurAutoSaveTime >= m_fAutoSaveTimeLength)
        {
            m_fCurAutoSaveTime = 0f;
            SaveAllUserData();
        }
    }


    public void Release()
    {
        m_bIsLoadCompleted = false;
        m_bInitialized = false;
    }
}

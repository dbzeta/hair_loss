using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public enum eGameType
{
    eTitle,
    eLobby,
    eInGame,
}

public class GameManager : Singleton<GameManager>
{

    public eGameType GameType = eGameType.eTitle;

    bool m_bInitialized = false;
    public bool Initialized { get { return m_bInitialized; } }

    override protected void Awake()
    {
        if (!IsNull)
        {
            Destroy(this.gameObject);
            return;
        }

        base.Awake();

        #region 화면 잠금 설정
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면이 꺼지지 않도록 설정
        // Screen.sleepTimeout = SleepTimeout.SystemSetting; // 디바이스 설정에 맞추어 화면이 꺼지도록 처리
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        // OnDemandRendering.renderFrameInterval = 1; // Unity3 이상
        #endregion

        // System.DateTime now = System.DateTime.Now;
        // Debug.Log("GameManager Awake " + now);

        Init();
    }
    private void Start()
    {
        if (!m_bInitialized)
            Init();

        // System.DateTime now = System.DateTime.Now;
        // Debug.Log("GameManager Start " + now);
    }

    public void Init()
    {
        if (m_bInitialized)
            return;

        Data.Init();
        // IAPManager.Init();
        // AdsManager.Init();
        LanguageMgr.Init();
        ResourcesManager.Init();
        UIManager.Init();
        Sound.Init();
        TimeMgr.Init();

        UserData.Init();
        ItemManager.Init();
        QuestManager.Init();

        m_bInitialized = true;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }

    private void OnApplicationQuit()
    {
        if (UserData.m_bIsLoadCompleted)
        {
            UserData.SaveAllUserData();
        }
    }

    bool m_bIsApplicationPause = false;
    private void OnApplicationPause(bool pause)
    {
        // 플레이 도중 게임 이탈했을 때
        if (pause)
        {
            if (m_bIsApplicationPause)
                return;

            m_bIsApplicationPause = pause;
            // UserData.m_UserInfoData.LastAccessTimestamp = TimeMgr.GetCurrentTimeStamp();
        }
        // 플레이 도중 게임 복귀
        else
        {
            if (!m_bIsApplicationPause)
                return;

            m_bIsApplicationPause = pause;
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        bool pause = !focus;
        // 플레이 도중 게임 이탈했을 때
        if (pause)
        {
            if (m_bIsApplicationPause)
                return;

            m_bIsApplicationPause = pause;
            // UserData.m_UserInfoData.LastAccessTimestamp = TimeMgr.GetCurrentTimeStamp();
        }
        // 플레이 도중 게임 복귀
        else
        {
            if (!m_bIsApplicationPause)
                return;

            m_bIsApplicationPause = pause;
        }
    }




    DataManager m_dataManager = null;
    public DataManager Data
    {
        get
        {
            if (m_dataManager == null)
                m_dataManager = this.transform.Find("DataManager").GetComponent<DataManager>();
            return m_dataManager;
        }
    }
    UserManager m_userManager = null;
    public UserManager UserData
    {
        get
        {
            if (m_userManager == null)
                m_userManager = this.transform.Find("UserManager").GetComponent<UserManager>();
            return m_userManager;
        }
    }

    SoundManager m_soundManager = null;
    public SoundManager Sound
    {
        get
        {
            if (m_soundManager == null)
                m_soundManager = this.transform.Find("SoundManager").GetComponent<SoundManager>();
            return m_soundManager;
        }
    }
    TimeManager m_timeManager = null;
    public TimeManager TimeMgr
    {
        get
        {
            if (m_timeManager == null)
                m_timeManager = this.transform.Find("TimeManager").GetComponent<TimeManager>();
            return m_timeManager;
        }
    }
    LanguageManager m_languageManager = null;
    public LanguageManager LanguageMgr
    {
        get
        {
            if (m_languageManager == null)
                m_languageManager = this.transform.Find("LanguageManager").GetComponent<LanguageManager>();
            return m_languageManager;
        }
    }
    ResourcesManager m_resourcesManager = null;
    public ResourcesManager ResourcesManager
    {
        get
        {
            if (m_resourcesManager == null)
                m_resourcesManager = this.transform.Find("ResourcesManager").GetComponent<ResourcesManager>();
            return m_resourcesManager;
        }
    }
    UIManager m_uiManager = null;
    public UIManager UIManager
    {
        get
        {
            if (m_uiManager == null)
                m_uiManager = this.transform.Find("UIManager").GetComponent<UIManager>();
            return m_uiManager;
        }
    }/*
    IAPManager m_iapManager = null;
    public IAPManager IAPManager
    {
        get
        {
            if (m_iapManager == null)
                m_iapManager = this.transform.Find("IAPManager").GetComponent<IAPManager>();
            return m_iapManager;
        }
    }
    AdsManager m_adsManager = null;
    public AdsManager AdsManager
    {
        get
        {
            if (m_adsManager == null)
                m_adsManager = this.transform.Find("AdsManager").GetComponent<AdsManager>();
            return m_adsManager;
        }
    }*/
    ObjectPoolManager m_objectPoolManager = null;
    public ObjectPoolManager ObjectPoolManager
    {
        get
        {
            if (m_objectPoolManager == null)
                m_objectPoolManager = this.transform.Find("ObjectPoolManager").GetComponent<ObjectPoolManager>();
            return m_objectPoolManager;
        }
    }
    ItemManager m_itemManager = null;
    public ItemManager ItemManager
    {
        get
        {
            if (m_itemManager == null)
                m_itemManager = this.transform.Find("ItemManager").GetComponent<ItemManager>();
            return m_itemManager;
        }
    }
    QuestManager m_questManager = null;
    public QuestManager QuestManager
    {
        get
        {
            if (m_questManager == null)
                m_questManager = this.transform.Find("QuestManager").GetComponent<QuestManager>();
            return m_questManager;
        }
    }


    public string appVersion = string.Empty;
    public void AppVersionCheck(Action<bool, string> _complete)
    {
        StartCoroutine(IEAppVersionCheck(_complete));
    }
    public IEnumerator IEAppVersionCheck(Action<bool, string> _complete)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
    string _AppID = "https://play.google.com/store/apps/details?id=com.dollook.raisewitch.global";
#elif UNITY_EDITOR
        string _AppID = "https://play.google.com/store/apps/details?id=com.dollook.raisewitch.global";
#endif

            UnityWebRequest _WebRequest = UnityWebRequest.Get(_AppID);

        yield return _WebRequest.SendWebRequest();

        // 정규식으로 전채 문자열중 버전 정보가 담겨진 태그를 검색한다.
        string _Pattern = @"<span class=""htlgb"">[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}<";
        Regex _Regex = new Regex(_Pattern, RegexOptions.IgnoreCase);
        Match _Match = _Regex.Match(_WebRequest.downloadHandler.text);

        Debug.Log(_WebRequest.downloadHandler.text);
        appVersion = string.Empty;
        if (_Match != null)
        {
            // 버전 정보가 담겨진 태그를 찾음
            // 해당 태그에서 버전 넘버만 가져온다
            _Match = Regex.Match(_Match.Value, "[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}");


            try
            {
                string[] _ClientVersion = (Application.version).Split('.');
                string[] _AppStoreVersion = (_Match.Value).Split('.');

                Debug.Log("  Application.version : " + Application.version + ", AppStore version :" + _Match.Value);

                if (_AppStoreVersion[0] != _ClientVersion[0] || _AppStoreVersion[1] != _ClientVersion[1] || _AppStoreVersion[2] != _ClientVersion[2])
                {
                    if (_complete != null)
                        _complete(true, _Match.Value);

                    yield break;
                }
            }
            catch (Exception Ex)
            {
                // 비정상 버전정보 파싱중 Exception처리

                Debug.LogError("비정상 버전 정보 Exception : " + Ex);
                Debug.LogError("  Application.version : " + Application.version + ", AppStore version :" + _Match.Value);
            }


        }
        else
        {
            Debug.LogError("Not Found AppStoreVersion Info");
        }

        if (_complete != null)
            _complete(false, "_Match is null" );
    }
}

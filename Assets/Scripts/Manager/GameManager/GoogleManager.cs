using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

public class GoogleManager : MonoBehaviour
{

    public Text logText;
    public InputField scoreInput;


    bool bInitialized = false;

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        Login();
    }

    public void Init()
    {
        if (bInitialized)
            return;
        
        GoogleInitilaize();

        bInitialized = true;
    }
    public void Release()
    {

    }

    public void GoogleInitilaize()
    {
        var config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void Login()
    {
        if (!bInitialized)
            Init();

        Social.localUser.Authenticate((bool success) => 
        {
            if (success)
            {
                logText.text = "���� �α��� ����";
            }
            else
            {
                logText.text = "���� �α��� ����";
            }
        });
    }

    public void LogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        logText.text = "���� �α׾ƿ�";

    }


    public void ShowLeaderboardUI() => Social.ShowLeaderboardUI();
    public void ShowLeaderboardUI_1() => ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard);
    public void AddLeaderboardUI_1() => Social.ReportScore(int.Parse(scoreInput.text), GPGSIds.leaderboard, (bool success)=> { });
}

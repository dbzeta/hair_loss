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
                logText.text = "구글 로그인 성공";
            }
            else
            {
                logText.text = "구글 로그인 실패";
            }
        });
    }

    public void LogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        logText.text = "구글 로그아웃";

    }


    public void ShowLeaderboardUI() => Social.ShowLeaderboardUI();
    public void ShowLeaderboardUI_1() => ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard);
    public void AddLeaderboardUI_1() => Social.ReportScore(int.Parse(scoreInput.text), GPGSIds.leaderboard, (bool success)=> { });
}

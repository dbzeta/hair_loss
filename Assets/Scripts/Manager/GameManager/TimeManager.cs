using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public long serverTimeStamp;
    public long diffTimeStamp;
    public long timeStamp;

    [Header("메일 재갱신 시간")]
    public float m_fMaxMailRefreshTime;
    public float m_fMailRefreshTime;
    [Header("자동 저장 시간")]
    public float m_fMaxAutoSaveTime;
    public float m_fAutoSaveTime;
    [Header("서버 시간 동기화 시간")]
    public float m_fMaxServerTimeSyncTime;
    public float m_fServerTimeSyncTime;

    bool m_bInitialized = false;
    public void Init()
    {
        if (m_bInitialized)
            return;

        m_fMaxMailRefreshTime = (5 * 60);
        m_fMailRefreshTime = 0;
        m_fMaxAutoSaveTime = (5 * 60);
        m_fAutoSaveTime = 0;
        m_fMaxServerTimeSyncTime = (1 * 60);
        m_fServerTimeSyncTime = 0;

        m_bInitialized = true;
    }

    public void SetTimeStampToServerDateTime(DateTime _serverDateTime)
    {
        serverTimeStamp = DateTimeToUnixTimestamp(_serverDateTime);
        long deviceTimeStamp = DateTimeToUnixTimestamp(DateTime.UtcNow);
        diffTimeStamp = deviceTimeStamp - serverTimeStamp;
    }
    public long GetCurrentTimeStamp()
    {
        long deviceTimeStamp = DateTimeToUnixTimestamp(DateTime.UtcNow);
        timeStamp = deviceTimeStamp - diffTimeStamp;
        return timeStamp;
    }
    public DateTime GetDateTime()
    {
        DateTime now = UnixTimestampToDateTime(GetCurrentTimeStamp());
        return now;
    }
    public DateTime UnixTimestampToDateTime(long unixTime)
    {
        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dt = dt.AddSeconds(unixTime);
        return dt;
    }
    public long DateTimeToUnixTimestamp(DateTime dateTime)
    {
        long timeStamp = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        return timeStamp;
    }

    public long GetTimeStampAdd(int day, int hour, int min, int sec)
    {
        DateTime newDateTime;
        DateTime nowUTCDateTime = GetDateTime();

        if (day != 0)
        {
            newDateTime = new DateTime(nowUTCDateTime.Year, nowUTCDateTime.Month, nowUTCDateTime.Day, 0, 0, 0, DateTimeKind.Utc);
            newDateTime = newDateTime.AddDays(day);
        }
        else
            newDateTime = nowUTCDateTime;

        if (hour != 0)
            newDateTime = newDateTime.AddHours(hour);
        if (min != 0)
            newDateTime = newDateTime.AddMinutes(min);
        if (min != 0)
            newDateTime = newDateTime.AddMinutes(sec);

        return DateTimeToUnixTimestamp(newDateTime);

    }
    public long GetTimeStampAddDays(int amount)
    {
        DateTime newDateTime;
        DateTime nowUTCDateTime = GetDateTime();
        newDateTime = new DateTime(nowUTCDateTime.Year, nowUTCDateTime.Month, nowUTCDateTime.Day, 0, 0, 0, DateTimeKind.Utc);
        newDateTime = newDateTime.AddDays(amount);
        return DateTimeToUnixTimestamp(newDateTime);
    }
    public long GetTimeStampAddMin(float amount)
    {
        DateTime newDateTime;
        DateTime nowUTCDateTime = GetDateTime();
        newDateTime = nowUTCDateTime.AddMinutes(amount);
        return DateTimeToUnixTimestamp(newDateTime);
    }
    public long GetTimeStampAddSec(float amount)
    {
        DateTime newDateTime;
        DateTime nowUTCDateTime = GetDateTime();
        newDateTime = nowUTCDateTime.AddSeconds(amount);
        return DateTimeToUnixTimestamp(newDateTime);
    }
    public long GetTimeStampMonthlyResetTime()
    {
        DateTime now = GetDateTime();
        DateTime dt = now.AddDays(1 - now.Day);
        dt = dt.AddMonths(1);
        return DateTimeToUnixTimestamp(dt);
    }
    public long GetTimeStampWeeklyResetTime()
    {
        DateTime now = GetDateTime();
        // now.DayOfWeek
        DateTime dt = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
        dt = dt.AddDays(7);
        return DateTimeToUnixTimestamp(dt);
    }
    public long GetTimeStampDailyResetTime()
    {
        DateTime now = GetDateTime();
        DateTime dt = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
        dt = dt.AddDays(1);
        return DateTimeToUnixTimestamp(dt);
    }
    public long CalcuTimeStamp(long _timestamp, double day, double hour, double min, double sec)
    {
        DateTime time = UnixTimestampToDateTime(_timestamp);
        time.AddDays(day);
        time.AddHours(hour);
        time.AddMinutes(min);
        time.AddSeconds(sec);

        return time.Ticks / System.TimeSpan.TicksPerSecond;
    }


    [ContextMenu("TestTime")]
    public void UnixTimeToLocalAndSetNewTimestamp()
    {
        DateTime newDateTime;

        DateTime nowUTCDateTime = GetDateTime();
        DateTime nowLocalDateTime = nowUTCDateTime.ToLocalTime();
        Debug.Log(nowLocalDateTime);

        newDateTime = new DateTime(nowLocalDateTime.Year, nowLocalDateTime.Month, 1);
        newDateTime = newDateTime.AddMonths(1);
        Debug.Log(newDateTime);

        long resetTimeStamp = DateTimeToUnixTimestamp(newDateTime);
        Debug.Log(resetTimeStamp);

        long remainTime = resetTimeStamp - GetCurrentTimeStamp();
        Debug.Log(remainTime);
        Debug.Log((float)remainTime / (60*24*60));
    }

    public string ConvertTime(float fTime, int iUnit = 2)
    {
        int hour, min, sec;
        if (iUnit == 1)
        {
            sec = (int)fTime;
            return sec.ToString("00");
        }
        else if (iUnit == 2)
        {
            min = (int)(fTime / 60);
            sec = (int)(fTime % 60);
            return min.ToString("00") + ":" + sec.ToString("00");
        }
        else
        {
            hour = (int)((fTime / 3600) % 24);
            min = (int)((fTime / 60) % 60);
            sec = (int)(fTime % 60);
            return hour.ToString("00") + ":" + min.ToString("00") + ":" + sec.ToString("00");
        }
    }


    public long GetRemainTime(long _timestamp)
    {
        return _timestamp - GetCurrentTimeStamp();
    }
}
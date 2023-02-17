using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_LANGUAGE_TYPE
{
    E_English = 10,
    E_Japanese = 22,
    E_Korean = 23,
}
public class LanguageManager : MonoBehaviour
{
    public E_LANGUAGE_TYPE m_Language;
    [HideInInspector] public List<UILanguageText> UILanguages = new List<UILanguageText>();
    public void Init()
    {
        m_Language = E_LANGUAGE_TYPE.E_Korean;//(E_LANGUAGE_TYPE)PlayerPrefs.GetInt("Language", (int)Application.systemLanguage);
    }


    public string GetLocalizeText(string _keyName, params string[] _param)
    {
        string localizeText = string.Empty;
        LocalizeData localizeData = GameManager.Instance.Data.GetLocalizeData(_keyName);
        if (localizeData != null)
        {
            if (m_Language == E_LANGUAGE_TYPE.E_Korean)
                localizeText = localizeData.text_kor;
            else if (m_Language == E_LANGUAGE_TYPE.E_English)
                localizeText = localizeData.text_eng;
            else if (m_Language == E_LANGUAGE_TYPE.E_Japanese)
                localizeText = localizeData.text_jpn;
        }
        if (_param != null && _param.Length > 0)
        {
            string newTxt = string.Empty;
            switch (_param.Length)
            {
                case 1:
                    newTxt = string.Format(localizeText, _param[0]);
                    break;
                case 2:
                    newTxt = string.Format(localizeText, _param[0], _param[1]);
                    break;
                case 3:
                    newTxt = string.Format(localizeText, _param[0], _param[1], _param[2]);
                    break;
                case 4:
                    newTxt = string.Format(localizeText, _param[0], _param[1], _param[2], _param[3]);
                    break;
                case 5:
                    newTxt = string.Format(localizeText, _param[0], _param[1], _param[2], _param[3], _param[4]);
                    break;
                case 6:
                    newTxt = string.Format(localizeText, _param[0], _param[1], _param[2], _param[3], _param[4], _param[5]);
                    break;
                case 7:
                    newTxt = string.Format(localizeText, _param[0], _param[1], _param[2], _param[3], _param[4], _param[5], _param[6]);
                    break;
                case 8:
                    newTxt = string.Format(localizeText, _param[0], _param[1], _param[2], _param[3], _param[4], _param[5], _param[6], _param[7]);
                    break;
                case 9:
                    newTxt = string.Format(localizeText, _param[0], _param[1], _param[2], _param[3], _param[4], _param[5], _param[6], _param[7], _param[8]);
                    break;
                case 10:
                    newTxt = string.Format(localizeText, _param[0], _param[1], _param[2], _param[3], _param[4], _param[5], _param[6], _param[7], _param[8], _param[9]);
                    break;
                default:
                    break;
            }
            localizeText = newTxt;
        }
        return localizeText;
    }

    public void Release()
    {
        foreach (var item in UILanguages)
        {
            if (item == null) UILanguages.Remove(item);
        }
    }
}

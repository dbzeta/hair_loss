using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILanguageText : MonoBehaviour
{
    public string keyName;
    public string[] param;
    // public ELOCALIZE_TEXT_YPE type = ELOCALIZE_TEXT_YPE.LTY_NAME;
    private Text text;
/*
    private void Awake()
    {
        text = this.GetComponent<Text>();
        text.text = string.Empty;
    }
    private void Start()
    {
        GameManager.Instance.LanguageMgr.UILanguages.Add(this);
        SetupText(keyName, param);
    }
    public void SetupText()
    {
        if (text == null)
        {
            Debug.Log(this.name);
        }
        // if (GameManager.Instance.LanguageManager.m_Language == E_LANGUAGE_TYPE.E_Korean) text.font = GameManager.Instance.ResourcesManager.GetFont(E_RESOURCE_FONT.E_ARIAL);
        // else if (GameManager.Instance.LanguageManager.m_Language == E_LANGUAGE_TYPE.E_English) text.font = GameManager.Instance.ResourcesManager.GetFont(E_RESOURCE_FONT.E_ARIAL);
        // else if (GameManager.Instance.LanguageManager.m_Language == E_LANGUAGE_TYPE.E_Japanese) text.font = GameManager.Instance.ResourcesManager.GetFont(E_RESOURCE_FONT.E_ARIAL);

        text.text = GameManager.Instance.LanguageMgr.GetLocalizeText(keyName);
    }
    public void SetupText(string _keyName, params string[] _param)
    {
        keyName = _keyName;
        param = _param;
        SetupText();
        if (param != null && param.Length > 0)
        {
            string newTxt = string.Empty;
            switch (param.Length)
            {
                case 1:
                    newTxt = string.Format(text.text, param[0]);
                    break;
                case 2:
                    newTxt = string.Format(text.text, param[0], param[1]);
                    break;
                case 3:
                    newTxt = string.Format(text.text, param[0], param[1], param[2]);
                    break;
                case 4:
                    newTxt = string.Format(text.text, param[0], param[1], param[2], param[3]);
                    break;
                case 5:
                    newTxt = string.Format(text.text, param[0], param[1], param[2], param[3], param[4]);
                    break;
                case 6:
                    newTxt = string.Format(text.text, param[0], param[1], param[2], param[3], param[4], param[5]);
                    break;
                case 7:
                    newTxt = string.Format(text.text, param[0], param[1], param[2], param[3], param[4], param[5], param[6]);
                    break;
                case 8:
                    newTxt = string.Format(text.text, param[0], param[1], param[2], param[3], param[4], param[5], param[6], param[7]);
                    break;
                case 9:
                    newTxt = string.Format(text.text, param[0], param[1], param[2], param[3], param[4], param[5], param[6], param[7], param[8]);
                    break;
                case 10:
                    newTxt = string.Format(text.text, param[0], param[1], param[2], param[3], param[4], param[5], param[6], param[7], param[8], param[9]);
                    break;
                default:
                    break;
            }
            text.text = newTxt;
        }
    }*/

}

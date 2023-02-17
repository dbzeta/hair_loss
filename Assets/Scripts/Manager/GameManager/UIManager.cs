using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    #region Popup 관리 DW
    // 현재의 고정 캔버스
    UISceneBase _sceneUI;

    [SerializeField] List<UIPopupBase> m_UIPopupBaseList = new List<UIPopupBase>();
    // 열었었던(새로만든) 팝업 리스트
    [SerializeField] List<UIPopupBase> m_InstantiatedPopupList = new List<UIPopupBase>();
    // 현재 열려있는 팝업 리스트
    public List<UIPopupBase> m_CurOpenPopupList = new List<UIPopupBase>();

    public System.Action OnChangedPopups = null;

    int m_SortingOrder = 10;

    bool m_bInitialized = false;

    [ContextMenu("UILoadAssetAtPath")]
    public void UILoadAssetAtPath()
    {
#if UNITY_EDITOR

        SerializableUIPopupBaseData serializableUIPopupBaseData = new SerializableUIPopupBaseData();
        TextAsset textAsset = Resources.Load<TextAsset>("Data/Utills/UIPopupBaseData") as TextAsset;
        serializableUIPopupBaseData = JsonUtility.FromJson<SerializableUIPopupBaseData>(textAsset.text);

        m_UIPopupBaseList.Clear();
        for (int i = 0; i < serializableUIPopupBaseData.datas.Count; i++)
        {
            UIPopupBaseData data = serializableUIPopupBaseData.datas[i];
            string folderPath = data.folder_name;
            string popupName = data.popup_name;

            char ch = folderPath[folderPath.Length - 1];
            if (ch.Equals('/') == false && ch.Equals('\\') == false)
                folderPath += "/";

            GameObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath(folderPath + popupName, typeof(GameObject)) as GameObject;
            if (obj != null)
            {
                m_UIPopupBaseList.Add(obj.GetComponent<UIPopupBase>());
                Debug.Log("UIManager Init");
            }
        }

        // string folderPath = "Assets/99.Dongwook/Prefabs/UIPopup/InGameScene/";
        // string[] objNames = {
        //     "UIInGameArenaPopup.prefab",
        //     "UIInfoPopup.prefab"
        // };

        // for (int i = 0; i < objNames.Length; i++)
        // {
        //     GameObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath(folderPath + objNames[i], typeof(GameObject)) as GameObject;
        //     if (obj != null)
        //     {
        //         m_UIPopupBaseList.Add(obj.GetComponent<UIPopupBase>());
        //         Debug.Log("UIManager Init");
        //     }
        // }
#endif
    }
    public void Init()
    {
        if (m_bInitialized)
            return;

        m_SortingOrder = 10;
        m_bInitialized = true;
    }

    public void AddCurOpenPopupList(UIPopupBase popupBase)
    {
        if (popupBase == null)
            return;

        if (m_CurOpenPopupList.Contains(popupBase) == false)
            m_CurOpenPopupList.Add(popupBase);

        if (OnChangedPopups != null)
            OnChangedPopups();
    }
    public T ShowSceneUI<T>(GameObject obj, string name = null) where T : UISceneBase
    {
        // 이름을 안받았다면 T로
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = obj;
        T sceneUI = UtilsClass.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }
    public T ShowPopupUI<T>(string name = null) where T : UIPopupBase
    {
        // 이름을 안받았다면 T로
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        T popup = GetInstantiatedPopup<T>(name);
        if (popup == null)
        {
            UIPopupBase basePopup = m_UIPopupBaseList.Find(x => x.GetType().Name.Equals(name));
            if (basePopup != null)
            {
                GameObject go = Instantiate(basePopup).gameObject;
                popup = UtilsClass.GetOrAddComponent<T>(go);
                m_InstantiatedPopupList.Add(popup);
            }
        }

        if (popup != null)
        {
            if (m_CurOpenPopupList.Contains(popup) == false)
                m_CurOpenPopupList.Add(popup);

            popup.transform.SetParent(Root.transform);
            popup.transform.localScale = Vector3.one;
            popup.transform.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            popup.transform.GetComponent<RectTransform>().offsetMax = Vector2.zero;

            popup.ShowPopup();

            m_SortingOrder++; // order 늘리기
        }

        return popup as T;
    }

    // 해당 팝업을 직접 지운다
    public void ClosePopupUI(UIPopupBase popup)
    {
        if (popup == null)
            return;

        UIPopupBase uIPopupBase = m_CurOpenPopupList.Find(x => (x == popup));

        // 비어있는 스택이라면 삭제 불가
        if (uIPopupBase == null)
            return;

        // 해당 팝업만 지우기
        // 23/05/16 JH 추가
        if (popup != null)
        {
            popup.gameObject.SetActive(false);

            if (popup.m_PopupType != EPOPUP_TYPE.ALWAYS_OPEN_POPUP)
            {
                if (m_InstantiatedPopupList.Contains(popup))
                {
                    popup.transform.SetParent(this.transform);
                }
            }
            // GameManager.Instance.Resource.Destroy(popup.gameObject);

            m_CurOpenPopupList.Remove(popup);
        }
        popup = null;
        m_SortingOrder--; // order 줄이기

        if (m_SortingOrder <= 10)
            m_SortingOrder = 10;

        if (OnChangedPopups != null)
            OnChangedPopups();


        //int iLastIdx = m_CurOpenPopupList.Count - 1;
        //if (m_CurOpenPopupList[iLastIdx] == null)
        //{
        //    ClosePopupUI();
        //    return;
        //}

        //if (m_CurOpenPopupList[iLastIdx] != popup)
        //{
        //    // 스택의 가장 위에있는 Peek() 것만 삭제할 수 잇기 때문에 popup이 Peek()가 아니면 삭제 못함
        //    Debug.Log("Close Popup Failed!");
        //    return;
        //}

        //ClosePopupUI();
    }
    public void ClosePopupUI()
    {
        if (m_CurOpenPopupList.Count == 0)
            return;

        int iLastIdx = m_CurOpenPopupList.Count - 1;
        UIPopupBase popup = m_CurOpenPopupList[iLastIdx];
        if (popup != null)
        {
            popup.gameObject.SetActive(false);

            if (popup.m_PopupType != EPOPUP_TYPE.ALWAYS_OPEN_POPUP)
            {
                if (m_InstantiatedPopupList.Contains(popup))
                {
                    popup.transform.SetParent(this.transform);
                }
            }
            // GameManager.Instance.Resource.Destroy(popup.gameObject);

            m_CurOpenPopupList.Remove(popup);
        }
        popup = null;
        m_SortingOrder--; // order 줄이기

        if (m_SortingOrder <= 10)
            m_SortingOrder = 10;

        if (OnChangedPopups != null)
            OnChangedPopups();
    }

    [ContextMenu("CloseAllPopupUI")]
    public void CloseAllPopupUI()
    {
        UIPopupBase[] arrOpenedPopup = m_CurOpenPopupList.ToArray();
        for (int i = 0; i < arrOpenedPopup.Length; i++)
        {
            // ClosePopupUI
            // 22.06.02
            arrOpenedPopup[i].ClosePopup();
        }
        m_CurOpenPopupList.Clear();

        m_InstantiatedPopupList = UtilsClass.CheckSurroundings(m_InstantiatedPopupList);

        if (OnChangedPopups != null)
            OnChangedPopups();

        // m_InstantiatedPopupList.RemoveAll(null);
        // if (m_InstantiatedPopupList.Contains(null))
        // {
        // }

        // while (m_OpenPopupStack.Count > 0)
        // {
        //     ClosePopupUI();
        // }
    }
    public void OnClickBackButton()
    {
        // 비어있는 스택이라면 삭제 불가
        if (m_CurOpenPopupList.Count == 0)
            return;

        UIPopupBase popup = m_CurOpenPopupList[m_CurOpenPopupList.Count - 1];
        if (popup == null || popup.CantClosePopup == true)
            return;

        popup.OnClickBackButton();

        if (OnChangedPopups != null)
            OnChangedPopups();
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = UtilsClass.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; // 캔버스 안에 캔버스 중첩 경우 (부모 캔버스가 어떤 값을 가지던 나는 내 오더값을 가지려 할때)

        GraphicRaycaster gr = UtilsClass.GetOrAddComponent<GraphicRaycaster>(go);

        if (sort)
        {
            canvas.sortingOrder = m_SortingOrder;
            // m_SortingOrder++;
        }
        else // soring 요청 X 라는 소리는 팝업이 아닌 일반 고정 UI
        {
            canvas.sortingOrder = 0;
        }
    }

    public int GetOpenPopupCount()
    {
        return m_CurOpenPopupList.Count;
    }

    public T GetInstantiatedPopup<T>(string popupName = null)
    {
        if (string.IsNullOrEmpty(popupName))
            popupName = typeof(T).Name;

        UIPopupBase popup = m_InstantiatedPopupList.Find(x => x.GetType().Name.Equals(popupName));
        if (popup != null)
        {
            return popup.GetComponent<T>();
        }
        return default(T);
    }
    public T GetCurOpenPopup<T>(string popupName = null)
    {
        if (string.IsNullOrEmpty(popupName))
            popupName = typeof(T).Name;

        UIPopupBase popup = m_CurOpenPopupList.Find(x => x.GetType().Name.Equals(popupName));
        if (popup != null)
        {
            return popup.GetComponent<T>();
        }
        return default(T);
    }
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    public void ReleaseCurSceneOpenedPopup()
    {
        CloseAllPopupUI();

        for (int i = 0; i < m_InstantiatedPopupList.Count; i++)
        {
            UIPopupBase uIPopupBase = m_InstantiatedPopupList[i];
            if (uIPopupBase == null) continue;
            GameManager.Instance.ResourcesManager.Destroy(uIPopupBase.gameObject);
        }
        m_InstantiatedPopupList.Clear();
        m_CurOpenPopupList.Clear();
    }

    public void Release()
    {
        OnChangedPopups = null;

        CloseAllPopupUI();

        for (int i = 0; i < m_InstantiatedPopupList.Count; i++)
        {
            UIPopupBase uIPopupBase = m_InstantiatedPopupList[i];
            if (uIPopupBase == null) continue;
            GameManager.Instance.ResourcesManager.Destroy(uIPopupBase.gameObject);
        }
        m_InstantiatedPopupList.Clear();
        m_CurOpenPopupList.Clear();
    }

    #endregion
}

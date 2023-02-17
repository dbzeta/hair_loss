using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public enum E_Resource_Type
{
    E_InGame,
    E_Item,
    E_Icon,
    E_Common_UI,
}

public class ResourcesManager : MonoBehaviour
{
    [SerializeField] SpriteAtlas TestAtlas = null;

    private SpriteAtlas inGameAtlas = null;
    private SpriteAtlas itemAtlas = null;
    private SpriteAtlas iconAtlas = null;
    private SpriteAtlas commonUIAtlas = null;

    public  void Init()
    {
        inGameAtlas = Resources.Load<SpriteAtlas>("Atlas/InGameAtlas");
        itemAtlas = Resources.Load<SpriteAtlas>("Atlas/ItemAtlas");
        iconAtlas = Resources.Load<SpriteAtlas>("Atlas/IconAtlas");
        commonUIAtlas = Resources.Load<SpriteAtlas>("Atlas/CommonUIAtlas");

        m_RarityColors = new Color[(int)eRarityType.eMyth + 1];
        {
            m_RarityColors[(int)eRarityType.eNormal] = new Color32(255, 255, 255, 255);
            m_RarityColors[(int)eRarityType.eRare] = new Color32(18, 242, 21, 255);
            m_RarityColors[(int)eRarityType.eEpic] = new Color32(11, 110, 204, 255);
            m_RarityColors[(int)eRarityType.eUnique] = new Color32(155, 61, 217, 255);
            m_RarityColors[(int)eRarityType.eLegend] = new Color32(226, 125, 19, 255);
            m_RarityColors[(int)eRarityType.eMyth] = new Color32(253, 30, 25, 255);
        }
    }

    public void Release()
    {
        TestAtlas = null;
    }

    public Sprite GetSprite(E_Resource_Type type, string spriteName)
    {
        Sprite sprite = null;
        switch (type)
        {
            case E_Resource_Type.E_InGame:
                sprite = inGameAtlas.GetSprite(spriteName);
                break;
            case E_Resource_Type.E_Item:
                sprite = itemAtlas.GetSprite(spriteName);
                break;
            case E_Resource_Type.E_Icon:
                sprite = iconAtlas.GetSprite(spriteName);
                break;
            case E_Resource_Type.E_Common_UI:
                sprite = commonUIAtlas.GetSprite(spriteName);
                break;
            default:
                break;
        }

        return sprite;
    }


    [SerializeField] Color[] m_RarityColors = new Color[(int)eRarityType.eMyth + 1];
    public Color GetRarityColor(eRarityType eRarityType)
    {
        return m_RarityColors[(int)eRarityType];
    }



    #region 기본 메소드
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Filed to load prefab : {path}");
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }
    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
    #endregion
}

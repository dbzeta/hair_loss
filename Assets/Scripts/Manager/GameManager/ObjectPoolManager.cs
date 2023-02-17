using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] List<PoolingObject> m_OriginPoolingObjects = new List<PoolingObject>();

    Dictionary<string, List<PoolingObject>> dicPoolingObjectLists = new Dictionary<string, List<PoolingObject>>();

    private void Start()
    {
        dicPoolingObjectLists = new Dictionary<string, List<PoolingObject>>();

        for (int i = 0; i < m_OriginPoolingObjects.Count; i++)
        {
            if (m_OriginPoolingObjects[i] != null)
            {
                string keyName = m_OriginPoolingObjects[i].keyName;
                if (string.IsNullOrEmpty(keyName)) continue;

                PoolingObject origin = m_OriginPoolingObjects[i];
                Transform parent = this.transform.Find(keyName);
                if (parent == null)
                {
                    GameObject parentObj = new GameObject(keyName);
                    parent = parentObj.transform;
                    parent.SetParent(this.transform);
                }
                PoolingObject newObj = Instantiate(origin, parent);

                List<PoolingObject> poolingObjects = null;
                if (dicPoolingObjectLists.ContainsKey(keyName))
                    poolingObjects = dicPoolingObjectLists[keyName];
                else
                { 
                    poolingObjects = new List<PoolingObject>();
                    dicPoolingObjectLists.Add(keyName, poolingObjects);
                }

                poolingObjects.Add(newObj);
                newObj.gameObject.SetActive(false);
            }
        }
    }

    public PoolingObject CreatePoolingObject(string _keyName)
    {
        PoolingObject origin = m_OriginPoolingObjects.Find(x=>x.keyName.Equals(_keyName));
        if (origin == null)
            return null;

        Transform parent = this.transform.Find(_keyName);
        if (parent == null)
        {
            GameObject parentObj = new GameObject(_keyName);
            parent = parentObj.transform;
            parent.SetParent(this.transform);
        }
        PoolingObject newObj = Instantiate(origin, parent);

        List<PoolingObject> poolingObjects = null;
        if (dicPoolingObjectLists.ContainsKey(_keyName))
        {
            poolingObjects = dicPoolingObjectLists[_keyName];
            if (poolingObjects == null)
            {
                poolingObjects = new List<PoolingObject>();
                dicPoolingObjectLists[_keyName] = poolingObjects;
            }
        }
        else
        {
            if (poolingObjects == null)
                poolingObjects = new List<PoolingObject>();
            dicPoolingObjectLists.Add(_keyName, poolingObjects);
        }
        poolingObjects.Add(newObj);
        newObj.gameObject.SetActive(false);
        return newObj;
    }
    public T GetObject<T>(string _keyName = null) where T : PoolingObject
    {
        PoolingObject obj = null;

        if (string.IsNullOrEmpty(_keyName))
            _keyName = typeof(T).Name;

        if (dicPoolingObjectLists.ContainsKey(_keyName) == false)
        {
            obj = CreatePoolingObject(_keyName);
        }
        else
        {
            List<PoolingObject> poolingObjects = dicPoolingObjectLists[_keyName];
            for (int i = 0; i < poolingObjects.Count; i++)
            {
                bool bActive = poolingObjects[i].transform.gameObject.activeSelf;
                if (bActive == false)
                {
                    obj = poolingObjects[i];
                    break;
                }
            }

            if (obj == null)
                obj = CreatePoolingObject(_keyName);
        }

        if (obj == null)
            return null;

        obj.gameObject.SetActive(true);

        return obj as T;
    }
    public void ReturnOnject(PoolingObject poolingObject)
    {
        if (poolingObject == null)
            return;

        poolingObject.gameObject.SetActive(false);

        Transform parent = this.transform.Find(poolingObject.keyName);
        poolingObject.transform.SetParent(parent);
    }
}

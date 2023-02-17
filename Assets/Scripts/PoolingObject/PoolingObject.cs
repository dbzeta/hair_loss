using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    public string keyName = string.Empty;

    protected bool m_bInitialized = false;
    virtual public void Init()
    {
        if (m_bInitialized)
            return;


        m_bInitialized = true;
    }
}

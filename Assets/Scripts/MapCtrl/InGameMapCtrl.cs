using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMapCtrl : MonoBehaviour
{

    private Renderer renderer;
    private Material myMaterial;

    [SerializeField] float scrollSpeed;

    private Vector2 savedOffset;
    bool m_bStartLoop = false;

    bool m_bInitialized = false;
    public void Init()
    {
        if (m_bInitialized)
            return;

        if (renderer == null) renderer = this.GetComponent<Renderer>();
        myMaterial = renderer.material;

        m_bInitialized = true;
    }
    public void StartLoop(float _fScrollSpeed)
    {
        if (!m_bInitialized)
            Init();

        scrollSpeed = _fScrollSpeed;
        m_bStartLoop = true;
    }
    void Update()
    {
        if (m_bStartLoop == false)
            return;
        
        if (myMaterial == null)
            return;

        float x = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(x, 0);
        myMaterial.mainTextureOffset = offset;
    }
}

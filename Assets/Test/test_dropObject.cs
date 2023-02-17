using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_dropObject : MonoBehaviour
{
    [SerializeField] float m_fMoveSpeed = 200f;

    public System.Action<test_dropObject, Collider2D> m_Release = null;

    bool m_bInitialized = false;
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        if (m_bInitialized)
            return;

        ChildColliderCtrl childColliderCtrl = this.transform.GetComponent<ChildColliderCtrl>();
        if (childColliderCtrl != null)
            childColliderCtrl.m_OnTriggerEnter2D += this.HandleTriggerEnter2D;

        m_bInitialized = true;
    }
    private void HandleTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D == null)
            return;

        if (collider2D.tag.Equals("Player"))
        {
            Release(collider2D);
        }
        else if (collider2D.tag.Equals("Ground"))
        {
            Release(collider2D);
        }
    }
    public void Release(Collider2D collider2D)
    {
        if (m_Release != null)
            m_Release(this, collider2D);
        m_Release = null;

        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            this.transform.position += Vector3.down * Time.deltaTime * Time.timeScale * m_fMoveSpeed;
        }
    }
}

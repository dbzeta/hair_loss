using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : MonoBehaviour
{
    [SerializeField] Canvas m_MainCanvas = null;
    [SerializeField] RectTransform m_InGamePanel = null;

    [SerializeField] RectTransform m_Character = null;
    

    [SerializeField] Button m_LeftButton = null;
    [SerializeField] Button m_RightButton = null;

    [SerializeField] float m_fSpeed = 100.0f;

    [SerializeField] float m_fMaxSpeed = 250f;
    [SerializeField] float m_fMoveSpeed = 100.0f;


    // Start is called before the first frame update
    void Start()
    {
        if (m_DropObjects != null)
        {
            m_dropObjs.Clear();
            for (int i = 0; i < m_DropObjects.childCount; i++)
            {
                Transform child = m_DropObjects.GetChild(i);
                test_dropObject obj = child.GetComponent<test_dropObject>();
                obj.Init();
                obj.gameObject.SetActive(false);
                m_dropObjs.Add(obj);
            }
        }

        fDropTimeLength = 0.5f;

        m_iScore = 0;
        if (m_ScoreText != null)
            m_ScoreText.text = string.Format("Score : {0}", m_iScore);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("GetKeyDown Left");
            OnPointerDown_Left();
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Debug.Log("GetKeyUp Left");
            OnPointerUp_L();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("GetKeyDown Right");
            OnPointerDown_Right();
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Debug.Log("GetKeyUp Right");
            OnPointerUp_R();
        }

        if (bPointerDownL)
        {
            float deltaTime = Time.deltaTime * Time.timeScale;
            m_fAccSpeed += -m_fSpeed * deltaTime;

            if (m_fAccSpeed <= -m_fMaxSpeed)
                m_fAccSpeed = -m_fMaxSpeed;
        }
        else if (bPointerDownR)
        {
            float deltaTime = Time.deltaTime * Time.timeScale;
            m_fAccSpeed += m_fSpeed * deltaTime;

            if (m_fAccSpeed >= m_fMaxSpeed)
                m_fAccSpeed = m_fMaxSpeed;
        }

        Move();

        fCurDropTime += Time.deltaTime * Time.timeScale;
        if (fCurDropTime > fDropTimeLength)
        {
            fCurDropTime = 0f;
            fDropTimeLength = UnityEngine.Random.Range(0.2f, 1.0f);
            DropObjects();
        }
    }

    private void Move()
    {
        if (m_InGamePanel == null || m_Character == null)
            return;

        if (bPointerDownL == false && bPointerDownR == false)
            return;

        float panel_width = m_InGamePanel.rect.width;

        float minX = -(panel_width / 2.0f);
        float maxX = (panel_width / 2.0f);

        float character_width = m_Character.rect.width;

        float min_posX = minX + (character_width / 2.0f);
        float max_posX = maxX - (character_width / 2.0f);

        Vector3 character_pos = m_Character.transform.localPosition;
        float delta_time = (Time.deltaTime * Time.timeScale);

        float speed = bPointerDownL ? -m_fMoveSpeed + m_fAccSpeed : m_fMoveSpeed + m_fAccSpeed;
        character_pos += (Vector3.right * speed * delta_time);

        float character_posX = character_pos.x;
        character_posX = Mathf.Clamp(character_posX, min_posX, max_posX);
        character_pos.x = character_posX;

        m_Character.transform.localPosition = character_pos;
    }

    [SerializeField] float m_fAccSpeed = 0f;
    [SerializeField] bool bPointerDownL = false;
    [SerializeField] bool bPointerDownR = false;
    public void OnPointerDown_Left()
    {
        bPointerDownL = true;
        bPointerDownR = false;

        m_fAccSpeed /= 2f;
    }
    public void OnPointerDown_Right()
    {
        bPointerDownL = false;
        bPointerDownR = true;

        m_fAccSpeed /= 2f;
    }
    public void OnPointerUp_L()
    {
        if (bPointerDownL) bPointerDownL = false;

        if (bPointerDownL == false && bPointerDownR == false)
            m_fAccSpeed = 0f;
    }
    public void OnPointerUp_R()
    {
        if (bPointerDownR) bPointerDownR = false;

        if (bPointerDownL == false && bPointerDownR == false)
            m_fAccSpeed = 0f;
    }

    [SerializeField] Transform m_DropObjects = null;
    List<test_dropObject> m_dropObjs = new List<test_dropObject>();


    float fCurDropTime = 0f;
    float fDropTimeLength = 0.5f;
    public void DropObjects()
    {
        int randCont = UnityEngine.Random.Range(1, 3);

        List<test_dropObject> activeObjects = new List<test_dropObject>();
        for (int i = 0; i < m_dropObjs.Count; i++)
        {
            if (m_dropObjs[i].gameObject.activeSelf)
                continue;

            test_dropObject test_DropObject = m_dropObjs[i];
            activeObjects.Add(test_DropObject);

            if (activeObjects.Count >= randCont)
                break;
        }

        int start_idx = activeObjects.Count;
        int end_idx = randCont;
        for (int i = start_idx; i < end_idx; i++)
        {
            test_dropObject newObj = Instantiate(m_dropObjs[0]);
            newObj.gameObject.SetActive(false);
            m_dropObjs.Add(newObj);
            activeObjects.Add(newObj);
        }

        float width = m_InGamePanel.rect.width;
        float posY = (m_InGamePanel.rect.height / 2f) - 50;
        for (int i = 0; i < activeObjects.Count; i++)
        {
            float posX = UnityEngine.Random.Range(-(width / 2f) + 50f, (width / 2f) - 50f);
            Vector3 spawn_pos = new Vector3(posX, posY, 0);
            activeObjects[i].transform.localPosition = spawn_pos;
            activeObjects[i].m_Release += this.HandleDropObjectRelease;
            activeObjects[i].gameObject.SetActive(true);
        }
    }

    [SerializeField] int m_iScore = 0;
    [SerializeField] Text m_ScoreText = null;

    private void HandleDropObjectRelease(test_dropObject test_DropObject, Collider2D collider2D)
    {
        if (test_DropObject == null)
            return;

        if (collider2D == null)
            return;

        if (collider2D.tag.Equals("Player"))
        {
            Time.timeScale = 0f;
        }
        else if (collider2D.tag.Equals("Ground"))
        {
            m_iScore += 100;
            if (m_ScoreText != null)
                m_ScoreText.text = string.Format("Score : {0}", m_iScore);
        }
    }
    private void HandleTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D == null)
            return;

        if (collider2D.tag.Equals("Enemy"))
        {

        }
        else if (collider2D.tag.Equals("Player"))
        { 

        }
        else if (collider2D.tag.Equals("Ground"))
        { 

        }
    }
}

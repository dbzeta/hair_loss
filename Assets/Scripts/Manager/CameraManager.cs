using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECAMERA_RENDER_TYPE
{
    CRT_RECT_BOX,
    CRT_FULL_SCREEN,
}
public class CameraManager : MonoBehaviour , IManager
{
    [SerializeField] ECAMERA_RENDER_TYPE m_CameraRenderType = ECAMERA_RENDER_TYPE.CRT_FULL_SCREEN;

    [SerializeField] private float m_fCurrentCamShakePower = 0.0f;
    [SerializeField] private float CameraShakeLength = 0.25f;
    [SerializeField] private float CameraShakePower = 0.15f;
    [SerializeField] private float CameraShakeBombPower = 1.0f;
    [SerializeField] private Vector3 defaultPostion = new Vector3(0, 0, -50);
    
    private float prevSize = 10.0f;
    private Vector3 prevPosition = new Vector3(0, 0, -10);

    private float delayTime;
    private float time;
    private Vector3 fromPosition;
    private Vector3 toPosition;
    private float fromSize;
    private float toSize;

    public Vector3 offSet;
    public GameObject Target;

    public void ManagerInit()
    {

    }

    public void ManagerRelease()
    {

    }

    public void ManagerReset()
    {

    }

    private void OnPreCull() => GL.Clear(true, true, Color.black);

    void Start()
    {
        Init();
    }

    public void Init2()
    {
        float targetAspect = 1080.0f / 1920.0f;

        Camera[] cameras = GameObject.FindObjectsOfType<Camera>();

        foreach (var cam in cameras)
        {

            float currentAspect = (float)Screen.width / (float)Screen.height;
            float rectHeight = currentAspect / targetAspect;
            Rect cameraRect = cam.rect;
            cameraRect.height = rectHeight;
            cameraRect.y = (1.0f - rectHeight) / 2.0f;
            cam.rect = cameraRect;
        }
    }
    public void Init()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;


        float targetaspect = 9f / 16f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;
        float scalewidth = 1.0f / scaleheight;

        if (m_CameraRenderType == ECAMERA_RENDER_TYPE.CRT_RECT_BOX)
        {
            if (scaleheight < 1.0f)
            {
                rect.height = scaleheight;
                rect.y = (1.0f - scaleheight) / 2f;
            }
            else
            {
                rect.width = scalewidth;
                rect.x = (1f - scalewidth) / 2f;
            }
        }
        camera.rect = rect;


       
        // {
        //     float deffalutAspect = 16.0f / 9.0f;
        //     float curSize = targetaspect;
        //     float _OrthographicSize = camera.orthographicSize;
        //     float size = (deffalutAspect / curSize) * _OrthographicSize;
        // 
        //     if (deffalutAspect > curSize)
        //         //    size = (curSize / deffalutAspect) * _OrthographicSize;
        //         size = _OrthographicSize;
        //     //Debug.Log(size);
        //     camera.orthographicSize = size;
        // 
        // 
        //     if (!camera.orthographic)
        //     {
        //         float defaultAspect = 16.0f / 9.0f;
        //         float _fov = camera.fieldOfView;
        //         float fov = (defaultAspect / curSize) * _fov;
        //         if (deffalutAspect > curSize) fov = _fov;
        //         //camera.fieldOfView = fov;
        //     }
        // }
    }

    public float GetAspectRatio(int aScreenWidth, int aScreenHeight)
    {
        float r = (float)aScreenWidth / (float)aScreenHeight;
        string _r = r.ToString("F2");
        string ratio = _r.Substring(0, 4);
        Debug.Log(r);
        switch (ratio)
        {
            case "2.37":
            case "2.39":
                Debug.Log("21:9");
                return r;
            case "1.25":
                Debug.Log("5:4");
                return r;
            case "1.33":
                Debug.Log("4:3");
                return r;
            case "1.50":
                Debug.Log("3:2");
                return r;
            case "1.60":
            case "1.56":
                Debug.Log("16:10");
                return r;
            case "1.67":
            case "1.78":
            case "1.77":
                Debug.Log("16:9");
                return r;
            case "0.67":
                Debug.Log("2:3");
                return r;
            case "0.56":
                Debug.Log("9:16");
                return r;
            case "2.22":
                Debug.Log("20:9");
                return r;
            case "2.11":
                Debug.Log("19:9");
                return r;
            case "2.00":
                Debug.Log("18:9");
                return r;
            default:
                Debug.Log("UnValue");
                return r;

        }
    }

    private void LateUpdate()
    {
        FollowCamera();
    }

    public void FollowCamera()
    {
        if (Target == null) return;
        Camera camera = this.GetComponent<Camera>();
        Vector3 targetPos = new Vector3(Target.transform.position.x, Target.transform.position.y , camera.transform.position.z);
        targetPos -= offSet;
        Vector3 cameraPos = camera.transform.position;
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetPos, Time.deltaTime * 2f);
        // (Target.transform.position.x, Target.transform.position.y + 1, cameraPos.z);
    }
}

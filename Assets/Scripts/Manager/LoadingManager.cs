using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static string SceneName = "InGameScene";

    [SerializeField] Image m_GaugeBarImage = null;
    [SerializeField] TMP_Text m_GaugeText = null;

    private void Start()
    {
        m_GaugeText.text = "0%";
        StartCoroutine(IELoadScene());
    }
    public static void LoadScene(string _sceneName)
    {
        //EffectManager.Instance.DeleteAllParticle();
        SceneName = _sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator IELoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneName);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        float delayTime = 0.8f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                m_GaugeBarImage.fillAmount = Mathf.Lerp(m_GaugeBarImage.fillAmount, op.progress, timer);
                float percent = m_GaugeBarImage.fillAmount * 100f;
                // Debug.Log(percent);
                m_GaugeText.text = string.Format("{0}%", percent.ToString("0.##"));
                if (m_GaugeBarImage.fillAmount >= op.progress)
                {  
                    // timer = 0f;
                }
            }
            else
            {
                m_GaugeBarImage.fillAmount = Mathf.Lerp(m_GaugeBarImage.fillAmount, 1f, timer);
                float percent = m_GaugeBarImage.fillAmount * 100f;
                // Debug.Log(percent);
                m_GaugeText.text = string.Format("{0}%", percent.ToString("0.##"));

                if (m_GaugeBarImage.fillAmount == 1.0f)
                {
                    if (timer > delayTime)
                    {
                        m_GaugeText.text = "100%";
                        op.allowSceneActivation = true;
                        break;
                    }
                }
            }
        }

        Scene curScene = SceneManager.GetSceneByName(SceneName);
        while (!curScene.isLoaded)
        {
            yield return null;
        }

        Scene prevScene = SceneManager.GetSceneByName("LoadingScene");
        if (prevScene != null)
        {
            SceneManager.UnloadSceneAsync(prevScene);
        }

        yield return null;
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    private static int _nextScene;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(int sceneName)
    {
        _nextScene = sceneName;
        SceneManager.LoadScene(1);
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(_nextScene);
        op.allowSceneActivation = false;
        
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                _slider.value = op.progress;
            }
            else
            {
                timer += Time.deltaTime;
                _slider.value = Mathf.Lerp(0.9f, 1, timer);
                if (_slider.value >= 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

public enum TransitionsEffect
{
    none = -1,
    fade,
    circleWap
}
public class sceneManager : MonoBehaviour
{
    public static sceneManager Instance;
    public UnityEvent<string> OnSceneChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
    }

    //public void ChangeNextSceen(TransitionsEffect transitionsEffect = TransitionsEffect.none) => StartCoroutine(SceneChangeTransitions(SceneManager.GetActiveScene().buildIndex + 1, transitionsEffect));

    //public void ChangeNextSceenAndTransitionsEffect(string num) => StartCoroutine(SceneChangeTransitions(SceneManager.GetActiveScene().buildIndex + 1, (TransitionsEffect)num));

    public void ReloadSceen() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void ChangeSceen(string name, TransitionsEffect transitionsEffect = TransitionsEffect.none) => StartCoroutine(SceneChangeTransitions(name, transitionsEffect));

    IEnumerator SceneChangeTransitions(string name, TransitionsEffect transitionsEffect)
    {
   
        OnSceneChanged?.Invoke(name);

        if ((int)transitionsEffect == -1)
        {
            SceneManager.LoadScene(name);
            yield return null;
        }

        GameObject chidObj = transform.GetChild((int)transitionsEffect).gameObject;
        chidObj.gameObject.SetActive(true);
        Animator ani = chidObj.GetComponent<Animator>();


        ani.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name, LoadSceneMode.Single);
        yield return new WaitForSeconds(1f);
    }
}

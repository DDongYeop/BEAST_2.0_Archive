using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(UIDocument))]
public class MainUiManager : MonoBehaviour
{
    private UIDocument m_MainMenuDocument;
    public UIDocument MainMenuDocument => m_MainMenuDocument;

    [SerializeField] private SerializableDict<string, MenuScreen> m_Screens;
    private MenuScreen m_CurScreen;

    private void Awake()
    {
        m_MainMenuDocument = GetComponent<UIDocument>();
    }

    public void SetScreen(string name)
    {
        MenuScreen NewScreen;

        if (string.IsNullOrEmpty(name) || m_MainMenuDocument == null)
        {
            Debug.LogWarning($"Screen {name} does not exist");
            return;
        }

        NewScreen = m_Screens.GetValue(name);
        if (NewScreen == null)
        {
            Debug.LogWarning($"Screen {name} does not exist");
            return;
        }


        if (m_CurScreen != null)
        {
            m_CurScreen.HideScreenRoutine(NewScreen.ShowScreenRoutine);
        }
        else
        {
            NewScreen.ShowScreenRoutine();
        }

        m_CurScreen = NewScreen;
    }

    public VisualElement GetVisualElement(string name)
    {
        if (string.IsNullOrEmpty(name) || m_MainMenuDocument == null)
            return null;

        return m_Screens.GetValue(name).ScreenElement;
        //return GetScreenCode(name).GetComponent<UIDocument>().rootVisualElement;
    }

    //public MenuScreen GetScreenCode(string name)
    //{
    //    if (string.IsNullOrEmpty(name) || m_MainMenuDocument == null)
    //        return null;

    //    return m_Screens.GetValue(name);
    //   //return transform.Find(name).GetComponent<MenuScreen>();
    //}
}

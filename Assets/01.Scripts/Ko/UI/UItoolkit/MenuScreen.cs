using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public abstract class MenuScreen : MonoBehaviour
{
    [Tooltip("this panel name")]
    [SerializeField] protected string m_ScreenName;

    [SerializeField] protected MainUiManager m_MainUI;
    [SerializeField] protected UIDocument m_Document;

    public VisualElement ScreenElement { protected set; get; }
    public VisualElement RootElement { protected set; get; }

    public event Action OnScreenStarted;
    public event Action OnScreenEnd;

    protected virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(m_ScreenName))
            m_ScreenName = this.GetType().Name;
    }

    protected virtual void Awake()
    {
        if (m_MainUI == null)
            m_MainUI = GetComponent<MainUiManager>();

        if (m_Document == null)
            m_Document = GetComponent<UIDocument>();

        if (m_Document == null && m_MainUI != null)
            m_Document = m_MainUI.MainMenuDocument;

        if (m_Document == null)
        {
            Debug.LogError($"MenuScreen  {m_ScreenName} : missing UIDocument. Check Script Execution Order.");
            return;
        }  

        SetVisualElements();
        RegisterButtonCallbacks();
    }

    protected virtual void SetVisualElements()
    {
        if (m_Document != null)
            RootElement = m_Document.rootVisualElement;

        ScreenElement = GetVisualElement(m_ScreenName);

        if (ScreenElement == null)
            Debug.LogError($"{m_ScreenName} is missing");
    }

    protected virtual void RegisterButtonCallbacks()
    {
        
    }

    public bool IsVisible()
    {
        if (ScreenElement == null)
            return false;

        return (ScreenElement.style.display == DisplayStyle.Flex);
    }

    public static void ShowVisualElement(VisualElement visualElement, bool state)
    {
        if (visualElement == null)
            return;

        visualElement.style.display = state ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public VisualElement GetVisualElement(string elementName)
    {
        if (string.IsNullOrEmpty(elementName) || RootElement == null)
            return null;

        return RootElement.Q(elementName);
    }

    #region ShowScreen

    public delegate void OnShowEventEnd();
    private OnShowEventEnd EventShowEnd;

    /// <summary>
    /// show the screen without motion.
    /// </summary>
    public virtual void ShowScreen()
    {
        if (IsVisible())
        {
            Debug.LogWarning($"{m_ScreenName} is already visible!");
            return;
        }

        OnShowScreen();
    }

    /// <summary>
    /// show the screen with motion. ***When the motion ends, the event must be executed unconditionally.***
    /// </summary>
    /// <param name="OnEventEnd"></param>
    public virtual void ShowScreenRoutine(OnShowEventEnd callback)
    {
        if (!IsVisible())
        {
            Debug.LogWarning($"{m_ScreenName} is already visible!");
            return;
        }
        EventShowEnd = callback;
    }

    public virtual void ShowScreenRoutine()
    {
        if (!IsVisible())
        {
            Debug.LogWarning($"{m_ScreenName} is already visible!");
            return;
        }
    }

    /// <summary>
    /// Runs when the screen is activated.
    /// </summary>
    public virtual void OnShowScreen()
    {
        ShowVisualElement(ScreenElement, true);
        OnScreenStarted?.Invoke();
        EventShowEnd?.Invoke();
        EventShowEnd = null;
    }

    #endregion


    #region HideScreen

    public delegate void OnHideEventEnd();
    private OnHideEventEnd EventHideEnd;

    /// <summary>
    /// Hides the screen with motion. ***When the motion ends, the event must be executed unconditionally.***
    /// </summary>
    /// <param name="OnEventEnd"></param>
    public virtual void HideScreenRoutine(OnHideEventEnd callback)
    {
        if (!IsVisible())
        {
            Debug.LogWarning($"{m_ScreenName} is already invisible!");
            return;
        }
        EventHideEnd = callback;
    }

    public virtual void HideScreenRoutine()
    {
        if (!IsVisible())
        {
            Debug.LogWarning($"{m_ScreenName} is already invisible!");
            return;
        }
        //EventHideEnd += OnHideScreenRoutineEnd;
    }

    /// <summary>
    /// Hide the Screen without motion.
    /// </summary>
    public virtual void HideScreen()
    {
        if (!IsVisible())
        {
            Debug.LogWarning($"{m_ScreenName} is already invisible!");
            return;
        }

        OnHideScreenRoutineEnd();
    }

    /// <summary>
    /// Runs when the screen is deactivated.
    /// </summary>
    public virtual void OnHideScreenRoutineEnd()
    {
        OnScreenEnd?.Invoke();
        ShowVisualElement(ScreenElement, false);
        EventHideEnd?.Invoke();
        EventHideEnd = null;
    }

    #endregion

}


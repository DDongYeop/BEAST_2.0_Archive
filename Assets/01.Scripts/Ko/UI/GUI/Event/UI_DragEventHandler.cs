using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DragEventHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private ScrollRect m_ScrollRect;
    public bool Enable = true;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(Enable)
            m_ScrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Enable)
            m_ScrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Enable)
            m_ScrollRect.OnEndDrag(eventData);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerDownHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;
    public Action<PointerEventData> OnClickDownHandler = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Managers.UI.SceneTail.CurrentStatus == UI_Tail.MakeStatus.Reinforcing)
            return;

        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Managers.UI.SceneTail.CurrentStatus == UI_Tail.MakeStatus.Reinforcing)
            return;

        if (OnClickDownHandler != null)
            OnClickDownHandler.Invoke(eventData);
    }
}

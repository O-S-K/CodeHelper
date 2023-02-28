using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PressHandler : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	[Serializable]
	public class ButtonPressEvent : UnityEvent
	{
	}

	public ButtonPressEvent OnPress = new ButtonPressEvent();

	public void OnPointerDown(PointerEventData eventData)
	{
		OnPress.Invoke();
	}
}

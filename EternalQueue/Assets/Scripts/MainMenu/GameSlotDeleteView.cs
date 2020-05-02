using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GameSlotDeleteView : MonoBehaviour, IPointerClickHandler
{
	public Action OnClickDelete;

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClickDelete?.Invoke();
	}
	
}

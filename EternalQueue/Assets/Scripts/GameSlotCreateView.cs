using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSlotCreateView : MonoBehaviour, IPointerClickHandler
{
	public System.Action OnGameSlotClickCreate;

	public void OnPointerClick(PointerEventData eventData)
	{
		OnGameSlotClickCreate?.Invoke();
	}
	
}

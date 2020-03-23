using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameSlotCreateView : MonoBehaviour, IPointerClickHandler
{
	public System.Action<string> OnGameSlotClickCreate;

	public InputField SlotName;

	private void Start()
	{
		SlotName.text = "Gay Game for Gaymers";
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnGameSlotClickCreate?.Invoke(SlotName.text);
	}
	
}

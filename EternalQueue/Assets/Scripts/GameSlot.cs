using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameSlot : MonoBehaviour, IPointerClickHandler
{
	[HideInInspector]
	public GameData gameData = null;

	public Button gameSlotDeleteView;
	public Text FileNameText;

	public Action<GameData> onDataClickSelect;
	public Action<GameData> onDataClickDelete;

	public void OnPointerClick(PointerEventData eventData)
	{
		if(gameData != null)
		{
			onDataClickSelect?.Invoke(gameData);
		}
	}

	public void OnDataClickDelete()
	{
		onDataClickDelete?.Invoke(gameData);
	}

	private void Start()
	{
		gameSlotDeleteView.onClick.AddListener(OnDataClickDelete);
		FileNameText.text = gameData?.fileName;
	}
}
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameSlot : MonoBehaviour, IPointerClickHandler
{
	[HideInInspector]
	public GameData gameData = null;

	public Button gameSlotDeleteView;
	public Text fileNameText;

	public Action<GameData> onDataClickSelect;
	public Action<GameData> onDataClickDelete;


	public void OnPointerClick(PointerEventData eventData)
	{
		if(gameData != null)
		{
			Debug.Log("Select game slot");
			onDataClickSelect?.Invoke(gameData);
		}
	}

	public void OnDataClickDelete()
	{
		Debug.Log("OnDataClickDelete");
		onDataClickDelete?.Invoke(gameData);
	}

	private void Start()
	{
		gameSlotDeleteView.onClick.AddListener(OnDataClickDelete);
		fileNameText.text = gameData?.fileName;
	}
}
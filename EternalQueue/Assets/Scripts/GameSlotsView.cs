using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//идея в том, чтобы инстансить этот класс и добавлять в view монобехи, GameSlot
public class GameSlotsView : MonoBehaviour
{
	//родитель для слотов
	public RectTransform content;

	//слот
	public GameSlot gameSlotView;
	//слот создания слота
	public GameSlotCreateView gameSlotCreateView;
	//кнопка возврата
	public Button returnButton;

	public Action<GameData> OnDataClickSelect;
	public Action<GameData> OnDataClickDelete;
	public Action<string> OnDataClickCreate;

	private void Start()
	{
		returnButton.onClick.AddListener(onReturnClick);
	}

	public void onReturnClick()
	{
		Destroy(this);
	}

	public void OnDataDelete(GameData gameData)
	{
		OnDataClickDelete?.Invoke(gameData);
	}

	public void OnDataSelect(GameData gameData)
	{
		OnDataClickSelect?.Invoke(gameData);
	}

	public void OnDataCreate(string name)
	{
		
	}

	public void CreateGameDataSlot(GameData gameData)
	{
		GameSlot gameslot = Instantiate(gameSlotView, content);
		gameslot.gameData = gameData;
		gameslot.onDataClickDelete += OnDataDelete;
		gameslot.onDataClickSelect += OnDataSelect;
	}

	public void LoadGameSlots(string[] files)
	{
		foreach (var str in files)
		{
			GameData gameData = GameData.Load(str);
			GameSlot localGameSlot = Instantiate(gameSlotView, content.transform);
			localGameSlot.gameData = gameData;
		}

		GameSlotCreateView localgameSlotCreateView = Instantiate(gameSlotCreateView, content.transform);
		localgameSlotCreateView.OnGameSlotClickCreate += OnDataCreate;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

//идея в том, чтобы инстансить этот класс и добавлять в view монобехи, GameSlot
public class GameSlotsView : MonoBehaviour
{
	//родитель для слотов
	public RectTransform content;

	//вьюха для слота
	public GameSlot gameSlotView;
	public GameSlotCreateView gameSlotCreateView;

	public Action<GameData> OnDataClickSelect;
	public Action<GameData> OnDataClickDelete;
	public Action<string> OnDataClickCreate;

	public void OnDataDelete(GameData gameData)
	{
		OnDataClickDelete?.Invoke(gameData);
	}

	public void OnDataSelect(GameData gameData)
	{
		OnDataClickSelect?.Invoke(gameData);
	}

	public void onDataCreate()
	{
		onDataCreate();
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
	}

	
}

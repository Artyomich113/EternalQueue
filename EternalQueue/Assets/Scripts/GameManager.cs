using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
	GameData gameData;

	public GameSlotsView gameSlots;
	public Canvas MainCanvas;

	void Start()
    {
		SpawnGameSlots();
		
	}
	

	public void OnDataLoaded(GameData gameData)
	{
		this.gameData = gameData;
		Debug.Log("gamedata loaded " + gameData.fileName);
	}

	public void OnDataDeleted(GameData gameData)
	{
		if (this.gameData.Equals(gameData))
		{
			GameData.DeleteDataFile(gameData.fileName);
			gameData = null;
		}


		Debug.Log("gamedata deleted " + gameData.fileName);
	}

	public void SpawnGameSlots()
	{
		GameSlotsView gameSlotsView = Instantiate(gameSlots, MainCanvas.transform);
		gameSlotsView.OnDataClickSelect += OnDataLoaded;
		gameSlotsView.OnDataClickDelete += OnDataDeleted;

		string[] files = Directory.GetFiles(Application.persistentDataPath + "Assets/Data/GameSlots/");
		gameSlotsView.LoadGameSlots(files);
		
	}



}

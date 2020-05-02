using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

	public Image backGrowndImage;
	public MainMenu mainMenu;
	public GameSlotsView gameSlots;
	public Canvas mainCanvas;

	MainMenu mainMenuInstance;
	GameSlotsView GameSlotsViewInstace;

	void Start()
	{
		SpawnGameSlots();
		SpawnMainMenu();
		mainMenuInstance?.gameObject.SetActive(false);
	}

	public void StartGame() //вызов запуск игры
	{
		mainMenuInstance?.gameObject.SetActive(false);
		backGrowndImage?.gameObject.SetActive(false);
	}

	#region Data managment

	public void OnDataLoaded(GameData gameData)
	{
		GameLogic.gameData = gameData;
		Debug.Log("gamedata loaded " + gameData.fileName);
	}

	public void OnDataDeleted(GameData gameData)
	{
		if (GameLogic.gameData != null)
		{
			if (GameLogic.gameData.Equals(gameData))
			{
				GameLogic.gameData = null;
			}
		}
		Debug.Log("gamedata deleted " + gameData.fileName);
		GameData.DeleteDataFile(gameData.fileName);
	}

	public void OnDataCreated(string name)
	{
		Debug.Log("GameData created");
		GameLogic.gameData = new GameData(name);

		GameData.Save(GameLogic.gameData);
	}

	public void SpawnGameSlots() // инициализация загрузки сохранений
	{
		GameSlotsViewInstace = Instantiate(gameSlots, mainCanvas.transform);
		GameSlotsViewInstace.OnDataClickSelect += OnDataLoaded;
		GameSlotsViewInstace.OnDataClickDelete += OnDataDeleted;
		GameSlotsViewInstace.OnDataClickCreate += OnDataCreated;
		GameSlotsViewInstace.returnButton.onClick.AddListener(MainMenu);

		string[] files = Directory.GetFiles(Application.persistentDataPath + "/gameSlots");
		foreach (var file in files)
		{
			Debug.Log("file " + file);
		}
		GameSlotsViewInstace.LoadGameSlots(files);
	}

	public void SpawnMainMenu() // инициализация главного меню
	{
		mainMenuInstance = Instantiate(mainMenu, mainCanvas.transform);
		mainMenuInstance.playGame.onClick.AddListener(StartGame);
		mainMenuInstance.loadGame.onClick.AddListener(LoadGame);
	}

	public void LoadGame() // вызов загрузки сохранений
	{
		GameSlotsViewInstace.gameObject.SetActive(true);
		mainMenuInstance.gameObject.SetActive(false);
	}

	public void MainMenu() // вызов главного меню
	{
		mainMenuInstance?.gameObject.SetActive(true);
		backGrowndImage?.gameObject.SetActive(true);
	}

	#endregion

}

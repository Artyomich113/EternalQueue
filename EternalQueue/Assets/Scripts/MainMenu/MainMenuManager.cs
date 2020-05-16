using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UIScripts;

public class MainMenuManager : MonoBehaviour
{
	GameLogic gameLogic;

	public Image backGrowndImage;
	public MainMenu mainMenu;
	public GameSlotsView gameSlots;
	public Canvas mainCanvas;

	MainMenu mainMenuInstance;
	GameSlotsView GameSlotsViewInstace;

	public Slider mistrustSlider;
	readonly string format = "{0:0.#}";
	public Text mistrustText;

	public Gold gold;

	public Timer timer;

	public RectTransform inGameUI;

	public WindowPainController windowPainController;

	void Start()
	{
		SpawnGameSlots();
		SpawnMainMenu();
		mainMenuInstance?.gameObject.SetActive(false);

		gameLogic = new GameLogic()
		{
			timer = timer,
		};
		timer.onTimeOut += gameLogic.OnTimeUp;
	}

	public void StartGame() //вызов запуск игры
	{
		mainMenuInstance?.gameObject.SetActive(false);
		backGrowndImage?.gameObject.SetActive(false);

		inGameUI.gameObject.SetActive(true);
		
	}

	public void OnUpdateMisstrust(float val)
	{
		mistrustSlider.value = val;
		mistrustText.text = string.Format(format, val) + '%';
	}

	public void OnUpdateGold(int val)
	{
		gold.text.text = val.ToString();
	}

	#region Data managment

	public void OnDataLoaded(GameData gameData)
	{
		GameLogic.gameData = gameData;
		Debug.Log("gamedata loaded " + gameData.fileName);

		GameSlotsViewInstace.gameObject.SetActive(false);
		mainMenuInstance.gameObject.SetActive(true);
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

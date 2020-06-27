using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UIScripts;
using System;

public class GameManager : MonoBehaviour
{
    GameLogic gameLogic;

    public UIManager uIManager;

    MainMenu mainMenuInstance;
    GameSlotsView GameSlotsViewInstace;

    public CameraWorldCordConverter converter;

    public GameItems gameItems;

    void Start()
    {
        uIManager.entitySprites = new Sprite[gameItems.items.Length];
        for (int i = 0; i < gameItems.items.Length; i++)
        {
            Texture2D tex = gameItems.items[i].texture;
            uIManager.entitySprites[i] = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
        }

        SpawnGameSlots();
        SpawnMainMenu();
        mainMenuInstance?.gameObject.SetActive(false);

        gameLogic = new GameLogic()
        {
            timer = uIManager.timer,
            gameManager = this,
        };



        uIManager.timer.onTimeOut += gameLogic.OnTimeUp;
        uIManager.submit.onClick.AddListener(gameLogic.PassTheCar);

        uIManager.nextDay.onClick.AddListener(NextDayHandler);
        void NextDayHandler()
        {
            uIManager.nextDay.gameObject.SetActive(false);
        };

        uIManager.nextDay.onClick.AddListener(gameLogic.DailyUpdate);

        uIManager.home.button.onClick.AddListener(gameLogic.HomeIncrement);
        uIManager.food.button.onClick.AddListener(gameLogic.FoodIncrement);
        uIManager.family.button.onClick.AddListener(gameLogic.FamilyIncrement);
    }

    public void ChangeGuy()
    {

    }

    public void StartGame() //вызов запуск игры
    {
        mainMenuInstance?.gameObject.SetActive(false);
        //uIManager.backGrowndImage?.gameObject.SetActive(false);

        uIManager.inGameUI.gameObject.SetActive(true);

        gameLogic.Init();
        gameLogic.DayStart();

    }

    //public void OnUpdateMisstrust(float val)
    //{
    //    uIManager.mistrustSlider.value = val;
    //    uIManager.mistrustText.text = string.Format(uIManager.format, val) + '%';
    //}

    public void OnUpdateGold(int val)
    {
        uIManager.gold.text.text = val.ToString();
    }

    #region Data managment

    public void OnDataLoaded(GameData gameData)
    {
        GameLogic.gameData = gameData;
        Debug.Log("gamedata loaded: " + gameData.fileName);

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
        GameSlotsViewInstace = Instantiate(uIManager.gameSlots, uIManager.mainCanvas.transform);

        GameSlotsViewInstace.OnDataClickSelect += OnDataLoaded;
        GameSlotsViewInstace.OnDataClickDelete += OnDataDeleted;
        GameSlotsViewInstace.OnDataClickCreate += OnDataCreated;
        GameSlotsViewInstace.returnButton.onClick.AddListener(MainMenu);

        string[] files = null;

        if (Directory.Exists(Application.persistentDataPath + "/gameSlots"))
            files = Directory.GetFiles(Application.persistentDataPath + "/gameSlots");
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/gameSlots");
        }

        GameSlotsViewInstace.LoadGameSlots(files);
    }

    public void SpawnMainMenu() // инициализация главного меню
    {
        mainMenuInstance = Instantiate(uIManager.mainMenu, uIManager.mainCanvas.transform);

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
        //uIManager.backGrowndImage?.gameObject.SetActive(true);
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UIScripts;

//using System;
//using Newtonsoft.Json;
//using System.Net;
//using System.Net.Http;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//public class GooglePlusAccessToken
//{
//    public string access_token { get; set; }
//    public string token_type { get; set; }
//    public int expires_in { get; set; }
//    public string id_token { get; set; }
//    public string refresh_token { get; set; }
//}

public class GameManager : MonoBehaviour
{
    GameLogic gameLogic;

    public UIManager uIManager;

    MainMenu mainMenuInstance;
    GameSlotsView GameSlotsViewInstace;

    public CameraWorldCordConverter converter;

    public GameItems gameItems;

    //protected string googleplus_client_id = "458878619548-khuatamj3qpiccnsm4q6dbulf13jumva.apps.googleusercontent.com";    // Replace this with your Client ID
    //protected string googleplus_client_secret = "4hiVJYlomswRd_PV5lyNQlfN";                                                // Replace this with your Client Secret
    //protected string googleplus_redirect_url = "http://localhost:2443/Index.aspx";                                         // Replace this with your Redirect URL; Your Redirect URL from your developer.google application should match this URL.
    //protected string Parameters;

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

        gameLogic.Bind();
        //uIManager.losingContainer.button.onClick.AddListener(gameLogic.OnNewGame);

        //uIManager.timer.onTimeOut += gameLogic.OnTimeUp;
        //uIManager.submit.onClick.AddListener(gameLogic.PassTheCar);

        //uIManager.nextDayButton.onClick.AddListener(NextDayHandler);
        //void NextDayHandler()
        //{
        //    uIManager.nextDayButton.gameObject.SetActive(false);
        //};

        //uIManager.nextDayButton.onClick.AddListener(gameLogic.DailyUpdate);

        //uIManager.home.button.onClick.AddListener(gameLogic.HomeIncrement);
        //uIManager.food.button.onClick.AddListener(gameLogic.FoodIncrement);
        //uIManager.family.button.onClick.AddListener(gameLogic.FamilyIncrement);

        //gameLogic.showText += uIManager.floatingText.ShowText;
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
        uIManager.goldText.text = val.ToString();
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

        GameSlotsViewInstace.CreateGameDataSlot(GameLogic.gameData);

        GameSlotsViewInstace.gameObject.SetActive(false);
        mainMenuInstance.gameObject.SetActive(true);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Artyomich;

public class GameLogic
{

    [Flags]
    public enum LoseCondition
    {
        food = 0,
        family = 1,
        home = 2,
        mistrust = 4,
    }

    public GameManager gameManager;

    public static GameData gameData;

    public Timer timer;

    public List<ItemData> bannedItems = new List<ItemData>();

    Box passBox;
    Box confiscationBox;
    Box bribeBox;

    Dictionary<string, ItemData> itemDataByName = new Dictionary<string, ItemData>();

    /// <summary>
    /// day ended succesfully without losing
    /// </summary>
    Action onDayEnd;

    Action<string> onLose;

    Action onNewGame;

    Action updateUI;

    /// <summary>
    /// Value, Color, position
    /// </summary>
    public Action<float, Color, Vector3> showText;


    /// <summary>
    /// value beetwen 0 and 1
    /// </summary>

    int day = 1;
    float gold = 500f;
    float mistrust = 0f;
    float hidenMistrust = 0f;

    int homeCap = 1;
    int foodCap = 1;
    int familyCap = 1;

    GameSettingsData gameSettingsData;
    
    bool isPlaying;


    public GameLogic(GameManager gameManager, GameSettings gameSettings)
    {
        this.gameManager = gameManager;
        this.gameSettingsData = new GameSettingsData(gameSettings.data);
    }
    public void CheckItems(List<Entity> bribeItems, List<Entity> passedItems, List<Entity> confiscatedItems)
    {
        bool perfectRunCheck = true;
        string message = "bribe items: ";
        foreach (var ob in bribeItems)
        {
            message += ", " + ob.item.name;
            if (bannedItems.Any(p => p.name == ob.item.name))
            {
                gold += gameSettingsData.bribeGold;

                MistrustIncrement(gameSettingsData.bannedItemInbrabeBoxMistrust);
                
                showText?.Invoke(gameSettingsData.bannedItemInbrabeBoxMistrust * 100, Color.red.Add(-0.2f), ob.gameObject.transform.position);
            }
            else
            {
                MistrustIncrement(gameSettingsData.normalItemInBrabeBoxMistrust);
                
                showText?.Invoke(gameSettingsData.bannedItemInbrabeBoxMistrust * 100, Color.red.Add(-0.2f), ob.gameObject.transform.position);
                message += " +" + gameSettingsData.bannedItemInbrabeBoxMistrust;
            }
        }
        if (bribeItems.Count > 0)
            Debug.Log(message);

        message = "confiscated items: ";
        foreach (var ob in confiscatedItems)
        {
            message += ", " + ob.item.name;
            if (bannedItems.Any(p => p.name == ob.item.name))
            {
                MistrustIncrement(gameSettingsData.bannedItemInconfiscationBoxMistrust);

                showText?.Invoke(-gameSettingsData.bannedItemInconfiscationBoxMistrust * 100, Color.green, ob.gameObject.transform.position);

                message += " -" + gameSettingsData.bannedItemInconfiscationBoxMistrust;
            }
            else
            {
                MistrustIncrement(gameSettingsData.normalItemInBrabeBoxMistrust);

                mistrust += gameSettingsData.bannedItemInbrabeBoxMistrust;
                showText?.Invoke(gameSettingsData.bannedItemInbrabeBoxMistrust * 100, Color.red, ob.gameObject.transform.position);

                message += " +" + gameSettingsData.bannedItemInbrabeBoxMistrust;
            }
        }
        if (confiscatedItems.Count > 0)
            Debug.Log(message);

        message = "passed items: ";
        foreach (var ob in passedItems)
        {
            message += ", " + ob.item.name;
            if (bannedItems.Any(p => p.name == ob.item.name))
            {
                MistrustIncrement(gameSettingsData.bannedItemInPassBoxMistrust);
                showText.Invoke(gameSettingsData.bannedItemInconfiscationBoxMistrust * 100, Color.red, ob.gameObject.transform.position);

                message += " +" + gameSettingsData.bannedItemInconfiscationBoxMistrust;
            }
            else
            {
                MistrustIncrement(gameSettingsData.normalItemInPassBoxMistrust);
            }

        }
        if (passedItems.Count > 0)
            Debug.Log(message);

        Debug.Log("mistrust = " + mistrust + " hidenMistrust = " + hidenMistrust);

        if (perfectRunCheck)
        {
            gold += gameSettingsData.perfectRunGold;
        }

        GoldUpdate();
        MistrustUpdate();
    }

    void MistrustIncrement(float value)
    {
        mistrust += value;
        if (mistrust < 0f)
            mistrust = 0f;
    }
    public void OnNewGame()
    {
        homeCap = gameSettingsData.maxCap;
        foodCap = gameSettingsData.maxCap;
        familyCap = gameSettingsData.maxCap;

        gold = gameSettingsData.startGold;
        mistrust = gameSettingsData.startMistrust;
        hidenMistrust = 0;
        day = 1;

        passBox.Clear();
        bribeBox.Clear();
        confiscationBox.Clear();

        DayStart();
        updateUI?.Invoke();
    }

    public void Bind()
    {
        UIManager uIManager = gameManager.uIManager;

        uIManager.losingContainer.button.onClick.AddListener(OnNewGame);
        onLose += uIManager.losingContainer.Appear;


        uIManager.timer.onTimeOut += OnTimeUp;
        uIManager.submit.onClick.AddListener(PassTheCar);


        uIManager.home.button.onClick.AddListener(HomeIncrement);
        uIManager.food.button.onClick.AddListener(FoodIncrement);
        uIManager.family.button.onClick.AddListener(FamilyIncrement);

        showText += uIManager.floatingText.ShowText;

        uIManager.nextDayButton.onClick.AddListener(DayStart);//

        uIManager.nextDayButton.onClick.AddListener(HideNextDayButton);

        //onDayEnd += DayStart;
        void HideNextDayButton()
        {
            uIManager.nextDayButton.gameObject.SetActive(false);
        }

        void ShowNextDayButton()
        {
            uIManager.nextDayButton.gameObject.SetActive(true);
        }

        onDayEnd += ShowNextDayButton;//DailyUpdate;

        onLose += OnLose;

        updateUI += GoldUpdate;
        updateUI += MistrustUpdate;
        updateUI += DayUpdate;
        updateUI += IconsUpdate;
    }

    public void PassTheCar()
    {
        if (isPlaying)
        {
            CheckItems(bribeBox.items, passBox.items, confiscationBox.items);
            CarArrived();

        }
    }

    /// <summary>
    /// sets items, starts timer
    /// </summary>
    public void DayStart()
    {
        bannedItems.RemoveRange(0, bannedItems.Count);
        int[] itemIndexes = gameManager.gameItems.items.GetUniqueIndexes(UnityEngine.Random.Range(2, 5));
        foreach (var i in itemIndexes)
        {
            bannedItems.Add(gameManager.gameItems.items[i]);
        }
        Debug.Log(itemIndexes.Print());

        string bannedItemsNames = "";
        for (int i = 0; i < itemIndexes.Length; i++)
        {
            bannedItemsNames += "|" + gameManager.gameItems.items[itemIndexes[i]].name + "| ,";
        }
        Debug.Log(bannedItemsNames);

        gameManager.uIManager.dropDownView.itemDatas = bannedItems;
        gameManager.uIManager.dropDownView.GenerateListView(gameManager.uIManager);
        CarArrived();

        timer.SetTimer(gameSettingsData.timePerDay);
        timer.StartTimer();

        isPlaying = true;
    }

    public void DayEnd()
    {
        Debug.Log("DayEnd");

        isPlaying = false;

        foodCap--;
        homeCap--;
        familyCap--;

        if (foodCap <= 0)
        {
            onLose.Invoke("your family got hungry and abandoned you. Then you have lost your mind and joined spetial police forces");
        }
        else if (homeCap <= 0)
        {
            onLose.Invoke("your family got no money to pay home rent, so they moved to parents, and abandon you.");
        }
        else if (familyCap <= 0)
        {
            onLose.Invoke("your family got baraly enought money to enjoy themself, so you lost them");
        }
        else if (mistrust + hidenMistrust > 1f)
        {
            onLose?.Invoke("you got arrested for violating law on carriage of goods");
        }
        else
            onDayEnd?.Invoke();

        IconsUpdate();
    }
    /// <summary>
    /// called when day itteration happened
    /// </summary>
    public void DailyUpdate()
    {
        day++;
        DayUpdate();

        mistrust -= gameSettingsData.daylyMistrustDecrease;

        DayStart();
    }

    public void CarArrived()
    {
        int itemsCount = UnityEngine.Random.Range(4, 8);
        int[] itemIds = gameManager.gameItems.items.GetUniqueIndexes(itemsCount);

        passBox.Clear();
        confiscationBox.Clear();
        bribeBox.Clear();

        for (int i = 0; i < itemsCount; i++)
        {
            Item item = new Item
            {
                name = gameManager.gameItems.items[itemIds[i]].name,
                weight = UnityEngine.Random.Range(1f, 10f)
            };

            Entity entity = PoolManager.instanse.GetPool("entity").Get() as Entity;
            entity.item = item;

            passBox.ItemPlaced(entity);

            entity.transform.position = gameManager.converter.RandomPositionInSquare(passBox.transform, -0.01f);
            if (itemDataByName.ContainsKey(item.name))
            {
                entity.SetTexture(itemDataByName[item.name].texture);
            }
            else
            {
                entity.SetTexture(new Texture2D(128, 128));
            }
        }
    }

    public void DayUpdate()
    {
        gameManager.uIManager.dayCounterText.text = "Day " + day;
    }
    public void IconsUpdate()
    {
        gameManager.uIManager.food.sliderHandler.SetEndValue((float)foodCap / gameSettingsData.maxCap);
        gameManager.uIManager.family.sliderHandler.SetEndValue((float)familyCap / gameSettingsData.maxCap);
        gameManager.uIManager.home.sliderHandler.SetEndValue((float)homeCap / gameSettingsData.maxCap);

        gameManager.uIManager.food.buttonText.text = gameSettingsData.foodCost.ToString();
        gameManager.uIManager.family.buttonText.text = gameSettingsData.familyCost.ToString();
        gameManager.uIManager.home.buttonText.text = gameSettingsData.homeCost.ToString();
    }

    public void FoodIncrement()
    {
        if (foodCap < gameSettingsData.maxCap && gold >= gameSettingsData.foodCost)
        {
            foodCap++;
            gold -= gameSettingsData.foodCost;
        }
        gameManager.uIManager.food.sliderHandler.SetEndValue((float)foodCap / gameSettingsData.maxCap);
        
        GoldUpdate();
    }
    public void FamilyIncrement()
    {
        if (familyCap < gameSettingsData.maxCap && gold >= gameSettingsData.familyCost)
        {

            gold -= gameSettingsData.familyCost;
            familyCap++;
        }
        gameManager.uIManager.family.sliderHandler.SetEndValue((float)familyCap / gameSettingsData.maxCap);
        //Debug.Log("FamilyIncrement");
        GoldUpdate();

    }
    public void HomeIncrement()
    {
        if (homeCap < gameSettingsData.maxCap && gold >= gameSettingsData.homeCost)
        {

            homeCap++;
            gold -= gameSettingsData.homeCost;
        }
        gameManager.uIManager.home.sliderHandler.SetEndValue((float)homeCap / gameSettingsData.maxCap);
        //Debug.Log("HomeIncrement");
        GoldUpdate();

    }

    public void MistrustUpdate()
    {
        //Debug.Log("misstrust update");
        gameManager.uIManager.misstrust.SetValue(mistrust, hidenMistrust);
    }

    public void GoldUpdate()
    {
        gameManager.uIManager.goldText.text = gold.ToString() + "G";
    }

    public void Init()
    {
        foreach (var ob in gameManager.gameItems.items)
        {
            itemDataByName.Add(ob.name, ob);
        }

        onNewGame += OnNewGame;

        //Vector3 root = new Vector3(0, 0, 0);

        Objectpool boxesPool = PoolManager.instanse.GetPool("box");

        passBox = boxesPool.Get() as Box;
        passBox.transform.localScale = gameManager.converter.DefineScale(new Vector3(0.4f, 0.4f, 1f));
        passBox.transform.position = gameManager.converter.DefinePosition(new Vector3(0.25f, 0.25f, 0f));
        passBox.SetColor(Color.white * 0.8f);

        confiscationBox = boxesPool.Get() as Box;
        confiscationBox.transform.position = gameManager.converter.DefinePosition(new Vector3(0.75f, 0.55f, 0f));
        confiscationBox.SetColor(Color.green * 0.8f);
        confiscationBox.transform.localScale = gameManager.converter.DefineScale(new Vector3(0.2f, 0.2f, 1f));

        bribeBox = boxesPool.Get() as Box;
        bribeBox.transform.position = gameManager.converter.DefinePosition(new Vector3(0.75f, 0.25f, 0f));
        bribeBox.SetColor(Color.red);
        bribeBox.transform.localScale = gameManager.converter.DefineScale(new Vector3(0.2f, 0.2f, 1f));

        gameManager.uIManager.windowPainController.MoveToWindow();
        gameManager.uIManager.misstrust.SetValue(mistrust, hidenMistrust);

        //IconsUpdate();
        //MistrustUpdate();
        //GoldUpdate();
        //DayUpdate();

        updateUI?.Invoke();
    }
    /// <summary>
    /// timer times up
    /// </summary>
    public void OnTimeUp()
    {
        DayEnd();
        Debug.Log("Time up");
    }

    public void OnLose(string message)
    {
        Debug.Log("lost with message " + message);
    }
}

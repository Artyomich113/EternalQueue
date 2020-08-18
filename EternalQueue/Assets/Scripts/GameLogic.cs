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


    Action onLose;

    Action onNewGame;

    /// <summary>
    /// Value, Color, position
    /// </summary>
    public Action<float, Color, Vector3> showText;


    /// <summary>
    /// value beetwen 0 and 1
    /// </summary>

    int day = 0;
    float mistrust = 0f;

    float hidenMistrust = 0f;

    float gold = 500f;

    const int homeCost = 350;
    const int foodCost = 350;
    const int familyCost = 300;

    int homeCap = maxCap;
    int foodCap = maxCap;
    int familyCap = maxCap;
    const int maxCap = 3;




    const int brabeGold = 200;
    const int perfectRunGold = 100;


    const float brabeMistrust = 0.2f;
    const float firstMistakeMistrust = 0.1f;
    const float mistakeMistrust = 0.05f;
    const float daylyMistrustDecrease = 0.2f;

    const float confiscationMistrust = 0.02f;

    bool isPlaying;

    const float timePerDay = 60;

    public void CheckItems(List<Entity> bribeItems, List<Entity> passedItems, List<Entity> confiscatedItems)
    {
        bool perfectRunCheck = true;
        string message = "bribe items: ";
        foreach (var ob in bribeItems)
        {
            message += ", " + ob.item.name;
            if (bannedItems.Any(p => p.name == ob.item.name))
            {
                gold += brabeGold;

                hidenMistrust += brabeMistrust;
                showText?.Invoke(brabeMistrust * 100, Color.red, ob.gameObject.transform.position);
            }
            else
            {
                mistrust += brabeMistrust;
                showText?.Invoke(brabeMistrust * 100, Color.red, ob.gameObject.transform.position);

                message += " +" + brabeMistrust;
            }
        }
        Debug.Log(message);

        message = "confiscated items: ";
        foreach (var ob in confiscatedItems)
        {
            message += ", " + ob.item.name;
            if (bannedItems.Any(p => p.name == ob.item.name))
            {
                mistrust = Mathf.Clamp(mistrust - confiscationMistrust, 0f, 1.0f);
                showText?.Invoke(-confiscationMistrust * 100, Color.green, ob.gameObject.transform.position);

                message += " -" + confiscationMistrust;
            }
            else
            {
                mistrust += brabeMistrust;
                showText?.Invoke(brabeMistrust * 100, Color.red, ob.gameObject.transform.position);

                message += " +" + brabeMistrust;
            }
        }
        Debug.Log(message);

        message = "passed items: ";
        foreach (var ob in passedItems)
        {
            message += ", " + ob.item.name;
            if (bannedItems.Any(p => p.name == ob.item.name))
            {
                mistrust = Mathf.Clamp(mistrust + confiscationMistrust, 0f, 1.0f);
                showText.Invoke(confiscationMistrust * 100, Color.red, ob.gameObject.transform.position);

                message += " +" + confiscationMistrust;
            }

        }
        Debug.Log(message);

        Debug.Log("mistrust = " + mistrust + " hidenMistrust = " + hidenMistrust);

        if (perfectRunCheck)
        {
            gold += perfectRunGold;
        }

        GoldUpdate();
        MistrustUpdate();
    }

    public void OnNewGame()
    {
        homeCap = maxCap;
        foodCap = maxCap;
        familyCap = maxCap;

        gold = 500;
        mistrust = 0;
        hidenMistrust = 0;
        day = 0;
    }

    public void Bind()
    {
        UIManager uIManager = gameManager.uIManager;

        uIManager.losingContainer.button.onClick.AddListener(OnNewGame);

        uIManager.timer.onTimeOut += OnTimeUp;
        uIManager.submit.onClick.AddListener(PassTheCar);

        uIManager.nextDayButton.onClick.AddListener(NextDayHandler);
        void NextDayHandler()
        {
            uIManager.nextDayButton.gameObject.SetActive(false);
        };

        uIManager.nextDayButton.onClick.AddListener(DailyUpdate);

        uIManager.home.button.onClick.AddListener(HomeIncrement);
        uIManager.food.button.onClick.AddListener(FoodIncrement);
        uIManager.family.button.onClick.AddListener(FamilyIncrement);

        showText += uIManager.floatingText.ShowText;

        onDayEnd += DayStart;

        onLose += OnLose;
    }

    public void PassTheCar()
    {
        if (isPlaying)
        {
            CheckItems(bribeBox.items, passBox.items, confiscationBox.items);
            CarArrived();

        }
    }

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

        gameManager.uIManager.timer.SetTimer(timePerDay);
        gameManager.uIManager.timer.StartTimer();

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
            onLose.Invoke();
        }
        else if (homeCap <= 0)
        {
            onLose.Invoke();
        }
        else if (familyCap <= 0)
        {
            onLose.Invoke();
        }
        else if (mistrust + hidenMistrust > 1f)
        {
            onLose?.Invoke();
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
        mistrust -= daylyMistrustDecrease;

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
    public void IconsUpdate()
    {
        gameManager.uIManager.food.sliderHandler.SetEndValue((float)foodCap / maxCap);
        gameManager.uIManager.family.sliderHandler.SetEndValue((float)familyCap / maxCap);
        gameManager.uIManager.home.sliderHandler.SetEndValue((float)homeCap / maxCap);

    }

    public void FoodIncrement()
    {
        if (foodCap < maxCap && gold >= foodCost)
        {
            foodCap++;
            gold -= foodCost;
        }
        gameManager.uIManager.food.sliderHandler.SetEndValue((float)foodCap / maxCap);
        //Debug.Log("FoodIncrement");
        GoldUpdate();
    }
    public void FamilyIncrement()
    {
        if (familyCap < maxCap && gold >= familyCost)
        {

            gold -= familyCost;
            familyCap++;
        }
        gameManager.uIManager.family.sliderHandler.SetEndValue((float)familyCap / maxCap);
        //Debug.Log("FamilyIncrement");
        GoldUpdate();

    }
    public void HomeIncrement()
    {
        if (homeCap < maxCap && gold >= homeCost)
        {

            homeCap++;
            gold -= homeCost;
        }
        gameManager.uIManager.home.sliderHandler.SetEndValue((float)homeCap / maxCap);
        //Debug.Log("HomeIncrement");
        GoldUpdate();

    }

    public void MistrustUpdate()
    {
        Debug.Log("misstrust update");
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

        IconsUpdate();
        MistrustUpdate();
        GoldUpdate();
    }

    public void OnTimeUp()
    {
        DayEnd();
        Debug.Log("Time up");
    }

    public void OnLose()
    {
        Debug.Log("lost");
    }
}

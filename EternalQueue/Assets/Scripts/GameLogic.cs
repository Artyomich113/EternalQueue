using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Artyomich;

public class GameLogic
{
    public GameManager gameManager;

    public static GameData gameData;

    public Timer timer;

    public List<ItemData> bannedItems = new List<ItemData>();

    Box entryBox;

    Box confiscationBox;

    Box bribeItems;

    Dictionary<string, ItemData> itemDataByName = new Dictionary<string, ItemData>();

    Action onDayEnd;

    Action onLose;

    /// <summary>
    /// value beetwen 0 and 1
    /// </summary>

    int day = 0;
    float mistrust = 0f;
    float hidenMistrust = 0f;

    float gold = 500f;


    int brabeGold = 200;
    int perfectRunGold = 100;


    float brabeMistrust = 0.2f;
    float firstMistakeMistrust = 0.1f;
    float mistakeMistrust = 0.05f;
    float daylyMistrustDecrease = 0.2f;

    float confiscationMistrust = 0.02f;

    bool isPlaying;

    float time = 60;



    public void CheckItems(List<Entity> bribeItems, List<Entity> passedItems, List<Entity> confiscatedItems)
    {
        bool perfectRunCheck = true;
        foreach (var ob in bribeItems)
        {
            if (bannedItems.Any(p => p.name == ob.name))
            {
                gold += brabeGold;
                hidenMistrust += brabeMistrust;
            }
            else
            {
                mistrust += brabeMistrust;
            }
        }

        foreach (var ob in confiscatedItems)
        {
            if (bannedItems.Any(p => p.name == ob.name))
            {
                mistrust -= confiscationMistrust;
            }
            else
            {
                mistrust += brabeMistrust;
            }
        }

        foreach (var ob in passedItems)
        {
            if (bannedItems.Any(p => p.name == ob.name))
            {
                mistrust = Mathf.Clamp(mistrust + confiscationMistrust, 0f, 1.1f);
            }
            else
            {

            }
        }

        if (perfectRunCheck)
        {
            gold += perfectRunGold;
        }


        GoldUpdate();
        MistrustUpdate();
    }

    public void PassTheCar()
    {
        if (isPlaying)
        {

            CheckItems(bribeItems.items, entryBox.items, confiscationBox.items);
            CarArrived();

        }
    }

    public void DayStart()
    {
        bannedItems.RemoveRange(0, bannedItems.Count);
        int[] itemIndexes = gameManager.gameItems.items.GetUniqueIndexes(UnityEngine.Random.Range(4, 8));
        foreach (var i in itemIndexes)
        {
            bannedItems.Add(gameManager.gameItems.items[i]);
        }
        Debug.Log(itemIndexes.Print());

        gameManager.uIManager.dropDownView.itemDatas = bannedItems;
        gameManager.uIManager.dropDownView.GenerateListView(gameManager.uIManager);
        CarArrived();

        gameManager.uIManager.timer.SetTimer(time);
        gameManager.uIManager.timer.StartTimer();

        isPlaying = true;
    }

    public void DayEnd()
    {
        Debug.Log("DayEnd");

        isPlaying = false;


        if (mistrust + hidenMistrust > 1f)
        {
            onLose?.Invoke();
        }
        else
            onDayEnd?.Invoke();
    }

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

        entryBox.Clear();
        confiscationBox.Clear();
        bribeItems.Clear();

        for (int i = 0; i < itemsCount; i++)
        {
            Item item = new Item
            {
                name = gameManager.gameItems.items[itemIds[i]].name,
                weight = UnityEngine.Random.Range(1f, 10f)
            };

            Entity entity = PoolManager.instanse.entityPool.Get() as Entity;

            entryBox.ItemPlaced(entity);
            entity.transform.position = gameManager.converter.RandomPositionInSquare(entryBox.transform, -0.01f);

            entity.SetTexture(itemDataByName[item.name].texture);
        }
    }

    public void MistrustUpdate()
    {
        Debug.Log("misstrust update");
        gameManager.uIManager.misstrust.SetValue(mistrust, hidenMistrust);
    }

    public void GoldUpdate()
    {
        gameManager.uIManager.gold.text.text = gold.ToString() + "G";
    }

    public void Init()
    {
        foreach (var ob in gameManager.gameItems.items)
        {
            itemDataByName.Add(ob.name, ob);
        }



        //Vector3 root = new Vector3(0, 0, 0);

        entryBox = PoolManager.instanse.boxPool.Get() as Box;
        entryBox.transform.localScale = gameManager.converter.DefineScale(new Vector3(0.4f, 0.4f, 1f));
        entryBox.transform.position = gameManager.converter.DefinePosition(new Vector3(0.25f, 0.25f, 0f));
        entryBox.SetColor(Color.white * 0.8f);

        confiscationBox = PoolManager.instanse.boxPool.Get() as Box;
        confiscationBox.transform.position = gameManager.converter.DefinePosition(new Vector3(0.75f, 0.55f, 0f));
        confiscationBox.SetColor(Color.green * 0.8f);
        confiscationBox.transform.localScale = gameManager.converter.DefineScale(new Vector3(0.2f, 0.2f, 1f));

        bribeItems = PoolManager.instanse.boxPool.Get() as Box;
        bribeItems.transform.position = gameManager.converter.DefinePosition(new Vector3(0.75f, 0.25f, 0f));
        bribeItems.SetColor(new Color(0.6f, 0f, 0.8f));
        bribeItems.transform.localScale = gameManager.converter.DefineScale(new Vector3(0.2f, 0.2f, 1f));

        gameManager.uIManager.windowPainController.MoveToWindow();

        gameManager.uIManager.misstrust.SetValue(mistrust, hidenMistrust);

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

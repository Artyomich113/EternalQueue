using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameLogic
{
    public GameManager gameManager;

    public static GameData gameData;

    public Timer timer;

    public string[] bannedItems;

    Box entryBox;

    Box deniedItems;

    Box brabeItems;

    float mistrust;

    public void CarPassed(Entity[] bribeItems, Entity[] passedItems, Entity[] confiscatedItems)
    {

    }

    public void Init()
    {
        Vector3 root = new Vector3(0, 0, 0);

        entryBox = PoolManager.instanse.boxPool.Get() as Box;
        entryBox.transform.localScale = gameManager.converter.DefineScale(new Vector3(0.4f, 0.4f, 1f));
        entryBox.transform.position = gameManager.converter.DefinePosition(new Vector3(0.25f, 0.25f, 0f));
        entryBox.SetColor(Color.white * 0.8f);

        deniedItems = PoolManager.instanse.boxPool.Get() as Box;
        deniedItems.transform.position = gameManager.converter.DefinePosition(new Vector3(0.75f, 0.55f, 0f));
        deniedItems.SetColor(Color.green * 0.8f);
        deniedItems.transform.localScale = gameManager.converter.DefineScale(new Vector3(0.2f, 0.2f, 1f));

        brabeItems = PoolManager.instanse.boxPool.Get() as Box;
        brabeItems.transform.position = gameManager.converter.DefinePosition(new Vector3(0.75f, 0.25f, 0f));
        brabeItems.SetColor(new Color(0.6f, 0f, 0.8f));
        brabeItems.transform.localScale = gameManager.converter.DefineScale(new Vector3(0.2f, 0.2f, 1f));

        gameManager.uIManager.windowPainController.MoveToWindow();
    }

    public void OnTimeUp()
    {
        Debug.Log("Time up");
    }

    public void OnLose()
    {

    }


}

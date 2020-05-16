using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameLogic
{
	public MainMenuManager mainMenuManager;

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
		entryBox.transform.localScale = new Vector2(12, 7);
		entryBox.transform.position = new Vector3(-3.5f, 0, 0);
		entryBox.SetColor(Color.white * 0.8f);
		
		deniedItems = PoolManager.instanse.boxPool.Get() as Box;
		deniedItems.transform.position = new Vector3(6.5f, 2f, 0);
		deniedItems.SetColor(Color.green * 0.8f);
		deniedItems.transform.localScale = new Vector2(5, 3);

		brabeItems = PoolManager.instanse.boxPool.Get() as Box;
		brabeItems.transform.position = new Vector3(6.5f, -2f, 0);
		brabeItems.SetColor(new Color(0.6f, 0f, 0.8f));
		brabeItems.transform.localScale = new Vector2(5, 3);

		mainMenuManager.windowPainController.MoveToWindow();
	}

	public void OnTimeUp()
	{
		Debug.Log("Time up");	
	}

	public void OnLose()
	{

	}


}

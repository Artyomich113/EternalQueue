using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameLogic
{
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

	}

	public void OnTimeUp()
	{
		Debug.Log("Time up");	
	}

	public void OnLose()
	{

	}


}

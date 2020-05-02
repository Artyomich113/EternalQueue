using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
	public Button playGame;
	public Button loadGame;
	public Button credits;

	private void OnDestroy()
	{
		playGame.onClick.RemoveAllListeners();
		loadGame.onClick.RemoveAllListeners();
		credits.onClick.RemoveAllListeners();
	}
}

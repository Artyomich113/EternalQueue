using System.Text;
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class GameData
{
	public string fileName;
	public bool isTutorial;

	public bool isSessionTerminated;
	public int day;
	public int money;
	public float mistrust;

	public GameData(string name)
	{
		fileName = name;
	}

	public static void CreateDataFile(string name)
	{
		string json = JsonUtility.ToJson(new GameData(name));
		using (StreamWriter stream = new StreamWriter(Application.persistentDataPath + "Assets/Data/GameSlots/" + name))
		{
			stream.Write(json);
		}
	}

	public static void DeleteDataFile(string name)
	{
		if(File.Exists(Application.persistentDataPath + "Assets/Data/GameSlots/" + name))
		{
			File.Delete(Application.persistentDataPath + "Assets/Data/GameSlots/" + name);
		}
	}

	public void Save()
	{
		string json = JsonUtility.ToJson(this);
		using (StreamWriter stream = new StreamWriter(Application.persistentDataPath + "Assets/Data/GameSlots/" + fileName))
		{
			stream.Write(json);
		}
	}

	public static GameData Load(string name)
	{
		GameData data = new GameData(name);

		using (StreamReader stream = new StreamReader(Application.persistentDataPath + "Assets/Data/GameSlots/" + name))
		{

			string json = stream.ReadToEnd();
			data = JsonUtility.FromJson<GameData>(json);
		}
		return data;
	}
}

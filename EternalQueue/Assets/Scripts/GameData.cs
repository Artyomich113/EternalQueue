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

	public GameData()
	{
		fileName = "";
	}

	public static void CreateDataFile(string name)
	{
		string json = JsonUtility.ToJson(new GameData(name));

		if (!Directory.Exists(Application.persistentDataPath + "/gameSlots"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/gameSlots");
		}

		using (StreamWriter stream = new StreamWriter(Application.persistentDataPath + "/gameSlots/" + name))
		{
			stream.Write(json);
		}
	}

	public static void DeleteDataFile(string name)
	{
		
		if (File.Exists(Application.persistentDataPath + "/gameSlots/" + name))
		{
			File.Delete(Application.persistentDataPath + "/gameSlots/" + name);
		}
	}

	public static void Save(GameData gameData)
	{
		string json = JsonUtility.ToJson(gameData);
		if(!Directory.Exists(Application.persistentDataPath + "/gameSlots"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/gameSlots");
		}

		using (StreamWriter stream = new StreamWriter(Application.persistentDataPath + "/gameSlots/" +gameData.fileName))
		{
			stream.Write(json);
		}
	}

	public static GameData Load(string Path)
	{
		GameData data = new GameData();

		using (StreamReader stream = new StreamReader(Path))
		{

			string json = stream.ReadToEnd();
			data = JsonUtility.FromJson<GameData>(json);
		}
		return data;
	}
}

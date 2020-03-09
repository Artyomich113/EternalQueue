using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DataPath : EditorWindow
{
	[MenuItem("DataPath/dataPath")]
	public static void AppDataPath()
	{
		Debug.Log(Application.dataPath);
	}

	[MenuItem("DataPath/persistentdataPath")]
	public static void AppPersistentDataPath()
	{
		Debug.Log(Application.persistentDataPath);
	}

	[MenuItem("Layout/Rebuild")]
	public static void RebuildLayout()
	{
		RectTransform[] rectTransforms = FindObjectsOfType<RectTransform>();
		Debug.Log(rectTransforms.Length);

		foreach (var ob in rectTransforms)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(ob);
		}
	}
}


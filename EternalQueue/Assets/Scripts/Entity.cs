using System;
using UnityEngine;


public class Entity : MonoBehaviour, IInteractive
{
	Item item;
	Vector3 offset;

	public Action<Entity> onItemRelesed;

	public void OnClick()
	{
		//Debug.Log("OnClick");
	}

	public void OnDown(Vector3 pos)
	{
		offset = pos - transform.position;
		Debug.Log("OnDown " + offset);
	}

	public void OnHold(Vector3 pos)
	{
		transform.position = pos - offset;
		Debug.Log("OnHold " + pos);
	}

	public void OnUp()// отпуск клавиши
	{
		offset = Vector3.zero;

		onItemRelesed?.Invoke(this);
		Debug.Log("OnUp");
	}
}

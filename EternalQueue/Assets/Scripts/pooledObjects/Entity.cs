using Artyomich;
using System;
using UnityEngine;


public class Entity : PooledObject, IInteractive
{
	[HideInInspector]
	public Item item;

	public MeshRenderer MeshRenderer;

	Material material;

	private void Awake()
	{
		material = MeshRenderer.material;
	}
	public void SetTexture(Texture2D texture2D)
	{
		material.SetTexture("_MainTex",texture2D);
	}

	Vector3 offset;
	Vector3 preDragPos;

	public Action<Entity> onItemRelesed;

	public void OnClick()
	{
		//Debug.Log("OnClick");
	}

	public void OnDown(Vector3 pos)
	{
		preDragPos = transform.position;

		offset = transform.position - pos;
		Debug.Log($"OnDown tran {transform.position} pos {pos} offset {offset}");
	}

	public void OnHold(Vector3 pos)
	{
		transform.position = (pos + offset).SetZ(transform.position.z);
		Debug.Log("OnHold " + pos);
	}

	public void OnUp(Box box)// отпуск клавиши
	{
		offset = Vector3.zero;
		if (box)
		{
			onItemRelesed?.Invoke(this);
			box.ItemPlaced(this);
		}
		else
		{
			transform.position = preDragPos;
		}
		Debug.Log("OnUp");
	}
}

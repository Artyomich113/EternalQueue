using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Box : MonoBehaviour, IInteractive
{
	public List<Entity> items;

	public void ItemPlaced(Entity item)//mouseUp. Controled in ClickSelectController.
	{
		if (!items.Contains(item))
		{
			items.Add(item);
			item.onItemRelesed += ItemRelesed;
		}

	}

	public void ItemRelesed(Entity item)//mouseUp. Controlled under item's Action<Item> onItemRelesed.
	{
		if (items.Contains(item))
		{
			items.Remove(item);
			item.onItemRelesed -= ItemRelesed;
		}
	}

	public void OnClick()
	{

	}

	public void OnDown(Vector3 v)
	{

	}

	public void OnHold(Vector3 v)
	{

	}

	public void OnUp()
	{

	}
}


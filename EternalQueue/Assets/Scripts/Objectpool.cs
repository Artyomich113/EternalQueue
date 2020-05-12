using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artyomich;

public class Objectpool : MonoBehaviour
{

	[SerializeField]
	private PooledObject[] prefabs;

	private Queue<PooledObject> objects;


	[SerializeField, Range(1, 10)]
	int increment = 1;

	//public int count;

	private void Awake()
	{
		objects = new Queue<PooledObject>();
	}
	public int GetLenth()
	{
		return objects.Count;
	}

	public PooledObject Get()
	{
		//Debug.Log("genericobjectpool index " + index);
		if (objects.Count == 0)
			addobjects(increment);
		PooledObject ob = objects.Dequeue();

		ob.gameObject.SetActive(true);
		return ob;
	}

	public void returnToPool(PooledObject objectToReturn, int index = 0)
	{
		objectToReturn.gameObject.SetActive(false);
		objects.Enqueue(objectToReturn);
	}

	public void addobjects(int count)
	{
		for (int i = 0; i < count; i++)
		{
			var newobject = Instantiate(prefabs.RandomElement());
			newobject.gameObject.SetActive(false);
			newobject.objectpool = this;
			objects.Enqueue(newobject);
		}
	}
}
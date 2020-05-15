using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	public static PoolManager instanse;


	public Objectpool boxPool;
	public Objectpool guyPool;
	public Objectpool entityPool;

	private void Awake()
	{
		if (instanse == null)
		{
			instanse = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this);
		}
	}

	private void Start()
	{
		
	}
}


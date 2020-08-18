using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public struct ObjectPoolName
{
    public Objectpool pool;
    public string name;
}
public class PoolManager : MonoBehaviour
{
    public static PoolManager instanse;

    public ObjectPoolName[] objectPoolName;

    Dictionary<string, Objectpool> objectPoolByName = new Dictionary<string, Objectpool>();

    private void Awake()
    {
        if (instanse == null)
        {
            instanse = this;
            DontDestroyOnLoad(this);

            foreach (var ob in objectPoolName)
            {
                objectPoolByName.Add(ob.name, ob.pool);
            }
            Debug.Log("PoolManager Initialized");
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {

    }

    public Objectpool GetPool(string name)
    {
        if (objectPoolByName.ContainsKey(name))
            return objectPoolByName[name];
        else
            return null;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
	[HideInInspector]
    public Objectpool objectpool;

    public virtual void ReturnToPool ()
    {
		objectpool?.returnToPool (this);
    }
}
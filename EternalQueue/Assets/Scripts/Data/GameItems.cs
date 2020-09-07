using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameItems : ScriptableObject
{
    public ItemData[] items;

    private void Awake()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].id = i;
        }
    }
}

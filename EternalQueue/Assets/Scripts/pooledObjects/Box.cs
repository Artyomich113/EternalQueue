using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Box : PooledObject, IInteractive
{

    public List<Entity> items;

    public MeshRenderer meshRenderer;

    [HideInInspector]
    public Material material;

    private void Awake()
    {
        material = meshRenderer.material;
    }

    public void SetColor(Color color)
    {
        //Debug.Log(color);
        material.SetColor("_Color", color);
    }

    public void SetSize(float wight, float height)
    {
        transform.localScale = new Vector3(wight, height, 1);
    }

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


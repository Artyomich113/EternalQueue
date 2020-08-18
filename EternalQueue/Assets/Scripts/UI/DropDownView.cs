using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DropDownView : MonoBehaviour, IDragHandler
{
    public UnityEngine.UI.Button dropDownButton;

    public VerticalLayoutGroup verticalLayoutGroup;

    public ScrollRect scrollRect;
    [HideInInspector]
    public List<ItemData> itemDatas;
    [HideInInspector]
    public List<ItemView> itemViews;

    public Text dropDownText;

    public Objectpool itemViewPool;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.W))
    //        scrollRect.verticalNormalizedPosition = 0.0f;

    //    if (Input.GetKeyDown(KeyCode.S))
    //        scrollRect.verticalNormalizedPosition = 1.0f;

    //}

    private void Awake()
    {
        dropDownButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        scrollRect.gameObject.SetActive(!scrollRect.gameObject.activeSelf);
    }

    public void GenerateListView(UIManager uIManager)
    {
        foreach (var itemview in itemViews)
        {
            itemview.ReturnToPool();
        }
        itemViews.RemoveRange(0, itemViews.Count);

        foreach (var itemData in itemDatas)
        {
            ItemView itemView = itemViewPool.Get() as ItemView;
            itemView.text.text = itemData.name;
            itemView.image.sprite = uIManager.entitySprites[itemData.id];

            itemViews.Add(itemView);

            itemView.transform.SetParent(verticalLayoutGroup.transform);
        }

        scrollRect.verticalNormalizedPosition = 1.0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
}

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

    public List<ItemData> itemDatas;
    public List<ItemView> itemViews;

    public Text dropDownText;

    public Objectpool itemViewPool;

    private void Awake()
    {
        dropDownButton.onClick.AddListener(OnButtonClick);
    }
    private void Start()
    {

    }

    public void OnButtonClick()
    {
        scrollRect.gameObject.SetActive(!scrollRect.gameObject.activeSelf);
    }

    public void GenerateListView()
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
            itemView.image.sprite = itemData.sprite;

            itemViews.Add(itemView);

            itemView.transform.SetParent(verticalLayoutGroup.transform);
        }

        scrollRect.verticalNormalizedPosition = 1f;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
}

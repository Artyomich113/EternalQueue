using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameSlotCreateView : MonoBehaviour
{
    public System.Action<string> OnGameSlotClickCreate;

    public InputField slotName;

    public Button createButton;

    private void Start()
    {
        slotName.text = "Gamers Game for Gaymers";
    }

    private void OnEnable()
    {
        createButton.onClick.AddListener(OnCreateButtonPressed);
        slotName.onEndEdit.AddListener(OnEndEdit);
    }

    private void OnDisable()
    {
        createButton.onClick.RemoveListener(OnCreateButtonPressed);
        slotName.onEndEdit.RemoveListener(OnEndEdit);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
    public void OnCreateButtonPressed()
    {
        Debug.Log("GameSlotCreateView click");

        string gameName = slotName.text;
        if (!string.IsNullOrEmpty(gameName))
        {
            OnGameSlotClickCreate?.Invoke(slotName.text);
        }
    }

    public void OnEndEdit(string name)
    {
        Debug.Log("OnEndEdit " + name);
    }

}

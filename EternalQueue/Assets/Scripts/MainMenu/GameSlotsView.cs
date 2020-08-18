using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//идея в том, чтобы инстансить этот класс и добавлять в view монобехи, GameSlot
public class GameSlotsView : MonoBehaviour
{
    //родитель для слотов
    public RectTransform content;

    //слот
    public GameSlot gameSlotView;
    //слот создания слота
    public GameSlotCreateView gameSlotCreateView;
    //кнопка возврата
    public Button returnButton;

    public Action<GameData> OnDataClickSelect;
    public Action<GameData> OnDataClickDelete;
    public Action<string> OnDataClickCreate;

    private void Start()
    {
        returnButton.onClick.AddListener(OnReturnClick);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnReturnClick();
        }
    }

    public void OnReturnClick()
    {
        gameObject.SetActive(false);
    }

    public void OnDataDelete(GameData gameData)
    {
        //Debug.Log("OnDataDelete");
        OnDataClickDelete?.Invoke(gameData);
    }

    public void OnDataSelect(GameData gameData)
    {
        OnDataClickSelect?.Invoke(gameData);
    }

    public void OnDataCreate(string name)
    {
        //Debug.Log("OnDataCreate");
        OnDataClickCreate?.Invoke(name);
    }

    public void CreateGameDataSlot(GameData gameData, bool preLast = false)
    {
        //Debug.Log("CreateGameDataSlot " + gameData.fileName);

        GameSlot gameslot = Instantiate(gameSlotView, content.transform);
        gameslot.gameData = gameData;

        if (preLast)
        {
            gameslot.transform.SetSiblingIndex(gameslot.transform.parent.childCount - 2);
        }
        gameslot.onDataClickDelete += OnDataDelete;
        gameslot.onDataClickSelect += OnDataSelect;
    }

    public void LoadGameSlots(string[] files)
    {
        for (int i = 0; i < files.Length; i++)
        {

            GameData gameData = GameData.Load(files[i]);
            
            CreateGameDataSlot(gameData);
        }


        GameSlotCreateView localgameSlotCreateView = Instantiate(gameSlotCreateView, content.transform);
        localgameSlotCreateView.OnGameSlotClickCreate += OnDataCreate;
    }
}

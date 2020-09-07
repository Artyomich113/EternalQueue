using UnityEngine;
using System;

[Serializable]
public class GameSettingsData
{
    public int startGold = 500;
    public float startMistrust = 0;

    public int homeCost = 350;
    public int foodCost = 350;
    public int familyCost = 300;

    public int maxCap = 3;

    public int bribeGold = 200;
    public int perfectRunGold = 100;
    
    public float bannedItemInbrabeBoxMistrust = 0.2f;
    public float normalItemInBrabeBoxMistrust = 0.1f;
    public float bannedItemInPassBoxMistrust = 0.05f;
    public float normalItemInPassBoxMistrust = 0.05f;
    public float bannedItemInconfiscationBoxMistrust = 0.02f;
    public float normalItemInconfiscationBoxMistrust = 0.02f;
    public float daylyMistrustDecrease = 0.2f;

    public float timePerDay = 60;

    public GameSettingsData(GameSettingsData gameSettingsData)
    {
        startGold = gameSettingsData.startGold;
        startMistrust = gameSettingsData.startMistrust;
        homeCost = gameSettingsData.homeCost;
        foodCost = gameSettingsData.foodCost;
        familyCost = gameSettingsData.familyCost;
        maxCap = gameSettingsData.maxCap;
        
        bribeGold = gameSettingsData.bribeGold;
        perfectRunGold = gameSettingsData.perfectRunGold;
        bannedItemInbrabeBoxMistrust = gameSettingsData.bannedItemInbrabeBoxMistrust;
        normalItemInBrabeBoxMistrust = gameSettingsData.normalItemInBrabeBoxMistrust;
        bannedItemInPassBoxMistrust = gameSettingsData.bannedItemInPassBoxMistrust;
        daylyMistrustDecrease = gameSettingsData.daylyMistrustDecrease;
        bannedItemInconfiscationBoxMistrust = gameSettingsData.bannedItemInconfiscationBoxMistrust;
        timePerDay = gameSettingsData.timePerDay;
    }
}
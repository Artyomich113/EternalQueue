using System.Collections;
using System.Collections.Generic;
using UIScripts;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //public Image backGrowndImage;

    public MainMenu mainMenu;

    public GameSlotsView gameSlots;

    public Canvas mainCanvas;

    public Misstrust misstrust;

    public Gold gold;

    public Timer timer;

    public RectTransform inGameUI;

    public WindowPainController windowPainController;

    public Button submit;

    public DropDownView dropDownView;

    public Button nextDay;

    public SliderButton home;
    public SliderButton family;
    public SliderButton food;

    [HideInInspector]
    public Sprite[] entitySprites;
}

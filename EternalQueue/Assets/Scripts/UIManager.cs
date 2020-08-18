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

    public Text goldText;

    public Timer timer;

    public RectTransform inGameUI;

    public WindowPainController windowPainController;

    public Button submit;

    public DropDownView dropDownView;

    public Button nextDayButton;

    public LosingContainer losingContainer;

    public SliderButton home;
    public SliderButton family;
    public SliderButton food;

    public FloatingText floatingText;

    [HideInInspector]
    public Sprite[] entitySprites;
}

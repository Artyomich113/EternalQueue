using System.Collections;
using System.Collections.Generic;
using UIScripts;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image backGrowndImage;

    public MainMenu mainMenu;

    public GameSlotsView gameSlots;
    public Canvas mainCanvas;

    public Slider mistrustSlider;

    public readonly string format = "{0:0.#}";

    public Text mistrustText;

    public Gold gold;

    public Timer timer;

    public RectTransform inGameUI;

    public WindowPainController windowPainController;

    public Button submit;


    public DropDownView dropDownView;
}

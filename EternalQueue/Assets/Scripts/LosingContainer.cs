using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Artyomich;

public class LosingContainer : MonoBehaviour
{
    public Button button;

    public Text text;

    public Image image;

    private void Awake()
    {
        void OnInterract()
        {
            button.interactable = false;
        }

        button.onClick.AddListener(OnInterract);
        button.onClick.AddListener(Disapear);
    }
    public void Appear(string loseCondition)
    {
        button.gameObject.SetActive(true);
        button.interactable = true;
        button.transform.localScale = Vector3.zero;
        button.transform.DOScale(Vector3.one, 1f);

        image.gameObject.SetActive(true);
        image.color = image.color.SetA(0f);
        image.DOFade(1f, 1f);

        text.gameObject.SetActive(true);
        text.text = loseCondition;

    }

    public void Disapear()
    {
        Debug.Log("Disapear");

        button.transform.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(button.transform.DOScale(Vector3.zero, 1f));
        
        image.color = image.color.SetA(1f);
        sequence.Insert(0f, image.DOFade(0f, 1f));
        
        sequence.AppendCallback(new TweenCallback(onEnd));

        TweenCallback tweenCallback = new TweenCallback(onEnd);

        void onEnd()
        {
            image.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }
    }
}

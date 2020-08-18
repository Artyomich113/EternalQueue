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
    public void Appear(string loseCondition = "")
    {
        button.transform.localScale = Vector3.zero;
        button.transform.DOScale(Vector3.one, 1f);

        image.color = image.color.SetA(0f);
        image.DOFade(1f, 1f);

        text.text = loseCondition;

    }

    public void Disapear()
    {

        image.color = image.color.SetA(0f);
        button.transform.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(button.transform.DOScale(Vector3.one, 1f));
        sequence.Insert(0f, image.DOFade(1f, 1f));
        
        sequence.AppendCallback(new TweenCallback(onEnd));

        TweenCallback tweenCallback = new TweenCallback(onEnd);

        void onEnd()
        {
            gameObject.SetActive(false);
        }
    }
}

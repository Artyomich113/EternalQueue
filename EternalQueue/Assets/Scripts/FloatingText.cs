using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SocialPlatforms;

public class FloatingText : MonoBehaviour
{
    
    [Range(1,60)]
    public float fadeDuration;

    public Text text;

    public RectTransform canvas;

    public Camera cameraRef;
    public void ShowText(float value, Color color, Vector3 position)
    {
        Text localText = Instantiate(text, canvas);

        localText.text = value.ToString();
        localText.color = color;

        Vector2 viewportPosition = cameraRef.WorldToViewportPoint(position);

        Vector2 WorldObject_ScreenPosition = new Vector2(
 ((viewportPosition.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f)),
 ((viewportPosition.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f)));

        localText.rectTransform.anchoredPosition = WorldObject_ScreenPosition;

        void DestroyText()
        {
            Destroy(localText.gameObject);
        }

        localText.DOFade(0f, fadeDuration).OnComplete(new TweenCallback(DestroyText));

    }

}

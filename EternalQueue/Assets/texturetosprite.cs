using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class texturetosprite : MonoBehaviour
{
    public Texture2D texture2D;

    public Sprite sprite;

    public RectTransform rectTransform;

    Sprite spriteref;
    private void Awake()
    {
        spriteref = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.0f, 0.0f), 100f);
        spriteref.name = "my new sprite";

        GetComponent<Image>().sprite = spriteref;
    }

}

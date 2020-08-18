using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artyomich;

public class ValueItterator : MonoBehaviour
{
    public float speed;

    float value;

    float endValue;

    public Action<float> onValueChanged;

    public Coroutine coroutine = null;

    public void SetValue(float val)
    {
        value = val;

        onValueChanged.Invoke(value);
    }
    public void SetEndValue(float val)
    {
        //Debug.Log("Set end value " + val);
        endValue = val;
        if (coroutine == null)
        {
            Debug.Log("Starting to itterate mistrust vaue");
            coroutine = StartCoroutine(SliderValueHandler());
        }
        else
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(SliderValueHandler());
        }
    }

    IEnumerator SliderValueHandler()
    {
        do
        {
            float substraction = endValue - value;
            value += Mathf.Sign(substraction) * (speed * Time.deltaTime).GoToFloat(Mathf.Abs(substraction));
            //Debug.Log("value " + value);
            onValueChanged?.Invoke(value);
            yield return null;
        } while (value != endValue);
    }
}

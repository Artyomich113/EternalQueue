using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Artyomich;
public class SliderHandler : ValueItterator
{
    public Slider slider;

    void OnValueChanged(float val)
    {
        //Debug.Log("sliderhandler onvalchanged " + val);
        slider.value = val;
    }
    
    private void OnEnable()
    {
        onValueChanged += OnValueChanged;
    }
    private void OnDisable()
    {
        onValueChanged -= OnValueChanged;
    }



    //[Range(0, 1)]
    //public float speed;

    //[HideInInspector]
    //float value;

    //[HideInInspector]
    //float EndValue;

    //public Coroutine coroutine = null;

    //public void SetValue(float val)
    //{
    //    value = val;

    //    slider.value = value;
    //}
    //public void SetEndValue(float val)
    //{
    //    EndValue = val;
    //    if (coroutine == null)
    //    {
    //        coroutine = StartCoroutine(SliderValueHandler());
    //        Debug.Log("Starting to itterate mistrust vaue");
    //    }
    //    //StopCoroutine(coroutine);
    //}


    //IEnumerator SliderValueHandler()
    //{
    //    Debug.Log($"start value {value} endvalue {EndValue}");
    //    do
    //    {
    //        float substraction = EndValue - value;
    //        value += Mathf.Sign(substraction) * (speed * Time.deltaTime).GoToFloat(Mathf.Abs(substraction));
    //        slider.value = value;
    //        slider.onValueChanged?.Invoke(slider.value);
    //        Debug.Log($"value {value}");
    //        yield return null;
    //    } while (value != EndValue);
    //}
}

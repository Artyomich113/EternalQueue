using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class Misstrust : MonoBehaviour
    {
        public Text text;

        public SliderHandler sliderHandler;

        public readonly string format = "{0:0.#}";

        public float hidenMistrustValue = 0f;

        public bool hideValue = true;

        private void OnEnable()
        {
            sliderHandler.onValueChanged += OnMisstrustValueChanged;

        }

        private void OnDisable()
        {
            sliderHandler.onValueChanged -= OnMisstrustValueChanged;
        }

        public void OnMisstrustValueChanged(float value)
        {
            //sliderHandler.slider.value = value;

            string hidenMistrust = " ";
            if (hidenMistrustValue != 0f)
            {
                hidenMistrust += (hidenMistrustValue > 0f) ? '+' : '-';
                if (hideValue)
                    hidenMistrust += "?";
                else
                    hidenMistrust += hidenMistrustValue;
            }

            text.text = string.Format(format, value * 100f) + "%" + hidenMistrust;
        }

        public void SetValue(float value, float hidenValue)
        {
            Debug.Log("mistrust set value");
            hidenMistrustValue = hidenValue;

            sliderHandler.SetEndValue(value);
        }
    }
}

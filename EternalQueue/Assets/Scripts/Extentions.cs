using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Artyomich
{

    public static class Extentions
    {
        /// <summary>
        /// helps to safely go to float without itteration 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="substraction"></param>
        /// <returns></returns>
        public static float GoToFloat(this float val, float substraction)
        {
            if (substraction > val)
            {
                return val;
            }
            else return substraction;
        }
        public static bool IsInside(this Vector2 pos, Rect rect)
        {
            return pos.x.IsBetween(rect.x, rect.x + rect.width) && pos.y.IsBetween(rect.y, rect.y + rect.height);
        }

        public static bool IsBetween(this float value, float leftValue, float rightValue)
        {
            return value > leftValue && value < rightValue;
        }
        public static float FractionalPart(this float val)
        {
            return (val % 1);
        }

        public static float FractionalPart(this float val, float val2)
        {
            return (val % val2);
        }

        public static Color SetA(this Color color, float val)
        {
            return new Color(color.r, color.g, color.b, val);
        }
        public static Color SetR(this Color color, float val)
        {
            return new Color(color.r, color.g, val, color.a);
        }
        public static Color SetG(this Color color, float val)
        {
            return new Color(color.r, val, color.b, color.a);
        }
        public static Color SetB(this Color color, float val)
        {
            return new Color(color.r, color.g, val, color.a);
        }

        public static Vector3 SetXY(this Vector3 vector, float valX, float valY)
        {
            return new Vector3(valX, valY, vector.z);
        }
        public static Vector3 SetYZ(this Vector3 vector, float valY, float valZ)
        {
            return new Vector3(vector.x, valY, valZ);
        }
        public static Vector3 SetZX(this Vector3 vector, float valZ, float valX)
        {
            return new Vector3(valX, vector.y, valZ);
        }

        public static Vector3 SetY(this Vector3 vector, float val)
        {
            return new Vector3(vector.x, val, vector.z);
        }
        public static Vector3 SetX(this Vector3 vector, float val)
        {
            return new Vector3(val, vector.y, vector.z);
        }
        public static Vector3 SetZ(this Vector3 vector, float val)
        {
            return new Vector3(vector.x, vector.y, val);
        }

        public static Vector2 SetX(this Vector2 vector, float val)
        {
            return new Vector2(val, vector.y);
        }
        public static Vector2 SetY(this Vector2 vector, float val)
        {
            return new Vector2(vector.x, val);
        }

        public static Vector3 ApplyMagnitude(this Vector3 vector, float mag)
        {
            return vector.normalized * mag;
        }

        public static Vector3 GetOrientationXZ(float val)
        {
            return new Vector3(Mathf.Sin(val), 0, Mathf.Cos(val));
        }

        public static T RandomElement<T>(this T[] source)
        {
            int index = UnityEngine.Random.Range(0, source.Length);
            //Debug.Log($"index {index}");
            return source[index];
        }

        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }

        private static System.Random rng = new System.Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        public static string Print<T>(this T[] array)
        {
            string str = "";
            for (int i = 0; i < array.Length; i++)
            {
                str += array[i] + " ";
            }
            return str;
        }

        public static int[] GetUniqueIndexes<T>(this T[]array, int count)
        {
            int lenth = array.Length;

            int[] a = new int[count];
            if (count >= lenth)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = i;
                }
                return a;
            }

            List<int> l = new List<int>();
            for (int i = 0; i < lenth; i++)
            {
                l.Add(i);
            }
            l.Shuffle();

            for (int i = 0; i < count; i++)
            {
                int index = UnityEngine.Random.Range(0, l.Count - 1);
                a[i] = l[index];
                l.RemoveAt(index);
            }

            return a;
        }

        public static HorizontalLayoutGroup CreateHorizontalLayoutElement(string name, RectTransform parent, bool ControllChildHeightSize, bool ControllChildWightSize, LayerMask layerMask, Rect rect)
        {
            GameObject localGameobject = new GameObject(name, typeof(RectTransform));
            RectTransform rectTransform = localGameobject.GetComponent<RectTransform>();
            //rectTransform.rect.Set(rect.x,rect.y,rect.width,rect.height);
            localGameobject.transform.SetParent(parent);
            HorizontalLayoutGroup horizontalLayoutGroup = localGameobject.AddComponent<HorizontalLayoutGroup>();

            horizontalLayoutGroup.childControlHeight = ControllChildHeightSize;
            horizontalLayoutGroup.childControlWidth = ControllChildWightSize;
            return horizontalLayoutGroup;
        }

        public static Text CreateText(RectTransform parent, string str, int fontsize, TextAnchor textAnchor, Font font, Color color)
        {
            GameObject localGameobject = new GameObject("Text", typeof(RectTransform));

            localGameobject.transform.SetParent(parent);
            Text text = localGameobject.AddComponent<Text>();
            text.text = str;
            text.font = font;
            text.color = color;
            text.fontSize = fontsize;
            text.raycastTarget = false;
            text.alignment = textAnchor;
            return text;
        }
    }
}
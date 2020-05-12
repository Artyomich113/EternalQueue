using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
	public Text text;

	float time = 0;

	public Action onTimeOut;

	public Coroutine timerCoroutine;

	public void SetTimer(float time)
	{
		this.time = time;
	} 

	public void StartTimer()
	{
		if (timerCoroutine != null)
		{
			StopCoroutine(timerCoroutine);
		}
		timerCoroutine = StartCoroutine(TimerCoroutine(time));
	}

	IEnumerator TimerCoroutine(float time)
	{
		while(time > 0)
		{
			time -= Time.deltaTime;

			int mins = (int)time/60;
			int sec = (int)time % 60;

			text.text = string.Format("{0:00}:{1:00}", mins, sec);
			yield return null;
		}
		onTimeOut?.Invoke();
	}

}

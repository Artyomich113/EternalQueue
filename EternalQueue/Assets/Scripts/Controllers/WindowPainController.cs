using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class WindowPainController : MonoBehaviour
{
	public Transform startTransform;
	public Transform stayTransform;
	public Transform finaleTransform;

	PooledObject target = null;

	private void Start()
	{
		target = PoolManager.instanse.GetPool("guy").Get();
	}

	private void OnDrawGizmos()
	{

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(startTransform.position, 0.1f);

		Gizmos.color = Color.green;
		Gizmos.DrawSphere(stayTransform.position, 0.1f);

		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(finaleTransform.position, 0.1f);
	}

	public void MoveToWindow()
	{
		if (DOTween.IsTweening(target.transform))
		{
			return;
		}

		target.transform.position = startTransform.position;
		target.transform.rotation = startTransform.rotation;

		Sequence sequence = DOTween.Sequence();

		sequence.Append(target.transform.DOMove(stayTransform.position, 1f));
		sequence.Insert(0.85f, target.transform.DORotateQuaternion(stayTransform.rotation, 0.25f));
	}

	public void MoveFromWindow()
	{
		if (DOTween.IsTweening(target.transform))
		{
			return;
		}

		target.transform.position = stayTransform.position;
		target.transform.rotation = stayTransform.rotation;

		Sequence sequence = DOTween.Sequence();

		sequence.Append(target.transform.DORotateQuaternion(finaleTransform.rotation, 0.25f));
		sequence.Append(target.transform.DOMove(finaleTransform.position, 1f));
	}

}

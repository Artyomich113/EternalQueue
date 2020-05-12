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

	public Transform target;

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
		target.position = startTransform.position;
		target.rotation = startTransform.rotation;

		Sequence sequence = DOTween.Sequence();

		sequence.Append(target.DOMove(stayTransform.position, 1f));
		sequence.Insert(0.85f, target.DORotateQuaternion(stayTransform.rotation, 0.25f));
	}

	public void MoveFromWindow()
	{
		target.position = stayTransform.position;
		target.rotation = stayTransform.rotation;

		Sequence sequence = DOTween.Sequence();

		sequence.Append(target.DORotateQuaternion(finaleTransform.rotation, 0.25f));
		sequence.Append(target.DOMove(finaleTransform.position, 1f));
	}

}

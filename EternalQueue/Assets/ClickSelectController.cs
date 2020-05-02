using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class ClickSelectController : MonoBehaviour
{
	[SerializeField]
	Camera _camera;

	public static event Action OnSelectedEntityChanged;

	public Entity itemEntity;

	public float clickTime = 0.2f;
	float downTime;

	public LayerMask itemsLayer;
	public LayerMask boxLayer;

	private void Start()
	{
		_camera = Camera.main;
	}

	private void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			downTime = Time.time;

			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out var hitInfo, 100f, itemsLayer))
			{
				itemEntity = hitInfo.collider.GetComponent<Entity>();
				itemEntity?.OnDown(hitInfo.point);
			}
		}

		if (Input.GetButton("Fire1"))// hold
		{
			Plane plane = new Plane(Vector3.back, Vector3.zero);
			Vector2 mousePos = Input.mousePosition;
			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (plane.Raycast(ray, out float enterDist))
			{
				Vector3 WorldMousePos = ray.GetPoint(enterDist);
				itemEntity?.OnHold(WorldMousePos);
			}

		}

		if (Input.GetButtonUp("Fire1"))
		{
			//Debug.Log("Up");
			if (Time.time - downTime < clickTime)// click
			{
				itemEntity?.OnClick();
			}
			else // release after hold
			{
				itemEntity?.OnUp();//убираю с ящика

				var ray = _camera.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out var hitInfo, 100, boxLayer) && itemEntity != null)// но если в ящик попал, то добавляю
				{
					var box = hitInfo.collider.GetComponent<Box>();
					box.ItemPlaced(itemEntity);
				}
			}
			itemEntity = null;
		}
	}

}

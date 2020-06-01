using Artyomich;
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
        if (!_camera)
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

        if (Input.GetButton("Fire1") && itemEntity)// hold
        {
            Plane plane = new Plane(Vector3.back, itemEntity.transform.position /*Vector3.zero*/);
            Vector2 mousePos = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float enterDist))
            {
                Vector3 WorldMousePos = ray.GetPoint(enterDist);
                //itemEntity?.OnHold(WorldMousePos.SetZ(itemEntity.transform.position.z));
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


            Box box = null;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 100, boxLayer) && itemEntity != null)// но если в ящик попал, то добавляю
            {
                box = hitInfo.collider.GetComponent<Box>();
                //box.ItemPlaced(itemEntity);
            }
            itemEntity?.OnUp(box);//убираю с ящика

            itemEntity = null;
        }
    }

}

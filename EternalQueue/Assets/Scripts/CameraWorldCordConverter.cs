using Artyomich;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWorldCordConverter : MonoBehaviour
{
    public Plane plane;

    public Camera cameraRef;

    float frontDist;
    float angle;
    float aspect;

    float zeroWight;
    float zeroHeight;

    /// <summary>
    /// BottomLeftCorner
    /// </summary>
    Vector3 bLCorner;

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Ray(cameraRef.transform.position, bLCorner - cameraRef.transform.position));
    }
    private void Awake()
    {
        frontDist = Math.Abs(cameraRef.transform.position.z);
       // Debug.Log("frontDist " + frontDist);
        angle = cameraRef.fieldOfView;
        aspect = cameraRef.aspect;

        zeroHeight = frontDist * Mathf.Tan(angle / 2 * Mathf.Deg2Rad) * 2;
       // Debug.Log("zeroHeight " + zeroHeight);
        zeroWight = zeroHeight * aspect;
       // Debug.Log("zerowight " + zeroWight);

        bLCorner = new Vector3(-zeroWight / 2, -zeroHeight / 2, 0);
        //Debug.Log("corner " + bLCorner);
        //Debug.Log("topRight " + (bLCorner + new Vector3(zeroWight, zeroHeight, 0f)));

    }

    /// <summary>
    /// x,y from 0 to 1, z - placed
    /// </summary>
    /// <param name="position">screen relative position</param>
    /// <returns></returns>
    public Vector3 DefinePosition(Vector3 position)
    {
        Vector3 vector3 = bLCorner + new Vector3(
            Mathf.InverseLerp(-frontDist, 0, position.z) * zeroWight * position.x,
            Mathf.InverseLerp(-frontDist, 0, position.z) * zeroHeight * position.y,
            position.z);

        return vector3;
    }

    /// <summary>
    /// x,y from 0 to 1, z - z position of object;
    /// </summary>
    /// <param name="scale">screen relative scale</param>
    /// <returns></returns>
    public Vector3 DefineScale(Vector3 scale)
    {
        Vector3 vector3 = new Vector3(Mathf.InverseLerp(-frontDist, 0, scale.z) * scale.x * zeroWight, Mathf.InverseLerp(-frontDist, 0, scale.z) * scale.y * zeroHeight, scale.z);
        return vector3;
    }

    public Vector3 RandomPositionInSquare(Transform obTransform, float zOffset = 0f)
    {
        Vector3 bottomLeft = obTransform.position - (obTransform.localScale / 2).SetZ(0f);

        float height = UnityEngine.Random.Range(0, obTransform.localScale.y);
        float wight = UnityEngine.Random.Range(0, obTransform.localScale.x);

        return bottomLeft + new Vector3(wight, height, zOffset);
    }
}

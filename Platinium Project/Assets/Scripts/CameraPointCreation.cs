using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointCreation : MonoBehaviour
{
    public Transform[] faceCenterPosition;

    private GameObject _cameraPointObj;
    public float diceCameraDistance;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < faceCenterPosition.Length; i++)
        {
            faceCenterPosition[i].transform.position = (faceCenterPosition[i].transform.position - transform.position).normalized * diceCameraDistance + transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //int layerMask = 1 << 13;

        //for (int i = 0; i < faceCenterPosition.Length - 1; i++)
        //{
            //RaycastHit hit;
            //// Does the ray intersect any objects excluding the player layer
            //if (Physics.Raycast(transform.position, new Vector3(faceCenterPosition[i].transform.position.x, faceCenterPosition[i].transform.position.y - transform.position.y, faceCenterPosition[i].transform.position.z) * 1000, out hit, Mathf.Infinity, layerMask))
            //{
            //    _cameraPointObj = new GameObject();
            //    _cameraPointObj.name = "CameraPostionFace" + i;
            //    _cameraPointObj.AddComponent<Transform>();
            //    _cameraPointObj.transform.position = hit.point;
            //    Instantiate(_cameraPointObj);

            //    Debug.DrawRay(transform.position, new Vector3(faceCenterPosition[i].transform.position.x, faceCenterPosition[i].transform.position.y - transform.position.y, faceCenterPosition[i].transform.position.z) * 1000 * hit.distance, Color.green);
            //    Debug.Log("Hit");
            //}
            //else
            //{
            //    Debug.DrawRay(transform.position, new Vector3(faceCenterPosition[i].transform.position.x, faceCenterPosition[i].transform.position.y - transform.position.y, faceCenterPosition[i].transform.position.z) * 1000, Color.white);
            //    Debug.Log("Did not Hit");
            //}
        //}
    }
}

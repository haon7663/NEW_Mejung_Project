using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Camera mCamera;
    private Camera mainCamera;
    private void Start()
    {
        mCamera = GetComponent<Camera>();
        mainCamera = Camera.main;
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, mainCamera.transform.position, Time.deltaTime * 7);
        transform.eulerAngles = mainCamera.transform.eulerAngles;
        mCamera.orthographicSize = mainCamera.orthographicSize;
    }
}

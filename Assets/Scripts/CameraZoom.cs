using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom: MonoBehaviour
{

    public float zoomSpeed = 5;
    public float smoothSpeed = 5.0f;
    public float minOrtho = 1.0f;
    public float maxOrtho = 20.0f;
    public float targetOrtho;


    private Vector3 dragOrigin;

    private bool isDragging = false;

    void Start()
    {
        targetOrtho = Camera.main.orthographicSize;
        //zoomSpeed = 5;
        //smoothSpeed = 5.0f;
        //minOrtho = 1.0f;
        //maxOrtho = 20.0f;
    }

    void Update()
    {

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }

        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);

    }

}
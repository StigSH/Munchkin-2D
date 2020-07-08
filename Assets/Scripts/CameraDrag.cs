using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{

    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    public bool cameraDragging = true;

    public float outerLeft = -10f;
    public float outerRight = 10f;
    public float outerUp = -10f;
    public float outerDown = 10f;

    void Update()
    {



        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        float left = Screen.width * 0.2f;
        float right = Screen.width - (Screen.width * 0.2f);
        float up = Screen.height * 0.2f;
        float down = Screen.height - (Screen.height * 0.2f);

        if (mousePosition.x < left && mousePosition.y < down)
        {
            cameraDragging = true;
        }
        else if (mousePosition.x > right && mousePosition.y > up)
        {
            cameraDragging = true;
        }

        if (cameraDragging)
        {

            if (Input.GetMouseButtonDown(1))
            {
                dragOrigin = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(1)) return;

            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            float moveX = pos.x * dragSpeed;
            float moveY = pos.y * dragSpeed * 0.5f;

            if(this.transform.position.x + moveX > outerRight || this.transform.position.x + moveX < outerLeft)
            {
                moveX = 0;
            }

            if (this.transform.position.y + moveY > outerUp || this.transform.position.y + moveY < outerDown)
            {
                moveY= 0;
            }


            transform.Translate(new Vector3(moveX,moveY,0), Space.World);
        }
    }
}
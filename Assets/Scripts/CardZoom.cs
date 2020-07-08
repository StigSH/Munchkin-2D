﻿using UnityEngine;

public class CardZoom : MonoBehaviour
{



    public GameObject Canvas;
    private GameObject zoomCard;


    public void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");


    }

    public void OnHoverEnter()
    {
        //Vector3 ZoomPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));

      
        zoomCard = Instantiate(gameObject,Camera.main.ViewportToWorldPoint(new Vector3(0.9f,0.3f,0)), Quaternion.identity);
        zoomCard.transform.position = new Vector3(zoomCard.transform.position.x, zoomCard.transform.position.y,0); //again we are adjusting so the object is in front of the camera.
        zoomCard.transform.SetParent(Canvas.transform, true);
        zoomCard.layer = LayerMask.NameToLayer("Zoom");
        
        RectTransform rect = zoomCard.GetComponent<RectTransform>();
        rect.localScale = new Vector2(1, 1);


    }

    public void OnHoverExit()
    {
            




        /*This is a bug fix
        The bug is that zoomCard and Canvas is reset, and therefor the game cant destroy the object.
        We know that a (clone)(clone) is created from the (clone) and we find that by looping through the canvas, since that is where we place zoom cards.
        This is quite expensive, so we only do this procedure when the bug has happened. Also in case we use Canvas to put other (clone)(clone)s then I make sure that it is in the Zoom Layer.
         */
        if (zoomCard == null)
        {
            if (Canvas == null)
            {
                Canvas = GameObject.Find("Main Canvas");
            }

            foreach (Transform t in Canvas.transform)
            {
                if (t.gameObject.name.Contains("(Clone)(Clone)") && t.gameObject.layer == LayerMask.NameToLayer("Zoom"))
                {
                    Destroy(t.gameObject);
                }

            }
        }
        else { 
            Destroy(zoomCard); 
        }

        //NetworkServer.Spawn(gameObject);
    }

}

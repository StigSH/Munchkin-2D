using UnityEngine;
using UnityEngine.UI;

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
        Transform tCardCount;
        int cardCnt;
        Text txt;
        string cardCntTxt;


        zoomCard = Instantiate(gameObject,Camera.current.ViewportToWorldPoint(new Vector3(0.9f,0.3f,0)), Quaternion.identity);
        zoomCard.transform.SetParent(Canvas.transform, true);
        zoomCard.transform.localRotation = Camera.current.transform.rotation;
        zoomCard.transform.position = new Vector3(zoomCard.transform.position.x, zoomCard.transform.position.y,0); //again we are adjusting so the object is in front of the camera.
        zoomCard.layer = LayerMask.NameToLayer("Zoom");

        if(gameObject.transform.parent.name.Contains("Pile"))
        {
            tCardCount = zoomCard.transform.Find("CardCount");
            txt = tCardCount.GetComponent<Text>();
            cardCnt = gameObject.transform.parent.childCount;
            if(cardCnt==1) 
            {
                cardCntTxt = cardCnt + " card";
            } else
            {
                cardCntTxt = cardCnt + " cards";
            }
            txt.text = cardCntTxt;
            tCardCount.gameObject.SetActive(true);

        }


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

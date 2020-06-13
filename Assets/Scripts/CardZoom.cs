using UnityEngine;

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
            
        zoomCard = Instantiate(gameObject, new Vector2(Input.mousePosition.x, Input.mousePosition.y + 250), Quaternion.identity);
        zoomCard.transform.SetParent(Canvas.transform, true);
            


        zoomCard.transform.localPosition = new Vector3(0, 0, 0);
        RectTransform rect = zoomCard.GetComponent<RectTransform>();
        rect.localScale = new Vector2(1, 1);

    }

    public void OnHoverExit()
    {
            
        Destroy(zoomCard);
        //NetworkServer.Spawn(gameObject);
    }

}

using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class CardZoom : MonoBehaviour
    {
        
        public GameObject Canvas;
        [SerializeField]
        private GameObject zoomCard;
        [SerializeField]
        private GameObject startParent;

        public void Awake()
        {
            Canvas = GameObject.Find("Main Canvas");

        }

        public void OnHoverEnter()
        {
            

            zoomCard = Instantiate(gameObject, new Vector2(Input.mousePosition.x, Input.mousePosition.y + 250),Quaternion.identity);
            zoomCard.transform.SetParent(Canvas.transform,true);



            zoomCard.transform.localPosition = new Vector3(0, 0, 0);
            RectTransform rect = zoomCard.GetComponent<RectTransform>();
            rect.localScale = new Vector2(1, 1);

        }

        public void OnHoverExit()
        {
            Destroy(zoomCard);
        }

    }
}

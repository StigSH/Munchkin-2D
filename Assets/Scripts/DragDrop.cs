using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class DragDrop : NetworkBehaviour
    {
        public GameObject Canvas;
        

        private bool isDragging = false;
        private bool isOverDropZone = false;
        private GameObject playerArea;
        private GameObject startParent;
        private Vector2 startPosition;


        private void Awake()
        {
            Canvas = GameObject.Find("Main Canvas");
        }

        void Update()   
        {

            if(isDragging)
            {
                transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                transform.SetParent(Canvas.transform,true);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            isOverDropZone = true;
            playerArea = collision.gameObject;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            isOverDropZone = false;
            playerArea = null;
        }

        public void StartDrag()
        {
            startParent = transform.parent.gameObject;
            startPosition = transform.position;
            isDragging = true;

        }

        public void EndDrag()
        {
            isDragging = false;
            if(isOverDropZone)
            {
                transform.SetParent(playerArea.transform, false);
            }
            else
            {
                transform.position = startPosition;
                transform.SetParent(startParent.transform, false);
            }
        }
    }
}

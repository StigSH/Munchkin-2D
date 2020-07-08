using Mirror;
using Mirror.Examples.Basic;
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
        public GameObject PlayerArea;
        public PlayerManager playerManager;

        private bool isDragging = false;
        private bool isOverDropZone = false;
        private bool isDraggable = true;
        private GameObject playerArea;
        private GameObject startParent;
        private Vector2 startPosition;


        private void Start()
        {

            Canvas = GameObject.Find("Main Canvas");
            PlayerArea = GameObject.Find("PlayerArea");

            if (gameObject.name.Contains("(Clone)(Clone")) return;
            if (!hasAuthority)
            {
                isDraggable = false;
            }
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
            if (!isDraggable) return; 
            startParent = transform.parent.gameObject;
            startPosition = transform.position;
            isDragging = true;

        }

        public void EndDrag()
        {
            if (!isDraggable) return;
            isDragging = false;
            if(isOverDropZone)
            {
                transform.SetParent(playerArea.transform, false);
                isDraggable = false;
                NetworkIdentity networkIdentity = NetworkClient.connection.identity;
                playerManager = networkIdentity.GetComponent<PlayerManager>();

                playerManager.PlayCard(gameObject);

            }
            else
            {
                transform.position = startPosition;
                transform.SetParent(startParent.transform, false);
            }
        }
    }
}

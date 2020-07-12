using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.Basic;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class PlayerManager : NetworkBehaviour
{

    public GameObject MainCanvas;
    public GameObject CardTemplate;
    public GameObject CardManager;
    public CardListManager cardListManager;

    [SyncVar]
    public int PlayerNum;


    List<GameObject> cards = new List<GameObject>();

    GameObject NewPlayer;



    public override void OnStartClient()
    {
        base.OnStartClient();

        MainCanvas = GameObject.Find("Main Canvas");
        gameObject.transform.SetParent(MainCanvas.transform, true);
        if (NetworkServer.connections.Count > 0) PlayerNum = NetworkServer.connections.Count;

        if(NetworkServer.connections.Count == 1)
        {
            gameObject.transform.localPosition = new Vector3(0, -260, 0);
        }
        else if(NetworkServer.connections.Count == 2)
        {
            gameObject.transform.localPosition = new Vector3(0, 280, 0);
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,180));
        }
        else if (NetworkServer.connections.Count == 3)
        {
            gameObject.transform.localPosition = new Vector3(-240,0 , 0);
            gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        else if (NetworkServer.connections.Count == 4)
        {
            gameObject.transform.localPosition = new Vector3(240, 0, 0);
            gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        //player 2: x = 0, y = 295, zrotation = 180 
        //player 3: x = -250, y = 0, zrotation = -90
        //player 4: x= 250 y = 0, zrotation = 90

        RpcTurnOnCamera(gameObject);

       
        //ClientScene.RegisterSpawnHandler()



    }

    [Server]
    public override void OnStartServer()
    {

        //from tutorial
        //cards.Add(Card1);
        //cards.Add(Card2);
        

        //Test add cards
        cardListManager = CardManager.GetComponent<CardListManager>();

    }

    [Command]
    public void CmdDealCards()
    {


        //Deal 4 treasure cards and 4 door cards



        for (int i = 0; i < 4; i++)
        {
            //Using ScriptableObject Database
            GameObject card = Instantiate(CardTemplate);
            int cardInt = Random.Range(0, cardListManager.CardList.Count);

            card.GetComponent<CardViz>().card = cardListManager.CardList[cardInt]; //Changing card in cardviz here only works on host
            CardTemplate.transform.localScale = new Vector2(0.5f, 0.5f);

            NetworkServer.Spawn(card, connectionToClient);
            RpcLoadCard(card, cardInt); //We use Rpc in order to make this work on clients as well
            RpcShowCard(card, "Dealt");


            //    //From tutorial
            //    //GameObject card = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            //    //card.transform.localScale = new Vector2(0.5f, 0.5f);
            //    //NetworkServer.Spawn(card, connectionToClient);
            //    //RpcShowCard(card, "Dealt");

            }
        }

    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
    }

    [Command]
    void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card,"Played");
    }


    [ClientRpc] 
    void RpcShowCard(GameObject card, string type)
    {
        //if (type == "Dealt")
        //{
        //    if (hasAuthority)
        //    {
        //        card.transform.SetParent(PlayerHand.transform, false);
        //    }
        //    else
        //    {
        //        card.transform.SetParent(EnemyHand.transform, false);
        //        card.GetComponent<CardFlipper>().Flip();
        //    }
        //}
        //else if (type == "Played")
        //{
        //    if(hasAuthority)
        //    {
        //        card.transform.SetParent(PlayerArea.transform, false);
        //    }
        //    else
        //    {
        //        card.transform.SetParent(EnemyArea.transform, false);
        //        card.GetComponent<CardFlipper>().Flip();
        //    }

            
        //}

    }

    [ClientRpc]
    void RpcLoadCard(GameObject card, int cardInt)
    {
        card.GetComponent<CardManagerScript>().LoadCardFromCardList(card, cardInt);
    }


    [ClientRpc]
    void RpcTurnOnCamera (GameObject NewPlayer)
    {
        if(hasAuthority)
        {
            NewPlayer.GetComponentInChildren<Camera>().enabled = true;
        }
        else
        {
            NewPlayer.GetComponentInChildren<Camera>().enabled = false;
        }
    }

}

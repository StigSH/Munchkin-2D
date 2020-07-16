using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.Basic;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Globalization;


public class PlayerManager : NetworkBehaviour
{

    public GameObject MainCanvas;
    public GameObject CardTemplate;
    public GameObject CardManager;
    public CardListManager cardListManager;

    [SyncVar]
    public int PlayerNum;

    //List<GameObject> cards = new List<GameObject>();




    public override void OnStartClient()
    {
        base.OnStartClient();

        MainCanvas = GameObject.Find("Main Canvas");
        
        gameObject.transform.SetParent(MainCanvas.transform, true);
        if (GameObject.FindGameObjectsWithTag("Player").Count<GameObject>() > 0) PlayerNum = GameObject.FindGameObjectsWithTag("Player").Count<GameObject>();
        gameObject.GetComponent<PlayerManager>().PlayerNum = PlayerNum;



        CmdSetPlayerPosition(gameObject, PlayerNum);
        CmdTurnOnCamera(gameObject);

        //player 2: x = 0, y = 295, zrotation = 180 
        //player 3: x = -250, y = 0, zrotation = -90
        //player 4: x= 250 y = 0, zrotation = 90



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
        int CardsDealt = 0;
        List<int> InitList = new List<int>();
        GameObject [] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player;
        List<Card> CardList = cardListManager.TreasureCardList;
        List<Card> FisherYates = cardListManager.FisherYates;

        //Clear fisher yates
        while(FisherYates.Count > 0)
        {
            Debug.Log(FisherYates.Count);
            FisherYates.RemoveAt(0);
        }

        //create the list that we can delete from
        for (int i = 0; i<CardList.Count; i++)
        {
            InitList.Add(i);
        }

        while(InitList.Count>0)
        {
            int rnd = Random.Range(0, InitList.Count);
            FisherYates.Add(CardList[InitList[rnd]]);
            InitList.RemoveAt(rnd);
        }


        //Deal 4 treasure cards to each player and then rest to card pile
        for(int i = 0; i < FisherYates.Count; i++)
        {
            //Using ScriptableObject Database
            GameObject card = Instantiate(CardTemplate);

            
            card.GetComponent<CardViz>().card = FisherYates[i]; //Changing card in cardviz here only works on host
            Debug.Log("Card on local: " + card.GetComponent<CardViz>().card);
            CardTemplate.transform.localScale = new Vector2(0.5f, 0.5f);

            NetworkServer.Spawn(card, connectionToClient);
            
            RpcLoadCard(card, i); //We use Rpc in order to make this work on clients as well
            if (CardsDealt / NetworkServer.connections.Count <= 4)
            {

                player = players[CardsDealt % NetworkServer.connections.Count];
                RpcSetParent(card, player, "Hand");
            }
                else
            {
                RpcSetParent(card, MainCanvas, "TreasureCardPile");
            }


        CardsDealt += 1;

        }

        //for (int i = 0; i < 4; i++)
        //{
        //    //Using ScriptableObject Database
        //    GameObject card = Instantiate(CardTemplate);
        //    int cardInt = Random.Range(0, cardListManager.TreasureCardList.Count);

        //    card.GetComponent<CardViz>().card = cardListManager.TreasureCardList[cardInt]; //Changing card in cardviz here only works on host

        //    CardTemplate.transform.localScale = new Vector2(0.5f, 0.5f);

        //    NetworkServer.Spawn(card, connectionToClient);
        //    RpcLoadCard(card, cardInt); //We use Rpc in order to make this work on clients as well
        //    RpcShowCard(card, "Dealt",-1);


        //    //    //From tutorial
        //    //    //GameObject card = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
        //    //    //card.transform.localScale = new Vector2(0.5f, 0.5f);
        //    //    //NetworkServer.Spawn(card, connectionToClient);
        //    //    //RpcShowCard(card, "Dealt");

        //    }
    }




    [ClientRpc]
    void RpcSetParent(GameObject card, GameObject parent,string type)
    {
        if(type=="Hand")
        {
            card.transform.SetParent(parent.transform.Find("PlayerHand"),false);
            if (NetworkClient.connection.identity.NetworkBehaviours[0].GetComponent<PlayerManager>().PlayerNum != parent.GetComponent<PlayerManager>().PlayerNum)
            {
                //card.GetComponent<CardFlipper>().Flip();
            }
            
        }
        if(type == "TreasureCardPile")
        {
            card.transform.SetParent(MainCanvas.transform.Find("TreasureCardPile"), false);
            card.GetComponent<CardFlipper>().Flip();
        }
    }


    [ClientRpc]
    void RpcLoadCard(GameObject card, int cardInt)
    {
        Debug.Log("Card  before load: " + card.GetComponent<CardViz>().card);
        card.GetComponent<CardManagerScript>().LoadCardFromCardList(card, cardInt);
        Debug.Log("Card  after load: " + card.GetComponent<CardViz>().card);
    }

    [Command]
    public void CmdTurnOnCamera(GameObject player)
    {
        RpcTurnOnCamera(player);
    }

    [ClientRpc]
    void RpcTurnOnCamera (GameObject player)
    {
        if(hasAuthority)
        {
            player.GetComponentInChildren<Camera>().enabled = true;
            MainCanvas.GetComponent<Canvas>().worldCamera = gameObject.GetComponentInChildren<Camera>();
        }
        else
        {
            player.GetComponentInChildren<Camera>().enabled = false;
        }
    }

    [Command]
    void CmdSetPlayerPosition(GameObject player, int PlayerPos)
    {
        RpcSetPlayerPosition(player, PlayerPos);
    }

    [ClientRpc]
    void RpcSetPlayerPosition(GameObject player, int PlayerPos)
    {


        if (PlayerPos == 1)
        {
            player.transform.localPosition = new Vector3(0, -306, 0);
        }
        else if (PlayerPos == 2)
        {

            player.transform.localPosition = new Vector3(0, 306, 0);
            
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if (PlayerPos == 3)
        {
            player.transform.localPosition = new Vector3(-255, 0, 0);
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        else if (PlayerPos == 4)
        {
            player.transform.localPosition = new Vector3(250, 0, 0);
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
    }
}

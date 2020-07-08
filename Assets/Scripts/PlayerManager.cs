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
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerHand;
    public GameObject EnemyHand;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject MainCanvas;
    public GameObject CardTemplate;
    public GameObject CardManager;

    List<GameObject> cards = new List<GameObject>();
    public CardListManager cardListManager;

    public override void OnStartClient()
    {
        base.OnStartClient();

        //ClientScene.RegisterSpawnHandler()

        PlayerHand = GameObject.Find("PlayerHand");
        EnemyHand = GameObject.Find("EnemyHand");
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");

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
        if (type == "Dealt")
        {
            if (hasAuthority)
            {
                card.transform.SetParent(PlayerHand.transform, false);
            }
            else
            {
                card.transform.SetParent(EnemyHand.transform, false);
                card.GetComponent<CardFlipper>().Flip();
            }
        }
        else if (type == "Played")
        {
            if(hasAuthority)
            {
                card.transform.SetParent(PlayerArea.transform, false);
            }
            else
            {
                card.transform.SetParent(EnemyArea.transform, false);
                card.GetComponent<CardFlipper>().Flip();
            }

            
        }

    }

    [ClientRpc]
    void RpcLoadCard(GameObject card, int cardInt)
    {
        card.GetComponent<CardManagerScript>().LoadCardFromCardList(card, cardInt);
    }




}

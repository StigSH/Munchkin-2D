using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.Basic;
using UnityEngine.UIElements;

public class PlayerManager : NetworkBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerHand;
    public GameObject EnemyHand;
    public GameObject PlayerArea;
    public GameObject MainCanvas;

    List<GameObject> cards = new List<GameObject>();


    public override void OnStartClient()
    {
        base.OnStartClient();

        //ClientScene.RegisterSpawnHandler()

        PlayerHand = GameObject.Find("PlayerHand");
        EnemyHand = GameObject.Find("EnemyHand");
        PlayerArea = GameObject.Find("PlayerArea");

    }

    [Server]
    public override void OnStartServer()
    {
        cards.Add(Card1);
        cards.Add(Card2);

    }

    [Command]
    public void CmdDealCards()
    {


        //Deal 4 treasure cards and 4 door cards
        for (int i = 0; i < 4; i++)
        {
            GameObject card = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            card.transform.localScale = new Vector2(0.5f, 0.5f);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");

        }
    }





    [ClientRpc] 
    void RpcShowCard(GameObject card, string type)
    {
        if (type == "Dealt")
        {
            if(hasAuthority)
            {
                card.transform.SetParent(PlayerHand.transform, false);
            }
            else
            {
                card.transform.SetParent(EnemyHand.transform, false);
            }
        }

    }




}

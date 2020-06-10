using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerHand;
    public GameObject EnemyHand;

    List<GameObject> cards = new List<GameObject>();

    void Start()
    {
        cards.Add(Card1);
        cards.Add(Card2);
    }

    public void OnClick()
    {

        //Deal 4 treasure cards and 4 door cards
        for (int i = 0; i < 4; i ++) {
            GameObject playerCard = Instantiate(cards[Random.Range(0,cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.transform.SetParent(PlayerHand.transform, false);
            playerCard.transform.localScale = new Vector3(0.5f, 0.5f,1);

            GameObject enemyCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            enemyCard.transform.SetParent(EnemyHand.transform, false);
            enemyCard.transform.localScale = new Vector3(0.5f, 0.5f, 1);



        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManagerScript : MonoBehaviour
{
    public GameObject CardManager;

    public void LoadCardFromCardList(GameObject card, int CardInt)
    {
        CardListManager cardListManager = CardManager.GetComponent<CardListManager>();

        card.GetComponent<CardViz>().card = cardListManager.CardList[CardInt];
    }

    
}

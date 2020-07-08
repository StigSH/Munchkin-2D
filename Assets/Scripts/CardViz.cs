using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardViz : MonoBehaviour
{
    public Card card;
    public Text Title;
    public Image Img;


    private void Start()
    {
        LoadCard(card);
        
    }



    public void LoadCard(Card c)
    {
        if (c == null) return;

        card = c;

        Img.sprite = c.Img;
        Title.text = c.CardName;

    }

    
}

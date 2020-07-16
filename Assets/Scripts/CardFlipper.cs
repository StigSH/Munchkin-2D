using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{
    public void Flip()
    {
        foreach(Transform child in transform)
        {
            if(child.gameObject.name != "Back" && child.gameObject.name != "CardCount")
            {
                if(child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
                
            }
            
        }


    }
}

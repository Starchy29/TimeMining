using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            
            if (collision.gameObject.GetComponent<CharacterController>() != null)
                collision.gameObject.GetComponent<CharacterController>().NearOven(true);
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(collision.gameObject.GetComponent<CharacterController>()!= null)
            collision.gameObject.GetComponent<CharacterController>().NearOven(false);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

   
}

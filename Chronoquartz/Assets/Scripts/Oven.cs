using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent.gameObject.GetComponent<CharacterController>().NearOven(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.transform.parent.gameObject.GetComponent<CharacterController>().NearOven(false);
    }

    public void ButtonClicked(this GameObject button)
    {
        Debug.Log("Button name is " + button.name);
    }
}

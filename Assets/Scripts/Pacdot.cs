using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacdot : MonoBehaviour
{
    // Start is called before the first frame update

    public bool issuperPacdot = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name=="Pacman")
        {
            if(issuperPacdot)
            {
                GameManager.Instance.OnEatPacdot(gameObject);
                GameManager.Instance.OnEatSuperPacdot();
                Destroy(gameObject);
            }
            else
            {
                GameManager.Instance.OnEatPacdot(gameObject);
                Destroy(gameObject);
            }
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAlive = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        changeColour();
    }

    private void changeColour()
    {
        if (isAlive)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}

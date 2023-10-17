using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState
{
    Alive,
    Dead
};
public class Cell : MonoBehaviour
{
    private CellState state;
    private GameObject[] neighbours;
    // Start is called before the first frame update
    
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
        if (state == CellState.Alive)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void setState(CellState newState)
    {
        state = newState;
    }

    public CellState getState()
    {
        return state;
    }

    public void setNeighbours(GameObject[] neighbours)
    {

    }
}

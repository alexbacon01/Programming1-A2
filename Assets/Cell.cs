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
    private int colPos;
    private int rowPos;
    private int aliveNeighbours;
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        changeColour();
        setAliveNeighbours();

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

    public void setNeighbours(GameObject[] cellNeighbours)
    {
        neighbours = cellNeighbours;
    }
    public GameObject[] getNeighbours()
    {
        return neighbours;
    }
    public void setPos(int col, int row)
    {
        colPos = col;
        rowPos = row;
    }

    public int getRowPos()
    {
        return rowPos;
    }
    public int getColPos()
    {
        return colPos;
    }

    public void setAliveNeighbours()
    {
        int numOfAliveNeighbours = 0;
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i].GetComponent<Cell>().getState() == CellState.Alive)
            {
                numOfAliveNeighbours++;
            }
        }
    }

    public int getAliveNeighbours()
    {
        return aliveNeighbours;
    }
}

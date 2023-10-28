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
    private int numCalled = 0;


    void Start()
    {
    }

    void Update()
    {
        changeColour();
    }

    /* method to change colour in sprite renderer dependent on cell state */
    private void changeColour()
    {
        if (state == CellState.Alive)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.black; //black if alive
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white; //white if dead
        }
    }

    /* public method to set cell state */
    public void setState(CellState newState)
    {
        state = newState; //sets state to new state defined in parameters
    }
    /* public method to get current state of cell */
    public CellState getState()
    {
        return state;
    }

    /* set neighbours of cell*/
    public void setNeighbours(GameObject[] cellNeighbours)
    {
        neighbours = cellNeighbours;
    }

    /* get neighbours of cell */
    public GameObject[] getNeighbours()
    {
        return neighbours;
    }

    /* set pos of cell in form of col and row */
    public void setPos(int col, int row)
    {
        colPos = col;
        rowPos = row;
    }

    /* get the row position of cell */
    public int getRowPos()
    {
        return rowPos;
    }

    /*get col pos of cell */
    public int getColPos()
    {
        return colPos;
    }

    /* set the num of alive neighbours for cell */
    public void setAliveNeighbours()
    {
        aliveNeighbours = 0;

        for (int i = 0; i < neighbours.Length; i++) //goes through the array of neighbours and counts how many are alive
        {
            if (neighbours[i].GetComponent<Cell>().getState() == CellState.Alive && aliveNeighbours < neighbours.Length)
            {
                aliveNeighbours++;
            }
        }
        Debug.Log("num called" + numCalled);
        numCalled++;
    }

    /* get num of alive neighbours */
    public int getAliveNeighbours()
    {
        return aliveNeighbours;
    }
}

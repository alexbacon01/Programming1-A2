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
        aliveNeighbours = 0;

        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i].GetComponent<Cell>().getState() == CellState.Alive && aliveNeighbours < neighbours.Length)
            {
                aliveNeighbours++;
            }
        }
        Debug.Log("num called" + numCalled);
        numCalled++;
    }

    public int getAliveNeighbours()
    {
        return aliveNeighbours;
    }
}

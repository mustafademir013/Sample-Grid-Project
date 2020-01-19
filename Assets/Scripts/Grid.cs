using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridPosition
{
    public int row;
    public int col;
}
public class Grid : MonoBehaviour
{
    // Start is called before the first frame update

    private GridPosition gridPosition;

    void Start()
    {

    }

    public void SetGridPosition(int row, int col)
    {
        gridPosition.row = row;
        gridPosition.col = col;
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
   
    void OnMouseDown()
    {
        Manager.Instance.CheckContent(gridPosition);
    }


}

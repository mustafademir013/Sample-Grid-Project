using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Manager : MonoBehaviour
{

    public static Manager Instance { private set; get; }
    // Start is called before the first frame update
    private GameObject[,] gridObjects;

    private GameObject[,] contentObjects;

    private int matchCount;

    [SerializeField] GameObject gridObject;
    [SerializeField] GameObject contentObject;
    [SerializeField] GameObject gridParentObject;

    void Start()
    {
        if (Instance == null)
            Instance = this;
    }
    
    ///////////////public Methods/////////////////////////////// 
     
  
    public void SpawnGrids(int gridCount)
    {
        matchCount = 0;
        UIManager.Instance.SetMatchText(matchCount);
        DeleteGrids();
        float width = Camera.main.orthographicSize * Camera.main.aspect;
        float gridScale = (width * 2) / gridCount;
        gridObject.transform.localScale = new Vector3(gridScale, gridScale, gridScale);
        float firstPositionX = -width + (gridScale / 2);
        float firstPositionY = Camera.main.orthographicSize - (gridScale / 2);
        gridObjects = new GameObject[gridCount, gridCount];
        contentObjects = new GameObject[gridCount, gridCount];
        for (int i = 0; i < gridCount; i++)
        {
            for (int j = 0; j < gridCount; j++)
            {
                Vector3 position = new Vector3(firstPositionX, firstPositionY, 0);
                gridObjects[i, j] = Instantiate(gridObject, position, Quaternion.identity);
                gridObjects[i, j].transform.parent = gridParentObject.transform;
                gridObjects[i, j].GetComponent<Grid>().SetGridPosition(i, j);
                firstPositionX += gridScale;
            }
            firstPositionX = -width + (gridScale / 2);
            firstPositionY -= gridScale;
        }
    }
    public void CheckContent(GridPosition gridPosition)
    {
        ArrayList filledGridPositions = new ArrayList();
        filledGridPositions.Add(gridPosition);

        int counter = 0;


        while (counter < filledGridPositions.Count)
        {
            GridPosition pivotPosition = (GridPosition)filledGridPositions[counter];
            GridPosition[] neighborPositions = GetNeighborGrid(pivotPosition);
            foreach (var item in neighborPositions)
            {
                try
                {
                    if (contentObjects[item.row, item.col] != null)
                        AddObjectToFilledList(item, filledGridPositions);
                }
                catch (System.Exception ex)
                {
                }
            }
            counter++;
        }
        if (filledGridPositions.Count > 2)
        {
            DestroyContentObjects(filledGridPositions);
            matchCount++;
            UIManager.Instance.SetMatchText(matchCount);
        }
        else
            SpawnContent(gridPosition);
    }

    
    /// ///////////////////////private methods///////////////////////
   

    private void DeleteGrids()
    {
        if (gridObjects == null)
        {
            return;
        }
        for (int i = 0; i < gridObjects.GetLength(0); i++)
            for (int j = 0; j < gridObjects.GetLength(0); j++)
            {
                if (contentObjects[i, j] != null)
                {
                    Destroy(contentObjects[i, j]);
                }
                Destroy(gridObjects[i, j]);
            }
    }

    private void SpawnContent(GridPosition gridPosition)
    {
        int row = gridPosition.row;
        int col = gridPosition.col;

        if (contentObjects[row, col] != null)
        {
            return;
        }
        contentObject.transform.localScale = gridObject.transform.localScale;
        Vector3 spawnPosition = gridObjects[row, col].transform.position;
        contentObjects[row, col] = Instantiate(contentObject, spawnPosition, Quaternion.identity);
        contentObjects[row, col].transform.parent = gridObjects[row, col].transform;
    }
    private void AddObjectToFilledList(GridPosition gridPosition, ArrayList filledList)
    {
        int counter = 0;
        foreach (var item in filledList)
        {
            GridPosition pos = (GridPosition)item;
            if (pos.row == gridPosition.row && pos.col == gridPosition.col)
                counter++;
        }
        if (counter == 0)
            filledList.Add(gridPosition);
    }

    private void DestroyContentObjects(ArrayList contentPositions)
    {
        foreach (var item in contentPositions)
        {
            GridPosition pos = (GridPosition)item;
            if (contentObjects[pos.row, pos.col] != null)
            {
                Destroy(contentObjects[pos.row, pos.col]);
                contentObjects[pos.row, pos.col] = null;
            }
        }
    }

    private GridPosition[] GetNeighborGrid(GridPosition gridPosition)
    {
        GridPosition[] neighborPositions = new GridPosition[4];

        neighborPositions[0].row = gridPosition.row;
        neighborPositions[0].col = gridPosition.col - 1;

        neighborPositions[1].row = gridPosition.row;
        neighborPositions[1].col = gridPosition.col + 1;

        neighborPositions[2].row = gridPosition.row - 1;
        neighborPositions[2].col = gridPosition.col;

        neighborPositions[3].row = gridPosition.row + 1;
        neighborPositions[3].col = gridPosition.col;

        return neighborPositions;
    }

}

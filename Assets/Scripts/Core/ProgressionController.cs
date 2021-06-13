using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionController : MonoBehaviour
{
    
    [SerializeField] LevelLayout levelLayout;
    [SerializeField] Vector2Int gridSize ;

    private GenericGrid<(int, bool)> _levelGrid;

    void Start()
    {
        PopulateGrid();
    }

    private void PopulateGrid(){

        _levelGrid = new GenericGrid<(int, bool)>();

        int w = levelLayout.levelGrid.GridSize.x;
        int h = levelLayout.levelGrid.GridSize.y;

        for(int x = 0; x < w; x++){
            for(int y = 0; y < h; y++){
                _levelGrid[x, y] = (levelLayout.levelGrid.GetCell(x, y), false);
            }
        }
    }

    public void SetGridAssetSize(){
        levelLayout.levelGrid.UpdateGridSize(gridSize);
    }

    private bool IncludeInGrid(int i){
        return i > 0;
    }
}

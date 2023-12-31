using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex=-1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData gridData;
    ObjectPlacer objectPlacer;
    
    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData gridData, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if(gridData.canPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = gridData;
        }

        if(selectedData == null)
        {
            return;
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if(gameObjectIndex == -1)
            {
                return;
            }
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
        Vector3 offset = new Vector3(2.5f, 0f, 2.5f);
        Vector3 cellPosition = grid.CellToWorld(gridPosition)+offset;
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !gridData.canPlaceObjectAt(gridPosition, Vector2Int.one);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        Vector3 offset = new Vector3(2.5f, 0f, 2.5f);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition)+offset, CheckIfSelectionIsValid(gridPosition));
    }
}

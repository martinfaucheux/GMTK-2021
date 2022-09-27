using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixCollider : MonoBehaviour
{

    public Vector2Int matrixPosition;
    private CollisionMatrix _collisionMatrix;

    // Use this for initialization
    void Start()
    {
        _collisionMatrix = CollisionMatrix.instance;
        matrixPosition = _collisionMatrix.GetMatrixPos(this.transform);
        _collisionMatrix.AddCollider(this);
    }

    private void OnDestroy()
    {
        Unregister();
    }

    public Vector3 GetRealPos()
    {
        return _collisionMatrix.GetRealWorldPosition(matrixPosition);
    }

    public bool IsValidDirection(Direction direction)
    {
        Vector2Int futureMatrixPosition = matrixPosition + direction.ToPos();
        return _collisionMatrix.IsValidPosition(futureMatrixPosition);
    }

    public GameObject GetObjectInDirection(Direction direction)
    {
        Vector2Int positionToCheck = matrixPosition + direction.ToPos();

        if (!_collisionMatrix.IsValidPosition(positionToCheck))
            return null;

        return _collisionMatrix.GetObjectAtPosition(positionToCheck);
    }

    public Vector2Int GetMaxInLinePosition(Direction direction)
    {

        Vector2Int maxInlinePosition = matrixPosition;
        Vector2Int positionToCheck = matrixPosition + direction.ToPos();

        while (_collisionMatrix.IsValidPosition(positionToCheck))
        {
            GameObject objectAtPosition = _collisionMatrix.GetObjectAtPosition(positionToCheck);
            if (objectAtPosition != null)
                break;

            maxInlinePosition = positionToCheck;
            positionToCheck += direction.ToPos();
        }
        return maxInlinePosition;
    }


    public Direction GetDirectionToOtherCollider(MatrixCollider otherCollider)
    {
        Vector2Int posDiff = otherCollider.matrixPosition - this.matrixPosition;
        return Direction.GetDirection2ValueFromCoord(posDiff.x, posDiff.y);
    }

    public List<GameObject> GetNeighborObjects()
    {
        List<GameObject> result = new List<GameObject>();
        foreach (Direction direction in Direction.GetAll<Direction>())
        {
            if (direction == Direction.IDLE)
                continue;
            GameObject neighborObject = GetObjectInDirection(direction);
            if (neighborObject != null)
                result.Add(neighborObject);
        }
        return result;
    }

    public void Unregister()
    {
        if (_collisionMatrix != null)
            _collisionMatrix.RemoveCollider(this);
    }


}

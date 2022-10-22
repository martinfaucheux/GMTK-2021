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

    public List<GameObject> GetObjectsInDirection(Direction direction)
    {
        Vector2Int positionToCheck = matrixPosition + direction.ToPos();

        if (!_collisionMatrix.IsValidPosition(positionToCheck))
            return new List<GameObject>();

        return _collisionMatrix.GetObjectsAtPosition(positionToCheck);
    }

    public Vector2Int GetMaxInLinePosition(Direction direction)
    {

        Vector2Int maxInlinePosition = matrixPosition;
        Vector2Int positionToCheck = matrixPosition + direction.ToPos();

        while (_collisionMatrix.IsValidPosition(positionToCheck))
        {
            List<GameObject> objectsAtPosition = _collisionMatrix.GetObjectsAtPosition(positionToCheck);
            if (objectsAtPosition.Count > 0)
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
            foreach (GameObject neighborObject in GetObjectsInDirection(direction))
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

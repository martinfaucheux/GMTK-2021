using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionMatrix : SingletonBase<CollisionMatrix>
{
    public Vector2Int matrixSize;
    public Vector3 origin;

    public bool showSceneBounds = true;
    public Color sceneBoundsColor;
    public GameObject borderWallPrefab;
    public GameObject GridUnitPrefab;

    private List<MatrixCollider> colliderList = new List<MatrixCollider>();

    public int maxDistance
    {
        get { return Mathf.Max(matrixSize.x, matrixSize.y); }
    }

    public void AddCollider(MatrixCollider collider)
    {
        colliderList.Add(collider);
    }

    public void RemoveCollider(MatrixCollider collider)
    {
        colliderList.Remove(collider);
    }


    // get the first object found at the given position
    public GameObject GetObjectAtPosition(Vector2Int matrixPosition)
    {
        foreach (MatrixCollider collider in colliderList)
        {
            if (collider.matrixPosition == matrixPosition)
                return collider.gameObject;
        }

        return null;
    }

    public List<GameObject> GetObjectsAtPosition(Vector2Int matrixPosition)
    {
        List<GameObject> result = new List<GameObject>();
        foreach (MatrixCollider collider in colliderList)
        {
            if (collider.matrixPosition == matrixPosition)
                result.Add(collider.gameObject);
        }
        return result;
    }

    public bool IsValidPosition(Vector2Int matrixPosition)
    {
        int x = matrixPosition.x;
        int y = matrixPosition.y;

        int xMax = matrixSize.x;
        int yMax = matrixSize.y;

        return ((x >= 0) & (y >= 0) & (x < xMax) & (y < yMax));
    }

    public Vector2Int GetMatrixPos(Transform transform)
    {
        Vector3 realPos = transform.position - origin;
        float x = realPos.x;
        float y = realPos.y;
        return new Vector2Int((int)x, (int)y);
    }

    public void CenterOrigin()
    {
        Vector3 newOrigin = -(Vector2)matrixSize / 2f;
        origin = newOrigin + new Vector3(0.5f, 0.5f, 0f);

    }

    public Vector3 GetRealWorldPosition(Vector2Int matrixPos)
    {
        float x = matrixPos.x;
        float y = matrixPos.y;
        Vector3 realWorldPos;
        realWorldPos = new Vector3(x, y, 0);
        return origin + realWorldPos;
    }

    public Vector3 GetRealWorldVector(Direction direction)
    {
        return (Vector2)direction.ToPos();
    }

}

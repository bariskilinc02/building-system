using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridGround : GridBase
{
    public Dictionary<VectorEdge, Edge> edges;

    protected override void Start()
    {
        base.Start();

        CreateEdges();
    }

    public void CreateEdges()
    {
        edges = new Dictionary<VectorEdge, Edge>();

        for (int i = 0; i < gridSize.x + 1; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                edges.Add(new VectorEdge(new Vector2Int(i, j), new Vector2Int(i, j + 1)), new Edge());
            }
        }

        for (int j = 0; j < gridSize.y + 1; j++)
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                edges.Add(new VectorEdge(new Vector2Int(i, j), new Vector2Int(i + 1, j)), new Edge());
            }
        }
    }

    public VectorEdge GetEdgeVector(Vector3 position)
    {
        var actualCoordinate = GetCellCoordinate(position);
        var cellCenterCoordinate = actualCoordinate + new Vector2(0.5f, 0.5f);

        var direction = new Vector2(position.x, position.z) - cellCenterCoordinate;
        float angle = Vector2.SignedAngle(Vector2.right, direction);

        if (angle > -45f && angle < 45f)
        {
            return new VectorEdge(new Vector2Int(1, 0) + actualCoordinate, new Vector2Int(1, 1) + actualCoordinate);
        }
        else if (angle > 45f && angle < 135f)
        {
            return new VectorEdge(new Vector2Int(0, 1) + actualCoordinate, new Vector2Int(1, 1) + actualCoordinate);
        }
        else if (angle > -135f && angle < -45f)
        {
            return new VectorEdge(new Vector2Int(0, 0) + actualCoordinate, new Vector2Int(1, 0) + actualCoordinate);
        }
        else //((angle > 135f && angle <= 180) || (angle >= -180f && angle < -135f))
        {
            return new VectorEdge(new Vector2Int(0, 0) + actualCoordinate, new Vector2Int(0, 1) + actualCoordinate);
        }
    }

    public Vector3 GetCornerWorldPosition(Vector2Int cornerCoordinate)
    {
        Vector3 result = new Vector3(cornerCoordinate.x + _rootPosition.x, _rootPosition.y,
            cornerCoordinate.y + _rootPosition.z);
        return result;
    }

    public Vector3 GetEdgeWorldPosition(VectorEdge edgeVector)
    {
        Vector2 edgeMiddlePoint = edgeVector.GetMiddlePoint();
        return new Vector3(edgeMiddlePoint.x, 0, edgeMiddlePoint.y) + _rootPosition;
    }

    public Vector2Int GetEdgeVectorPoint(Vector3 position)
    {
        var actualCoordinate = GetCellCoordinate(position);
        var worldCoordinate = GetCornerWorldPosition(actualCoordinate);
        var cellCenterCoordinate = actualCoordinate + new Vector2(0.5f, 0.5f);

        var direction = new Vector2(position.x, position.z) - new Vector2(worldCoordinate.x, worldCoordinate.z) -
                        new Vector2(0.5f, 0.5f);
        float angle = Vector2.SignedAngle(Vector2.right, direction);

        if (angle is > 0f and < 90f)
        {
            return
                new Vector2Int(1, 1) +
                actualCoordinate; //new VectorEdge(new Vector2Int(1, 0) + actualCoordinate, new Vector2Int(1, 1) + actualCoordinate);
        }
        else if (angle is > 90f and < 180f)
        {
            return
                new Vector2Int(0, 1) +
                actualCoordinate; //new VectorEdge(new Vector2Int(0, 1) + actualCoordinate, new Vector2Int(1, 1) + actualCoordinate);
        }
        else if (angle is > -90f and < 0f)
        {
            return
                new Vector2Int(1, 0) +
                actualCoordinate; //new VectorEdge(new Vector2Int(0, 0) + actualCoordinate, new Vector2Int(1, 0) + actualCoordinate);
        }
        else //((angle > 135f && angle <= 180) || (angle >= -180f && angle < -135f))
        {
            return
                new Vector2Int(0, 0) +
                actualCoordinate; //new VectorEdge(new Vector2Int(0, 0) + actualCoordinate, new Vector2Int(0, 1) + actualCoordinate);
        }
    }

    //Returns edge vector list in a line with given length and direction
    public List<VectorEdge> GetEdgeVectorsInRange(Vector2Int startPoint, VectorDirection direction, int length)
    {
        List<VectorEdge> edgeVectors = new List<VectorEdge>();

        Vector2Int startVector = startPoint;
        Vector2Int additiveVector = new Vector2Int();

        switch (direction)
        {
            case VectorDirection.Top:
                additiveVector = new Vector2Int(0, 1);
                break;
            case VectorDirection.Right:
                additiveVector = new Vector2Int(1, 0);
                break;
            case VectorDirection.Bottom:
                additiveVector = new Vector2Int(0, -1);
                break;
            case VectorDirection.Left:
                additiveVector = new Vector2Int(-1, 0);
                break;
        }

        for (int i = 0; i < length; i++)
        {
            edgeVectors.Add(new VectorEdge(startVector, startVector + additiveVector));

            startVector += additiveVector;
        }

        return edgeVectors;
    }

    //Finds edge vectors inside a field, not around it
    public List<VectorEdge> GetEdgeVectorsInField(Vector2Int startPoint, Direction direction, Vector2Int size)
    {
        List<VectorEdge> edgeVectors = new List<VectorEdge>();

        #region 0 degrees

        if (direction == Direction._0)
        {
            Vector2Int cellToSearch = startPoint + new Vector2Int(1, 0);
            for (int i = 1; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    edgeVectors.Add(new VectorEdge(cellToSearch, cellToSearch + new Vector2Int(0, 1)).Fix());
                    cellToSearch.y += 1;
                }

                cellToSearch.y = startPoint.y;
                cellToSearch.x += 1;
            }

            cellToSearch = startPoint + new Vector2Int(0, 1);

            for (int j = 1; j < size.y; j++)
            {
                for (int i = 0; i < size.x; i++)
                {
                    edgeVectors.Add(new VectorEdge(cellToSearch, cellToSearch + new Vector2Int(1, 0)));
                    cellToSearch.x += 1;
                }

                cellToSearch.x = startPoint.x;
                cellToSearch.y += 1;
            }
        }

        #endregion

        #region 90 degrees

        else if (direction == Direction._90)
        {
            Vector2Int cellToSearch = startPoint + new Vector2Int(1, 0);
            for (int i = 1; i < size.y; i++)
            {
                for (int j = 0; j < size.x; j++)
                {
                    edgeVectors.Add(new VectorEdge(cellToSearch, cellToSearch + new Vector2Int(0, -1)).Fix());
                    cellToSearch.y -= 1;
                }

                cellToSearch.y = startPoint.y;
                cellToSearch.x += 1;
            }

            cellToSearch = startPoint + new Vector2Int(0, -1);

            for (int j = 1; j < size.x; j++)
            {
                for (int i = 0; i < size.y; i++)
                {
                    edgeVectors.Add(new VectorEdge(cellToSearch, cellToSearch + new Vector2Int(1, 0)));
                    cellToSearch.x += 1;
                }

                cellToSearch.x = startPoint.x;
                cellToSearch.y -= 1;
            }
        }

        #endregion

        #region 180 degrees

        else if (direction == Direction._180)
        {
            Vector2Int cellToSearch = startPoint + new Vector2Int(0, 0);
            for (int i = 1; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    edgeVectors.Add(new VectorEdge(cellToSearch, cellToSearch + new Vector2Int(0, 1)).Fix());
                    cellToSearch.y -= 1;
                }

                cellToSearch.y = startPoint.y;
                cellToSearch.x -= 1;
            }

            cellToSearch = startPoint + new Vector2Int(0, 0);

            for (int j = 1; j < size.y; j++)
            {
                for (int i = 0; i < size.x; i++)
                {
                    edgeVectors.Add(new VectorEdge(cellToSearch, cellToSearch + new Vector2Int(1, 0)).Fix());
                    cellToSearch.x -= 1;
                }

                cellToSearch.x = startPoint.x;
                cellToSearch.y -= 1;
            }
        }

        #endregion

        #region 270 degrees

        else if (direction == Direction._270)
        {
            Vector2Int cellToSearch = startPoint + new Vector2Int(0, 0);
            for (int i = 1; i < size.y; i++)
            {
                for (int j = 0; j < size.x; j++)
                {
                    edgeVectors.Add(new VectorEdge(cellToSearch, cellToSearch + new Vector2Int(0, 1)).Fix());
                    cellToSearch.y += 1;
                }

                cellToSearch.y = startPoint.y;
                cellToSearch.x -= 1;
            }

            cellToSearch = startPoint + new Vector2Int(0, 1);

            for (int j = 1; j < size.x; j++)
            {
                for (int i = 0; i < size.y; i++)
                {
                    edgeVectors.Add(new VectorEdge(cellToSearch, cellToSearch + new Vector2Int(1, 0)).Fix());
                    cellToSearch.x -= 1;
                }

                cellToSearch.x = startPoint.x;
                cellToSearch.y += 1;
            }
        }

        #endregion


        return edgeVectors;
    }

    public List<Vector2Int> GetEdgeCornersInRange(Vector2Int startPoint, VectorDirection direction, int count)
    {
        List<Vector2Int> cornerVectors = new List<Vector2Int>();

        Vector2Int startVector = startPoint;
        Vector2Int additiveVector = new Vector2Int();

        switch (direction)
        {
            case VectorDirection.Top:
                additiveVector = new Vector2Int(0, 1);
                break;
            case VectorDirection.Right:
                additiveVector = new Vector2Int(1, 0);
                break;
            case VectorDirection.Bottom:
                additiveVector = new Vector2Int(0, -1);
                break;
            case VectorDirection.Left:
                additiveVector = new Vector2Int(-1, 0);
                break;
        }

        for (int i = 0; i < count; i++)
        {
            cornerVectors.Add(startVector);
            startVector += additiveVector;
        }

        return cornerVectors;
    }

    //Finds direction of line with given start and end point
    public VectorDirection GetVectorDirectionAndLength(Vector2Int startPoint, Vector2Int endPoint, out int length)
    {
        VectorDirection resultDirection;

        int wallPreviewLengthActualValue = 0;

        int x = endPoint.x - startPoint.x;
        int y = endPoint.y - startPoint.y;

        int xAbs = Mathf.Abs(x);
        int yAbs = Mathf.Abs(y);

        if (xAbs >= yAbs)
        {
            length = xAbs;
            if (x > 0)
            {
                resultDirection = VectorDirection.Right;
            }
            else
            {
                resultDirection = VectorDirection.Left;
            }
        }
        else
        {
            length = yAbs;
            if (y > 0)
            {
                resultDirection = VectorDirection.Top;
            }
            else
            {
                resultDirection = VectorDirection.Bottom;
            }
        }

        return resultDirection;
    }

    public VectorEdge GetEdgeVectorInDirection(Vector2Int cellCoordinate, Direction direction)
    {
        switch (direction)
        {
            case Direction._0:
                return new VectorEdge(cellCoordinate + new Vector2Int(1, 0), cellCoordinate + new Vector2Int(1, 1));
            case Direction._90:
                return new VectorEdge(cellCoordinate + new Vector2Int(0, 1), cellCoordinate + new Vector2Int(1, 1));
            case Direction._180:
                return new VectorEdge(cellCoordinate + new Vector2Int(0, 0), cellCoordinate + new Vector2Int(0, 1));
            default:
                return new VectorEdge(cellCoordinate + new Vector2Int(0, 0), cellCoordinate + new Vector2Int(1, 0));
        }
    }

    public Edge GetEdgeInDirection(Vector2Int cellCoordinate, Direction direction)
    {
        VectorEdge edgeVector = GetEdgeVectorInDirection(cellCoordinate, direction);
        return edges[edgeVector];
    }

    public bool IsThereAnyWallInFieldAndIsFieldExist(Vector2Int startPoint, Direction direction, Vector2Int size)
    {
        foreach (var edgeVector in GetEdgeVectorsInField(startPoint, direction, size))
        {
            if (edges.TryGetValue(edgeVector, out Edge edge))
            {
                if (edge.wall != null)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        return false;
    }
}

public enum EdgeDirection
{
    Top,
    Left,
    Bottom,
    Right
}

public enum VectorDirection
{
    Top,
    Left,
    Bottom,
    Right
}
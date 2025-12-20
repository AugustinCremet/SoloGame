using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PathChecker
{
    private Tilemap _blockedTilemap;
    private Tilemap _walkableTilemap;
    private List<Vector3Int> _directPath = new List<Vector3Int>();
    private int _maxDistance = 20;

    private static readonly Vector3Int[] directions =
    {
        new Vector3Int ( 1,  0 , 0),
        new Vector3Int (-1,  0 , 0),
        new Vector3Int ( 0,  1 , 0),
        new Vector3Int ( 0, -1 , 0),
    };

    public PathChecker(Tilemap blockedTilemap, Tilemap walkableTilemap, int maxDistance = 20)
    {
        _blockedTilemap = blockedTilemap;
        _walkableTilemap = walkableTilemap;
        _maxDistance = maxDistance;
    }

    public bool CheckPath(Vector3Int start, Vector3Int end, HashSet<Vector3Int> extraBlockedCell)
    {
        Queue<Vector3Int> open = new Queue<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int?> parents = new();

        Dictionary<Vector3Int, int> distances = new Dictionary<Vector3Int, int>();

        open.Enqueue(start);
        visited.Add(start);
        parents[start] = null;
        distances[start] = 0;
        DrawX(start, Color.red);

        while (open.Count > 0)
        {
            Vector3Int current = open.Dequeue();
            int currentDistance = distances[current];

            if (currentDistance >= _maxDistance)
            {
                continue;
            }

            if (current == end)
            {
                _directPath = BuildPath(parents, end);
                DrawCorrectPath();
                return true;
            }

            int nextDistance = currentDistance + 1;

            foreach (var dir in directions)
            {
                Vector3Int next = current + dir;

                if (nextDistance > _maxDistance)
                {
                    continue;
                }

                if (visited.Contains(next))
                {
                    continue;
                }
                if (!IsWalkable(next, extraBlockedCell))
                {
                    continue;
                }

                visited.Add(next);
                open.Enqueue(next);
                parents[next] = current;
                distances[next] = nextDistance;
                DrawX(next, Color.red);
            }
        }

        return false;
    }

    private bool IsWalkable(Vector3Int cell, HashSet<Vector3Int> extraBlockedCell)
    {
        if (_blockedTilemap.HasTile(cell) || !_walkableTilemap.HasTile(cell))
        {
            return false;
        }

        if(extraBlockedCell.Contains(cell))
        {
            return false;
        }

        return true;
    }

    private List<Vector3Int> BuildPath(Dictionary<Vector3Int, Vector3Int?> parents, Vector3Int goalCell)
    {
        List<Vector3Int> path = new();
        Vector3Int? current = goalCell;

        while (current != null)
        {
            path.Add(current.Value);
            current = parents[current.Value];
        }

        path.Reverse();
        return path;
    }

    private void DrawX(Vector3Int current, Color color)
    {
        Vector3 worldPos = _blockedTilemap.CellToWorld(current);
        Vector3 leftOffset = worldPos + new Vector3(0.25f, 0.5f, 0f);
        Vector3 bottomOffset = worldPos + new Vector3(0.5f, 0.25f, 0f);

        Debug.DrawLine(leftOffset, leftOffset + new Vector3(0.5f, 0f, 0f), color, 1f);
        Debug.DrawLine(bottomOffset, bottomOffset + new Vector3(0f, 0.5f, 0f), color, 1f);
    }

    private void DrawCorrectPath()
    {
        foreach(var cell in _directPath)
        {
            DrawX(cell, Color.green);
        }
    }
}

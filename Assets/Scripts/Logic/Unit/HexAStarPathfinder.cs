using System.Collections.Generic;
using System.Linq;
using HexagonScripts;
using UnityEngine;

namespace Logic.Unit
{
    public class HexAStarPathfinder
    {
        private readonly Field.Field field;

        public HexAStarPathfinder(Field.Field field)
        {
            this.field = field;
        }

        public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
        {
            if (start == goal)
                return new List<Vector2Int> { start };

            var openSet = new List<Vector2Int> { start };
            var closedSet = new HashSet<Vector2Int>();

            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

            var gScore = new Dictionary<Vector2Int, int>
            {
                [start] = 0
            };

            var fScore = new Dictionary<Vector2Int, int>
            {
                [start] = HexagonMath.Distance(start, goal)
            };

            while (openSet.Count > 0)
            {
                var current = openSet
                    .OrderBy(h => fScore.GetValueOrDefault(h, int.MaxValue))
                    .First();

                if (current == goal)
                    return ReconstructPath(cameFrom, current);

                openSet.Remove(current);
                closedSet.Add(current);

                var currentHex = field.GetHex(current);
                if (currentHex == null)
                    continue;

                foreach (var neighbor in field.GetNeighbours(currentHex))
                {
                    if (!field.IsWalkable(neighbor))
                        continue;

                    var nCoord = neighbor.coordinates;

                    if (closedSet.Contains(nCoord))
                        continue;

                    int tentativeG = gScore[current] + 1;

                    if (!openSet.Contains(nCoord))
                        openSet.Add(nCoord);
                    else if (tentativeG >= gScore.GetValueOrDefault(nCoord, int.MaxValue))
                        continue;

                    cameFrom[nCoord] = current;
                    gScore[nCoord] = tentativeG;
                    fScore[nCoord] = tentativeG + HexagonMath.Distance(nCoord, goal);
                }
            }

            return null; // пути нет
        }

        private List<Vector2Int> ReconstructPath(
            Dictionary<Vector2Int, Vector2Int> cameFrom,
            Vector2Int current)
        {
            var path = new List<Vector2Int> { current };

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }

            path.Reverse();
            return path;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace PathfindingCSharp
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public int x, y;


        public bool Equals(Vector2 other)
        {
            return x == other.x && y == other.y;
        }
    }
    public class Map
    {
        public const char emptyChar = '.';
        public const char blockedChar = '#';
        public const char invalidChar = '?';
        public const char pathChar = '@';

        private char[,] mapPositions;
        Vector2 startPosition;
        Vector2 goalPosition;

        public Map(string[] mapData, Vector2 startPos, Vector2 endPos)
        {
            if (startPos.x < 0 || startPos.x >= mapData.Length || startPos.y < 0 || startPos.y >= mapData.Length)
                throw new Exception("Start position is out of bounds, provided coordinates were: " + startPos.x + "," + startPos.y);

            if (endPos.x < 0 || endPos.x >= mapData.Length || endPos.y < 0 || endPos.y >= mapData.Length)
                throw new Exception("End position is out of bounds, provided coordinates were: " + endPos.x + "," + endPos.y);

            if (mapData[startPos.y][startPos.x] == blockedChar)
                throw new Exception("Start position is invalid, provided coordinates were: " + startPos.x + "," + startPos.y);

            if (mapData[endPos.y][endPos.x] == blockedChar)
                throw new Exception("End position is invalid, provided coordinates were: " + endPos.x + "," + endPos.y);

            startPosition = startPos;
            goalPosition = endPos;

            mapPositions = new char[mapData.Length, mapData.Length];

            for (int i = 0; i < mapData.Length; ++i)
            {
                for (int j = 0; j < mapData[i].Length; ++j)
                {
                    int index = (i * mapData.Length) + j;

                    if (startPos.x == j && startPos.y == i)
                        mapPositions[i, j] = pathChar;
                    else if (endPos.x == j && endPos.y == i)
                        mapPositions[i, j] = pathChar;
                    else
                    {
                        switch (mapData[i][j])
                        {
                            case emptyChar:
                                mapPositions[i, j] = emptyChar;
                                break;

                            case blockedChar:
                                mapPositions[i, j] = blockedChar;
                                break;

                            default:
                                mapPositions[i, j] = invalidChar;
                                break;
                        }
                    }

                }
            }
        }

        private bool IsBlocked(Vector2 position)
        {
            return mapPositions[position.y, position.x] == blockedChar;
        }

        public void DisplayMap()
        {
            Console.Clear();
            Console.WriteLine(SolutionToString());
        }

        public string SolutionToString()
        {
            StringBuilder sb = new StringBuilder(mapPositions.Length + mapPositions.GetLength(0)); //add extra space for the new lines

            for (int i = 0; i < mapPositions.GetLength(0); ++i)
            {
                for (int j = 0; j < mapPositions.GetLength(1); ++j)
                {
                    sb.Append(mapPositions[i, j]);
                }
                sb.Append('\n');
            }

            return sb.ToString();
        }

        public bool ComputePath()
        {
            //TODO: Implement solution here
            var frontier = new Queue<Vector2>();
            frontier.Enqueue(startPosition);
            var visited = new List<Vector2>();
            //var cameFrom = new List<Vector2>();
            var cameFrom = new Dictionary<Vector2, Vector2>();

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                visited.Add(current);

                if (current.Equals( goalPosition))
                {

                }
                foreach (var point in Neighbors(current))
                {
                    if (visited.Contains(point) || mapPositions[point.y, point.x] == '#')
                    {
                        continue;
                    }

                    frontier.Enqueue(point);
                    visited.Add(point);
                    cameFrom[point] = current;
                }
            }

            var currentpos = goalPosition;
            while (!currentpos.Equals(startPosition))
            {
                mapPositions[currentpos.y, currentpos.x] = '@';
                try
                {
                    currentpos = cameFrom[currentpos];
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public Vector2[] Neighbors(Vector2 current)
        {
            //DONT LOOK AT THIS CODE. ITS UGLY
            var north = new Vector2();
            var northeast = new Vector2();
            var east = new Vector2();
            var southeast = new Vector2();
            var south = new Vector2();
            var southwest = new Vector2();
            var west = new Vector2();
            var northwest = new Vector2();
            north = current;
            north.y += 1;
            northeast = current;
            northeast.x += 1; northeast.y += 1;
            east = current;
            east.x += 1;
            southeast = current;
            southeast.x += 1; southeast.y += -1;
            south = current;
            south.y += -1;
            southwest = current;
            southwest.x += -1; southwest.y += -1;
            west = current;
            west.x += -1;
            northwest = current;
            northwest.x += -1; northwest.y += 1;

            return new[] { north, northeast, east, southeast, south, southwest, west, northwest };
        }



    };
}

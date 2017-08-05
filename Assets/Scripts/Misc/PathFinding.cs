using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace PathFinding {
    public static class Extensions
    {
        public static List<PathFinding.Location> GetAdjacentWalkableLocations(this short[,] value, int x, int y, PathFinding.PathComparer comparer)
        {
            var result = new List<PathFinding.Location>();

            if (x > 0 && comparer(value[x - 1, y]))
                result.Add(new PathFinding.Location() { CoordX = x - 1, CoordY = y });

            if (x < value.GetLength(0) - 1 && comparer(value[x + 1, y]))
                result.Add(new PathFinding.Location() { CoordX = x + 1, CoordY = y });

            if (y > 0 && comparer(value[x, y - 1]))
                result.Add(new PathFinding.Location() { CoordX = x, CoordY = y - 1 });

            if (y < value.GetLength(1) - 1 && comparer(value[x, y + 1]))
                result.Add(new PathFinding.Location() { CoordX = x, CoordY = y + 1 });

            return result;
        }

    }

    /// <summary>
    /// This class utilizes A* path finding algorithm.
    /// Reference article is in "https://www.raywenderlich.com/4946/introduction-to-a-pathfinding".
    /// To keep it simple, instead of a generic graph, this class will use the grid type I use
    /// within the project.
    /// </summary>
    public class PathFinding
    {
        public delegate bool PathComparer(short value);

        public static class Utility
        {

            public static int ManhattanDistance(int x, int y, int targetX, int targetY)
            {
                return Mathf.Abs(x - targetX) + Mathf.Abs(y - targetY);
            }
        }

        //private void Start()
        //{
        //    //HashSet<Location> lol = new HashSet<Location>(new Location.Comparer());

        //    //lol.Add(new Location() { CoordX = 1, CoordY = 0 });
        //    //lol.Add(new Location() { CoordX = 1, CoordY = 0 });
        //    //lol.Add(new Location() { CoordX = 1, CoordY = 0 , F = 2});
        //    //lol.Add(new Location() { CoordX = 1, CoordY = 2 });

        //    //Debug.Log(lol.Contains(new Location { CoordX = 1, CoordY = 0 }));

        //    //short[,] map = new short[10,10]
        //    //{
        //    //    { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    //    { 0, 1, 0, 0, 1, 0, 0, 0, 0, 0 },
        //    //    { 0, 1, 0, 0, 1, 0, 0, 0, 0, 0 },
        //    //    { 0, 1, 0, 0, 1, 0, 0, 0, 0, 0 },
        //    //    { 0, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
        //    //    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    //    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    //    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    //    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    //    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        //    //};

        //    //var path = FindPath(map, 0, 0, 3, 3);

        //    //Debug.Log(path.Count);
        //}

        public class Location
        {
            public int CoordX, CoordY;
            public int G; // Distance from starting point.
            public int H; // Estimated distance to target. (For simplicity I will be using Manhattan Distance)
            public int F; // Just summation of G and H scores.
            public Location Parent; // Link to the previous step is stored so we can back trace and construct the whole path at the end.

            public static bool operator ==(Location x, Location y)
            {
                if(object.ReferenceEquals(x, null))
                {
                    return object.ReferenceEquals(y, null);
                }

                return x.CoordX == y.CoordX && x.CoordY == y.CoordY;
            }

            public static bool operator !=(Location x, Location y)
            {
                var nullX = object.ReferenceEquals(x, null);
                var nullY = object.ReferenceEquals(y, null);

                if (nullX || nullY)
                {
                    return !(nullX && nullY);                
                }                

                return x.CoordX != y.CoordX || x.CoordY != y.CoordY;
            }

            public class Comparer : EqualityComparer<Location>
            {
                public override bool Equals(Location x, Location y)
                {
                    return x.CoordX == y.CoordX && x.CoordY == y.CoordY;
                }

                public override int GetHashCode(Location obj)
                {
                    return obj.CoordX.GetHashCode() * 10 + obj.CoordY.GetHashCode();
                }
            }
        }

        public static List<Vector2> FindPath(short[,] map, int sourceX, int sourceY, int destX, int destY, short mask = 1)
        {
            Location current = null;
            Location start = new Location() { CoordX = sourceX, CoordY = sourceY, Parent = null };
            Location target = new Location() { CoordX = destX, CoordY = destY };

            var locationComparer = new Location.Comparer();

            HashSet<Location> openList = new HashSet<Location>(locationComparer); // This will hold the locations to be considered.
            // These are the locations we are done with.
            HashSet<Location> closedList = new HashSet<Location>(locationComparer); 

            int g = 0; // This will keep track of our G score.

            openList.Add(start);

            while (openList.Count > 0)
            {
                // Take the location with the lowest F score.
                current = openList.First((y) => y.F == openList.Min((x) => x.F));

                if (current == target)
                {
                    // Found the path. Next, construct the path and return.
                    return constructPathFromNode(current);
                }

                closedList.Add(current);
                openList.Remove(current);

                var adjacentLocations = map.GetAdjacentWalkableLocations(current.CoordX, current.CoordY, (x) => (x & mask) == 0);
                g++;

                foreach(var location in adjacentLocations)
                {
                    if (closedList.Contains(location))
                    {
                        continue;
                    }

                    if (!openList.Contains(location))
                    {
                        location.G = g;
                        location.H = Utility.ManhattanDistance(location.CoordX, location.CoordY, target.CoordX, target.CoordY);
                        location.F = location.G + location.H;
                        location.Parent = current;

                        openList.Add(location);
                    }
                    else
                    {
                        // Check if current node with the G score g makes a better path
                        // when parents location. If so change parent for this location.
                        if (g + location.H < location.H)
                        {
                            location.G = g;
                            location.F = location.G + location.H;
                            location.Parent = current;
                        }
                    }
                }
            }
            return null;
        }

        // Back trace from the given node. And construct a Vec2 path.
        private static List<Vector2> constructPathFromNode(Location node)
        {
            List<Vector2> path = new List<Vector2>();

            Location current = node;

            while(current.Parent != null)
            {
                path.Add(new Vector2(current.CoordX, current.CoordY));
                current = current.Parent;
            }

            // This will exclude the starting point from the path.
            path.Reverse();
            return path;
        }
    }
}
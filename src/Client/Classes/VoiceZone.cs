﻿using CitizenFX.Core;
using System;
using System.Collections.Generic;

namespace OpenRP.Framework.Client.Controllers
{
    public class VoiceZone
    {
        static readonly Dictionary<string, int> _grid = new Dictionary<string, int>();
        static readonly int minX = -5000;
        static readonly int maxX = 6000;
        static readonly int minY = 9000;
        static readonly int maxY = -5000;
        static readonly int distanceX = Math.Abs(minX) + Math.Abs(maxX);
        static readonly int distanceY = Math.Abs(minY) + Math.Abs(maxY);

        static VoiceZone()
        {
            BuildGrid();
        }

        static void BuildGrid()
        {
            var interval = 100;
            var count = 0;

            for (var y = 0; y < distanceY; y += interval)
            {
                for (var x = 0; x < distanceX; x += interval)
                {
                    var quad = new GridCoord(x / interval, y / interval);
                    _grid.Add(quad.GetHashCode(), count);
                    count++;
                }
            }
        }

        public static int GetGridCoord(Vector3 pos)
        {
            var x = Math.Floor((pos.X - minX) / 100.0);
            var y = Math.Ceiling((pos.Y - minY) / 100.0) * -1;
            var gridCoord = new GridCoord((int)x, (int)y);
            var gridHash = gridCoord.GetHashCode();
            return _grid[gridHash];
        }
    }
}

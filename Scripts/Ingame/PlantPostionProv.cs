using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1W.MoonPlant
{
    public static class PlantPostionProv
    {
        private static float y = 0f;

        public static Dictionary<(int x, int y), Vector3> PositionDictionary = new Dictionary<(int x, int y), Vector3>
        {
            { (0, 0), new Vector3(-3.75f, y, 3.75f) },
            { (1, 0), new Vector3(-1.25f, y, 3.75f) },
            { (2, 0), new Vector3(1.25f, y, 3.75f) },
            { (3, 0), new Vector3(3.75f, y, 3.75f) },
            { (0, 1), new Vector3(-3.75f, y, 1.25f) },
            { (1, 1), new Vector3(-1.25f, y, 1.25f) },
            { (2, 1), new Vector3(1.25f, y, 1.25f) },
            { (3, 1), new Vector3(3.75f, y, 1.25f) },
            { (0, 2), new Vector3(-3.75f, y, -1.25f) },
            { (1, 2), new Vector3(-1.25f, y, -1.25f) },
            { (2, 2), new Vector3(1.25f, y, -1.25f) },
            { (3, 2), new Vector3(3.75f, y, -1.25f) },
            { (0, 3), new Vector3(-3.75f, y, -3.75f) },
            { (1, 3), new Vector3(-1.25f, y, -3.75f) },
            { (2, 3), new Vector3(1.25f, y, -3.75f) },
            { (3, 3), new Vector3(3.75f, y, -3.75f) },
        };
    }
}
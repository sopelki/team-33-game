using UnityEngine;

namespace HexagonScripts
{
    public class HexagonMath
    {
        public static Vector2Int OffsetToAxial(int x, int y)
        {
            var q = x - (y - (y & 1)) / 2;

            return new Vector2Int(q, y);
        }
    }
}
using UnityEngine;

namespace DefaultNamespace
{
    public class HexagonMath
    {
        public static Vector2Int OffsetToAxial(int x, int y)
        {
            var q = x - (y - (y & 1)) / 2;
            var r = y;
            
            return new Vector2Int(q, r);
        }
    }
}
using System.Collections.Generic;
using HexagonScripts;

namespace Field
{
    [System.Serializable]
    public class FieldData
    {
        public List<Hexagon> savedHexes = new();
        public List<MapObjectData> savedObjects = new();
    }
}
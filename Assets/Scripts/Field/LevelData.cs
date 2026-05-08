using UnityEngine;

namespace Field
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Game/Level Asset")]
    public class LevelAsset : ScriptableObject
    {
        public string levelName;
        public TextAsset jsonFile;
    }
}
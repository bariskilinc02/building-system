using UnityEngine;

namespace Game.BuildSystem
{
    [CreateAssetMenu(fileName = "new Building", menuName = "ScriptableObject/Building", order = 0)]
    public class Building : ScriptableObject
    {
        public GameObject Prefab;
        public int Width;
        public int Height;
    }
}
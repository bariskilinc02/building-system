using UnityEngine;

namespace Game.GridSystem
{
    public class GridTile
    {
        public int x;
        public int y;

        public GridMap ConnectedGridMap;
        public GridTileTransform GridTileTransform;
        public Transform Build;

        public bool IsEmpty => Build == null;

        public void SetBuild(Transform transform)
        {
            Build = transform;
        }
        
        public void ClearBuild()
        {
            Build = null;
        }
        public GridTile(int x, int y, GridMap gridMap)
        {
            this.x = x;
            this.y = y;
            ConnectedGridMap = gridMap;
        }


    }
}

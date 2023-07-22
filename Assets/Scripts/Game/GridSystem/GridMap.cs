using System;
using System.Collections.Generic;
using Game.BuildSystem;
using Unity.Mathematics;
using UnityEngine;

namespace Game.GridSystem
{
    public class GridMap : MonoBehaviour
    {
        [SerializeField] private Transform gridMapPivot;
        
        private List<GridTile> _gridTiles;
        private List<GridTileTransform> _gridTileTransforms;

        [SerializeField] private int _gridWidth;
        [SerializeField] private int _gridHeight;

        public GameObject TileTransformPrefab;


        private void InitializeGridMap()
        {
            CreateGridTiles(_gridWidth, _gridHeight);
            InstantiateGridTileTransforms(_gridTiles, gridMapPivot);
        }
        private void Awake()
        {
        
            InitializeGridMap();
        }

        private void CreateGridTiles(int gridWidth, int gridHeight)
        {
            _gridTiles = new List<GridTile>();

            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    GridTile grid = new GridTile(i,j, this);
                    _gridTiles.Add(grid);
                }
            }
        }

        private void InstantiateGridTileTransforms(List<GridTile> gridTiles, Transform rootTransform)
        {
           
            for (int i = 0; i < gridTiles.Count; i++)
            {
                Vector3 rootPosition = rootTransform.position;
                rootPosition += new Vector3(gridTiles[i].x, 0, gridTiles[i].y);
                
                GameObject tileTransform = Instantiate(TileTransformPrefab, rootPosition, quaternion.identity);
                tileTransform.transform.parent = gridMapPivot;

                GridTileTransform gridTransform = tileTransform.GetComponent<GridTileTransform>();
                gridTransform.InitGridTransform(_gridTiles[i], this);

                gridTiles[i].GridTileTransform = gridTransform;
            }
            
        }

        public GridTile GetGrid(int xPosition, int yPosition)
        {
            return _gridTiles.Find(x => x.x == xPosition && x.y == yPosition);
        }

        public GridTile GetGridWithPosition(Vector3 position, out Vector3 gridPosition)
        {
            gridPosition = new Vector3(position.x.ToInt(),0,position.z.ToInt());
            return GetGrid(position.x.ToInt(), position.z.ToInt());
        }

        public bool IsAreaEmpty(Building building, GridTile pivotGridTile)
        {
            for (int i = 0; i < building.Width; i++)
            {
                for (int j = 0; j < building.Height; j++)
                {
                    if (GetGrid( pivotGridTile.x + i, pivotGridTile.y + j) == null)
                    {
                        return false;
                    }
                    else if (GetGrid( pivotGridTile.x + i, pivotGridTile.y + j).IsEmpty != true)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        public void BuildToArea(Building building, GridTileTransform pivotGridTransform)
        {
            GameObject tempBuild = Instantiate(building.Prefab, pivotGridTransform.GetPivotPosition(), Quaternion.identity);
            
            for (int i = 0; i < building.Width; i++)
            {
                for (int j = 0; j < building.Height; j++)
                {
                    GetGrid(pivotGridTransform.GetConnectedGridTile().x + i,
                        pivotGridTransform.GetConnectedGridTile().y + j).Build = tempBuild.transform;
                }
            }
        }

        public List<GridTile> GetArea(Building building, GridTile pivotGridTile, out bool allTilesAccessable)
        {
            List<GridTile> gridTiles = new List<GridTile>();
            allTilesAccessable = true;
            
            for (int i = 0; i < building.Width; i++)
            {
                for (int j = 0; j < building.Height; j++)
                {
                    GridTile tile = GetGrid(pivotGridTile.x + i, pivotGridTile.y + j);
                    if (tile != null)
                    {
                        gridTiles.Add(tile);
                    }
                    else
                    {
                        allTilesAccessable = false;
                    }
                }


            }
            return gridTiles;
        }

        public void SetAreaHighlight(List<GridTile> gridTiles, HighlightType highlightType)
        {
            foreach (GridTile gridTile in gridTiles)
            {
                gridTile.GridTileTransform.SetHighlight(highlightType);
            }
        }
    }
}

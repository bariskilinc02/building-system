using System.Collections.Generic;
using Game.GridSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.BuildSystem
{
    public class Builder : MonoBehaviour
    {
        private GridTile _currentGridTile;
        private GridMap _currentGridMap;
        private List<GridTile> _currentGridTiles;

        public Building Building;

        private void Update()
        {
            Build();
        }

        private void Build()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Mouse3D.GetComponentMouseClick(out GridTileTransform tileTransform))
                {
                    _currentGridMap = tileTransform.GetConnectedGridTable();
                    _currentGridTile = tileTransform.GetConnectedGridTile();
                    List<GridTile> _currentGridTiles = _currentGridMap.GetArea(Building, _currentGridTile,out bool allTilesAccessable);

                    if (_currentGridMap.IsAreaEmpty(Building, _currentGridTile))
                    {
                        _currentGridMap.BuildToArea(Building, tileTransform);
                        _currentGridMap.SetAreaHighlight(_currentGridTiles, HighlightType.Green);
                    }
                    else
                    {
                        _currentGridMap.SetAreaHighlight(_currentGridTiles, HighlightType.Red);
                    }
                }
            }
            
            if (Mouse3D.GetComponentMouseClick(out GridTileTransform tileTransformm))
            {
                _currentGridMap = tileTransformm.GetConnectedGridTable();
                _currentGridTile = tileTransformm.GetConnectedGridTile();
                _currentGridTiles = _currentGridMap.GetArea(Building, _currentGridTile,out bool allTilesAccessable);

                if (_currentGridMap.IsAreaEmpty(Building, _currentGridTile))
                {
                    _currentGridMap.SetAreaHighlight(_currentGridTiles, HighlightType.Green);
                }
                else
                {
                    _currentGridMap.SetAreaHighlight(_currentGridTiles, HighlightType.Red);
                }
            }
            
        }
    }
}

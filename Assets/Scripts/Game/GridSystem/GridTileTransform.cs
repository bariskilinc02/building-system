using System;
using UnityEngine;

namespace Game.GridSystem
{
    public partial class GridTileTransform : MonoBehaviour
    {
        private GridMap _connectedGridMap;
        private GridTile _grid;

        private Material _material;
        private bool _isHighlight;
        private HighlightType _highlightType;
        private bool _visualUpdated;


        private void Awake()
        {
            _material = GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            UpdateMaterial();
        }

        public void InitGridTransform(GridTile grid, GridMap gridMap)
        {
            _connectedGridMap = gridMap;
            _grid = grid;
        
        }

        public void SetBuild()
        {
        
        }

        public GridMap GetConnectedGridTable()
        {
            return _connectedGridMap;
        }
        
        public GridTile GetConnectedGridTile()
        {
            return _grid;
        }

        public Vector3 GetPivotPosition()
        {
            return transform.GetChild(0).transform.position;
        }

        public void SetHighlight(HighlightType highlightType)
        {
            _visualUpdated = false;
            _highlightType = highlightType;
        }

        private void UpdateMaterial()
        {
            if(_visualUpdated)
                return;

            _visualUpdated = true;

            _material.color = _highlightType == HighlightType.None
                ? Color.black
                : (_highlightType == HighlightType.Green ? Color.green : Color.red);

            if ( _highlightType != HighlightType.None)
            {
                _visualUpdated = false;
                _highlightType = HighlightType.None;
            }
        }
    }
}

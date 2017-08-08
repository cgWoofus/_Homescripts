using UnityEngine;
using System.Collections.Generic;

public class CameraBound{
    public List<Bounds> _boundCollection = new List<Bounds>();
    Stack<Vector2> _referenceStack = new Stack<Vector2>();
    public CameraBound()
    {
    }

    public bool checkPoints(FrustumEdge _edge)
    {
        var _vertices = new Vector2[2] { _edge._vertexA, _edge._vertexB };
        int _passCount = 0;
        for (int _z = 0; _z < 2; _z++)
        for (int _y = 0; _y < _boundCollection.Count; _y++)
        {
            if (_boundCollection[_y].Contains(_vertices[_z]))
            {
                _passCount++;
                break;
            }
        }

        return _passCount == 2 ?  true : false;

    }


    public  void FillBoundList(LevelHandler _lvlHandle)
    {
        var _focusObjects = _lvlHandle._focusPts;
        for (int _x = 0; _x < _focusObjects.Count; _x++)
        {
            _boundCollection.Add(_focusObjects[_x]);
        }
    }
    /// <summary>
    /// clear Bound Collection
    /// </summary>
    public void ClearList()
    {
        _boundCollection.Clear();
    }
}


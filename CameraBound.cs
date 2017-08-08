using UnityEngine;
using System.Collections.Generic;

public class CameraBound{
    public List<Bounds> _boundCollection = new List<Bounds>();
    LevelHandler _lvlHandle;
    Stack<Vector2> _referenceStack = new Stack<Vector2>();
    public CameraBound()
    {



    //    CameraBound._boundCollection.Add(_bound);

    }

    /// <summary>
    /// Check Registered BoundList for this point
    /// </summary>
    /// <param name="_ptOfReference"></param>
    /// <returns></returns>
    public float checkList(params Vector2[] _ptOfReference)
    {
        // if Collection does not exist pass  normal values;    


        if (_boundCollection.Count < 1)
            return 1f;

        int _passCount = 0 ;
        for (int _y = 0; _y < _ptOfReference.Length; _y++)
        {            
            for (int _x = 0; _x < _boundCollection.Count; _x++)
            {
                if (_boundCollection[_x].Contains(_ptOfReference[_y]))
                {                  
                    _passCount++;
                    break;
                }
            }
        }

        if (_passCount == _ptOfReference.Length)
            return 1;
        else
            return 0;
    }

    /// <summary>
    /// pass Focus Gameobjects to be converted into Bounds
    /// </summary>
    /// <param name="_focusObjects"></param>
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


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraCtrl : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    GameObject _midPoint,_midPoint2;

    [SerializeField]
    float _distance;
    Vector3 _initialVelocity = Vector3.zero;
    [SerializeField]
    Camera _camera;
     
    #region Switches
    [SerializeField]
    bool _includePlayer, _enableBounds;
    #endregion

    /// <summary>
    ///  frustum points
    /// </summary>
    [SerializeField]
    Vector2 _topLeft, _topRight, _bottomLeft, _bottomRight;

    Vector2[] _frustumRight;
    Vector2[] _frustumLeft;


    [SerializeField]
    List<Transform> _focusPts = new List<Transform>();
    Transform _player;
    CameraBound _boundManager;
    [SerializeField]
    LevelHandler _lvlHandle;

    FrustumEdge _left, _right, _up, _down;

    #region Restriction Enum
    enum directionHorizontal { Left, Right, None };
    enum directionVertical { Up, Down, None }
    directionHorizontal _myHorizontalRestriction;
    directionVertical _myVerticalRestricion;
    #endregion

    void Start()
    {
        //get player transform  add to focus point
        includePlayer(_includePlayer);
        // instance bound manager            
        _boundManager = new CameraBound();
        if (_lvlHandle && _enableBounds)
            _boundManager.FillBoundList(_lvlHandle);


        //_frustumRight = new Vector2[] { _topRight, _bottomRight};
        // _frustumLeft = new Vector2[] { _topLeft, _bottomLeft };
        _left = new FrustumEdge();
        _right = new FrustumEdge();
        _up = new FrustumEdge();
        _down = new FrustumEdge();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 _res = _enableBounds ? boundRestriction() : (Vector2)averageFocusPts(_focusPts);
        move(transform, _res);
    }

    private Vector2 boundRestriction()
    {
        //check if movement is within bounds
        var _rawFocus = averageFocusPts(_focusPts);
        var _horizontal = Vector2.right * transform.position.x;
        var _vertical = Vector2.up * transform.position.y;
        var _normalizedDirection = (_rawFocus - transform.position).normalized;
        var _directionHorizontal = _normalizedDirection.x < 0 ? directionHorizontal.Left : directionHorizontal.Right;
        var _directionVertical = _normalizedDirection.y < 0 ? directionVertical.Down : directionVertical.Up;


        // if direction of x is negative(left) check if left movement is allowed 
        if (_myHorizontalRestriction != _directionHorizontal)
            _horizontal = horizontal(_rawFocus.x);

        if (_myVerticalRestricion != _directionVertical)
            _vertical = vertical(_rawFocus.y);

        var _res = _horizontal + _vertical;
        return _res;
    }

    void Update()
    {

        createFrustum();
        if(_enableBounds)
            checkEdges();

    }

    void move(Transform _t , Vector3 _targetPos)
    {
        //  var _targetPos = averageFocusPts(_focusPts);
        _t.position = Vector3.SmoothDamp(_t.position, _targetPos, ref _initialVelocity, 0.2f);
    }

    Vector2 horizontal(float _val)
    {

        var _pos = Vector2.right * _val;
        return _pos;
    }


    Vector2 vertical(float _val)
    {
        var _pos = Vector2.up * _val;
        return _pos;
    }


    void checkEdges()
    {
        //check left edge
        const float _offset = 3;
        var _curPos = _midPoint.transform;

        if (!_boundManager.checkPoints(_left))
            _myHorizontalRestriction = directionHorizontal.Left;
        else if (!_boundManager.checkPoints(_right))
            _myHorizontalRestriction = directionHorizontal.Right;
        else
            _myHorizontalRestriction = directionHorizontal.None;

        if (!_boundManager.checkPoints(_up))
            _myVerticalRestricion = directionVertical.Up;
        else if (!_boundManager.checkPoints(_down))
            _myVerticalRestricion = directionVertical.Down;
        else
            _myVerticalRestricion = directionVertical.None;

    }

    Vector3 averageFocusPts(List<Transform> _tSet)
    {
        var _curPos = Vector3.zero;
        _curPos += _midPoint.transform.position;
        if(_tSet.Count>0)
            for(int _x = 0; _x<_tSet.Count; _x++)
            {
                if (!_tSet[_x])
                    continue;
                _curPos += _tSet[_x].position;
            }

        _curPos = _curPos /( _tSet.Count + 1);
        return Utilities.transformZPos(transform.position.z,_curPos);
    }

    void createFrustum()
    {
        var frustumHeight = 1.0f * _distance * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustumWidth = frustumHeight * _camera.aspect;

        //Top Right
        var pos1 = _camera.transform.position;
        pos1 += Vector3.forward * _distance;
        pos1 += (Vector3.up * frustumHeight);
        pos1 += Vector3.right * frustumWidth;

        _topRight = pos1;

        //Bottom Right
        var pos2 = _camera.transform.position;
        pos2 += Vector3.forward * _distance;
        pos2 -= (Vector3.up * frustumHeight);
        pos2 += Vector3.right * frustumWidth;

        _bottomRight = pos2;

        // Top Left
        var pos3 = _camera.transform.position;
        pos3 += Vector3.forward * _distance;
        pos3 += (Vector3.up * frustumHeight);
        pos3 -= Vector3.right * frustumWidth;

        _topLeft = pos3;

        // Bottom Left
        var pos4 = _camera.transform.position;
        pos4 += Vector3.forward * _distance;
        pos4 -= (Vector3.up * frustumHeight);
        pos4 -= Vector3.right * frustumWidth;

        _bottomLeft = pos4;



        _left.setVertices(_topLeft, _bottomLeft);
        _right.setVertices(_topRight , _bottomRight);
        _up.setVertices(_topLeft,_topRight);
        _down.setVertices(_bottomLeft, _bottomRight);

        viewFrustum(pos1, pos2, pos3, pos4);

    }

    private static GameObject CreateFrustumPoint(Vector3 pos1)
    {
        var _object = new GameObject("topRight").transform;
        _object.transform.position = pos1;

        return _object.gameObject;
    }

    private static void viewFrustum(Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4)
    {
        Debug.DrawLine(pos3, pos1, Color.red);
        Debug.DrawLine(pos4, pos2, Color.red);
        Debug.DrawLine(pos3, pos4, Color.red);
        Debug.DrawLine(pos1, pos2, Color.red);
    }

    void includePlayer(bool _decision)
    {
        if (!_decision)
            return;
         _player = GameObject.FindGameObjectWithTag("Player").transform;
         if (_player)
            _focusPts.Add(_player);

    }
}
public class UIMenu
{
    bool _decision;

    public UIMenu()
    { }
   
}


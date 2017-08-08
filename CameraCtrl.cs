using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraCtrl : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    GameObject _midPoint;

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
        //check if movement is within bounds
        move();
    }
    void Update()
    {

        createFrustum();
        checkEdges();

    }

    void move()
    {
        var _targetPos = averageFocusPts(_focusPts);
        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _initialVelocity, 0.2f);
    }


    void checkEdges()
    {
        //check left edge
        // if both vertex detected something offset mipoint by some amount
        var _leftAdjust = _boundManager.checkPoints(_left) == true ? 0f : 0.5f;
        var _rightAdjust = _boundManager.checkPoints(_right) == true ? 0f : -0.5f;
        var _upAdjust = _boundManager.checkPoints(_up) == true ? 0f : -0.5f;
        var _downAdjust = _boundManager.checkPoints(_down) == true ? 0f : 0.5f;

        var _side = Vector2.right * (_leftAdjust + _rightAdjust);
        var _nod = Vector2.up * (_upAdjust + _downAdjust);

        _midPoint.transform.localPosition = _side + _nod;


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

public class FrustumEdge
{
    public  Vector2  _vertexA,_vertexB;
    public void setVertices(Vector2 _v1, Vector2 _v2)
    {
        _vertexA = _v1;
        _vertexB = _v2;
    }
}


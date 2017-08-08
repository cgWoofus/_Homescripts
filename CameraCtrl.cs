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


	void Start ()
    {
        //get player transform  add to focus point
        includePlayer(_includePlayer);
        // instance bound manager            
        _boundManager = new CameraBound();
        if (_lvlHandle&& _enableBounds)
            _boundManager.FillBoundList(_lvlHandle);


        _frustumRight = new Vector2[] { _topRight, _bottomRight};
        _frustumLeft = new Vector2[] { _topLeft, _bottomLeft };
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //check if movement is within bounds
        move();
	}
    void Update()
    {

        createFrustum();

    }

    void move()
    {
        // var _tar =
       var _bndResult = _boundManager.checkList(_topLeft, _topRight, _bottomLeft, _bottomRight);

       var _targetPos = averageFocusPts(_focusPts);

     //  _targetPos *= _bndResult;
       if(_bndResult==1)
       transform.position = Vector3.SmoothDamp(transform.position, _targetPos , ref _initialVelocity, 0.2f);
   
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


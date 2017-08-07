using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraCtrl : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    GameObject _midPoint;

    [SerializeField]
    float _distance;
    [SerializeField]
    Transform _target;
    Vector3 _initialVelocity = Vector3.zero;

    [SerializeField]
    List<Transform> _focusPts = new List<Transform>();
    Transform _player;
	void Start ()
    {
        //get player transform  add to focus point
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        if (_player)
            _focusPts.Add(_player);
             
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        move();
	}
    void Update()
    {
        viewFrustum();

    }

    void move()
    {
       // var _tar =
        transform.position = Vector3.SmoothDamp(transform.position, averageFocusPts(_focusPts), ref _initialVelocity, 0.2f);
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


    void viewFrustum()
    {
        ///var distance = transform.position.z*-1f;
        var frustumHeight = 2.0f * _distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustumWidth = frustumHeight * Camera.main.aspect;

        //Top Right
        var pos1 = transform.position;
            pos1 += Vector3.forward * _distance;
            pos1 += (Vector3.up * frustumHeight);
            pos1 += Vector3.right * frustumWidth;
        
        //Bottom Right
        var pos2 = transform.position;
            pos2 += Vector3.forward * _distance;
            pos2 -= (Vector3.up * frustumHeight);
            pos2 += Vector3.right * frustumWidth;

        // Top Left
        var pos3 = transform.position;
            pos3 += Vector3.forward * _distance;
            pos3 += (Vector3.up * frustumHeight);
            pos3 -= Vector3.right * frustumWidth;
        // Bottom Left
        var pos4 = transform.position;
            pos4 += Vector3.forward * _distance;
            pos4 -= (Vector3.up * frustumHeight);
            pos4 -= Vector3.right * frustumWidth;



        // pos1 += transform.position;
        //    pos2 += transform.position;
        Debug.DrawLine(pos3, pos1, Color.red);
        Debug.DrawLine(pos4, pos2, Color.red);
        Debug.DrawLine(pos3, pos4, Color.red);
        Debug.DrawLine(pos1, pos2, Color.red);

    }


}

public class UIMenu
{
    bool _decision;

    public UIMenu()
    { }

}
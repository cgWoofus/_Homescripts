using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraCtrl : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    GameObject _midPoint;
    [SerializeField]
    Transform _target;
    Vector3 _initialVelocity = Vector3.zero;

    [SerializeField]
    List<Transform> _focusPts = new List<Transform>();
	void Start ()
    {
            
	}
	
	// Update is called once per frame
	void Update ()
    {
        move();
	}

    void move()
    {
        var _tar =
        transform.position = Vector3.SmoothDamp(transform.position, averageFocusPts(_focusPts), ref _initialVelocity, 0.2f);
    }

    Vector3 averageFocusPts(List<Transform> _tSet)
    {
        var _curPos = Vector3.zero;
        _curPos += _midPoint.transform.position;
        if(_tSet.Count>0)
            for(int _x = 0; _x<_tSet.Count; _x++)
            {
                _curPos += _tSet[_x].position;
            }

        _curPos = _curPos /( _tSet.Count + 1);
        return _curPos;
    }


}

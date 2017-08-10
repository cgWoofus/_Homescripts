using UnityEngine;
using System.Collections.Generic;

public class BG2Handler : MonoBehaviour
{
    [SerializeField][HideInInspector] int max, min, current;

    [SerializeField]
    public GameObject _activeObject;
   
    public List<GameObject> _BGObjectList = new List<GameObject>();

    public void setActive(int _num)
    {
        this._activeObject = _BGObjectList[_num];
    }



    private void OnValidate()
    {
        
    }

}

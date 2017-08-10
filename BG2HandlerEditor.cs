using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BG2Handler))]
public class BG2HandlerEditor : Editor
{
    BG2Handler _myInstance;
    const float _baseSpace = 30f;
    const int _maxUnitsPerPage = 5;
    private void OnEnable()
    {
      _myInstance = (BG2Handler)target;  
    }
    public override void OnInspectorGUI()
    {

        EditorGUILayout.LabelField("Active Object");        



        EditorGUILayout.ObjectField(_myInstance._activeObject,typeof(Object),false);
        GUILayout.Space(5f);


        // draw all buttons from the list

        for (int _x = 0; _x < _myInstance._BGObjectList.Count; _x++)
        {

            if (_myInstance._BGObjectList[_x] == null)
                continue;
            GUILayout.BeginHorizontal();
            if(GUILayout.Button(_myInstance._BGObjectList[_x].name))
            {
                Debug.Log(string.Format("Clicked  {0}", _x));
                _myInstance.setActive(_x);
            }
                
            GUILayout.EndHorizontal();

        }


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<"))
        {
    //        GUI.enabled = false;
        }
        if (GUILayout.Button(">"))
        { }

        GUILayout.EndHorizontal();

        GUILayout.Space(_baseSpace);
      //  base.OnInspectorGUI();

    }

}
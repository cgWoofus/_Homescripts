using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
//using System.Linq;

public static class Utilities
    {


       public static float playerPosition,startPos,bufferPos,endPos,timerRate;
       public static bool Trigger;
       public static Stack<Transform> _stackTemplate = new Stack<Transform>(50);

    //parse  string while trimming a given character
    public static int convertLayerTag(string tagName,char letter)
        {
            tagName = tagName.Trim(letter);

            int value;
            var parse = int.TryParse(tagName, out value);

            if (!parse)
            {
                Debug.Log("cant parse check object layer");
                return 999;
          
            }
            return value;
        
        }

    //parse  string while trimming a given string
    public static int convertLayerTag(string tagName, string word)
    {
        foreach(char letter in word)
          tagName = tagName.Trim(letter);

        int value;
        var parse = int.TryParse(tagName, out value);

        if (!parse)
        {
            Debug.Log("cant parse check object layer");
            return 999;

        }
        return value;
    }

    //parse  string while trimming a given string
    public static int convertLayerTag(string tagName, int indexStart,int _length)
    {

        tagName = tagName.Substring(indexStart, _length);
 
        int value;
        var parse = int.TryParse(tagName, out value);

        if (!parse)
        {
            Debug.Log("cant parse check object layer");
            return 999;

        }
        return value;
    }
    //word trim return string
    public static string trimWord(string source, string wordToRemove )
    {
        var newName = source;
        //foreach (char letter in wordToRemove)
            newName = newName.Trim(wordToRemove.ToCharArray());

        return newName;

    }

    //create digit padding
    public static string padder(int _num, int _maxDigit)
    {
        string _res;

        switch (_maxDigit)
        {

            case 2:
                _res = string.Format("{0:D2}", _num);
                break;
            case 3:
                _res = string.Format("{0:D3}", _num);
                break;
            default:
                Debug.Log("error on naming invalid maxNumber");
                _res = string.Format("{0:D2}", _num);
                break;
        }
        return _res;

    }

    //retain y and z
    public static Vector3 transformXPos(float value,Vector3 sourcePos)

        {
            var number = new Vector3(value,sourcePos.y, sourcePos.z);
            return number;

         }

    //retain x and z
    public static Vector3 transformYPos(float value, Vector3 sourcePos)
    {
        var posY = Vector3.up * value;
        var posXZ = ((Vector3.right*sourcePos.x) + (Vector3.forward)*sourcePos.z);
        return posY + posXZ;

    }

    //retain x and y
    public static Vector3 transformZPos(float value, Vector3 sourcePos)
    {
        var posZ = Vector3.forward * value;
        var posXY = ((Vector3.right * sourcePos.x) + (Vector3.up) * sourcePos.y);
        return posZ + posXY;

    }

    //convert v3 to v2
    public static Vector2[] ConvertArray(Vector3[] v3)
    {
        Vector2[] v2 = new Vector2[v3.Length];
        for (int i = 0; i < v3.Length; i++)
        {
            Vector3 tempV3 = v3[i];
            v2[i] = new Vector2(tempV3.x, tempV3.y);
        }
        return v2;
    }

    //get raycast from camera to scene returns v3
    public static Vector3 ScreenToScenePt(Vector3 v3)
    {

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v3 = Utilities.transformZPos(v3.z, pos);
        return v3;


    }

    //return parent transform
    public static GameObject GetParent(GameObject b)
    {

        if (b.transform.parent != null)
            return b.transform.parent.gameObject;
        else
            return b;

    }

    public static Transform[] GetChildren(Transform _t)
    {

        _stackTemplate.Clear();
        for (int x = 0; x < _t.childCount; x++)
            _stackTemplate.Push(_t.GetChild(x));

        return _stackTemplate.ToArray();

    }

    public static void Populate<T>(this T[] arr, T value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = value;
        }
    }

    public static FileInfo[] listOfXML(int wrldNum,int lvlNum)
    {
        var _worldString = padder(wrldNum, 2);
        var _lvlString = padder(lvlNum, 2);
        var info = new DirectoryInfo(string.Format("Assets/Resources/data/editor/{0}/{1}/", _worldString, _lvlString));
        var fileInfo = info.GetFiles("*.xml");

      //  foreach (FileInfo _a in fileInfo)
         //   Debug.Log(_a);

        return fileInfo;
    }

    public static void getList(this List<string>arr,Object[] list)
    {
        var _c = list;
        for (int i = 0; i < _c.Length; i++)
        {           
            arr.Add(_c[i].name);
        }
    }


#if UNITY_EDITOR
    public static void CreateFolder(string _rootFolder,string _MainFolder)
    {
        if (!UnityEditor.AssetDatabase.IsValidFolder(string.Format("Assets/Resources/data/{0}", _MainFolder)))
        {
            Debug.Log("folder does not exist");
            UnityEditor.AssetDatabase.CreateFolder("Assets/Resources/data", _MainFolder);
        }
        UnityEditor.AssetDatabase.Refresh();

    }
#endif

}

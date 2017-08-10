using UnityEngine;
public class FrustumEdge
{
    public  Vector2  _vertexA,_vertexB;
    public void setVertices(Vector2 _v1, Vector2 _v2)
    {
        _vertexA = _v1;
        _vertexB = _v2;
    }
}


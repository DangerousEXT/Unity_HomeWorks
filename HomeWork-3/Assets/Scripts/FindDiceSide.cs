using UnityEngine;

public class FindDiceSide
{
    public int GetTopFace(Transform cubeTransform)
    {
        var directions = new (Vector3 dir, int value)[]
        {
            (cubeTransform.up, 2),
            (-cubeTransform.up, 5),
            (cubeTransform.forward, 1),
            (-cubeTransform.forward, 6),
            (-cubeTransform.right, 3),
            (cubeTransform.right, 4)
        };
        var maxDot = -1f;
        var topFace = 0;
        foreach (var (dir, value) in directions)
        {
            var dot = Vector3.Dot(dir.normalized, Vector3.up);
            if (dot > maxDot)
            {
                maxDot = dot;
                topFace = value;
            }
        }
        return topFace;
    }
}


using UnityEngine;

public static class Extensions
{
    public static bool IsPointInsideRect(this Bounds bounds, Vector3 point)
    {
        var minX = bounds.min.x;
        var minY = bounds.min.y;
        var maxX = bounds.max.x;
        var maxY = bounds.max.y;
        
        return point.x <= maxX && point.y <= maxY && point.x >= minX && point.y >= minY;
    }
}
using System;
using UnityEngine;


public static class LineIntersection
{
    public const float kTolerance = 0.01f;

    public static bool FindIntersection(Vector2 point1, Vector2 point2,
        float x3, float y3, float x4, float y4, bool checkOnInside, out Vector2 point, float tolerance = kTolerance)
    {
        return FindIntersection(point1.x, point1.y, point2.x, point2.y, x3, y3, x4, y4, checkOnInside, out point,
        tolerance);
    }


    public static bool FindIntersection(float x1, float y1, float x2, float y2,
        float x3, float y3, float x4, float y4, bool checkOnInside, out Vector2 point, float tolerance = kTolerance)
    {
        if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance && Math.Abs(x1 - x3) < tolerance)
        {
            point = Vector2.zero;
            return false;
        }

        if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance && Math.Abs(y1 - y3) < tolerance)
        {
            point = Vector2.zero;
            return false;
        }

        if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance)
        {
            point = Vector2.zero;
            return false;
        }

        if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance)
        {
            point = Vector2.zero;
            return false;
        }
        
        float x, y;

        if (Math.Abs(x1 - x2) < tolerance)
        {
            float m2 = (y4 - y3) / (x4 - x3);
            float c2 = -m2 * x3 + y3;

            x = x1;
            y = c2 + m2 * x1;
        }
        else if (Math.Abs(x3 - x4) < tolerance)
        {
            float m1 = (y2 - y1) / (x2 - x1);
            float c1 = -m1 * x1 + y1;

            x = x3;
            y = c1 + m1 * x3;
        }
        else
        {
            float m1 = (y2 - y1) / (x2 - x1);
            float c1 = -m1 * x1 + y1;

            float m2 = (y4 - y3) / (x4 - x3);
            float c2 = -m2 * x3 + y3;

            x = (c1 - c2) / (m2 - m1);
            y = c2 + m2 * x;

            if (!(Math.Abs(-m1 * x + y - c1) < tolerance
                && Math.Abs(-m2 * x + y - c2) < tolerance))
            {
                point = Vector2.zero;
                return false;
            }
        }

        if (!checkOnInside || (IsInsideLine(x1, y1, x2, y2, x, y, tolerance) &&
            IsInsideLine(x3, y3, x4, y4, x, y, tolerance)))
        {
            point = new Vector2() { x = x, y = y };
            return true;
        }

        point = Vector2.zero;
        return false;
    }

    private static bool IsInsideLine(float x1, float y1, float x2, float y2, float x, float y, float t)
    {
        return (x >= x1 - t && x <= x2 + t
                    || x >= x2 - t && x <= x1 + t)
               && (y >= y1 - t && y <= y2 + t
                    || y >= y2 - t && y <= y1 + t);
    }
}
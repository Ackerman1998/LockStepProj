using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : Singleton<CollisionManager>
{
    /// <summary>
    /// check point collision rect
    /// </summary>
    /// <param name="pointX"></param>
    /// <param name="pointY"></param>
    /// <param name="rectX"></param>
    /// <param name="rectY"></param>
    /// <param name="rectHeight"></param>
    /// <param name="rectWidth"></param>
    /// <returns></returns>
    public bool PointCollisionRect(int pointX,int pointY,int rectX,int rectY,int rectHeight,int rectWidth) {
        if (pointX>=(rectX - rectWidth / 2) &&pointY>=(rectY-rectHeight/2)&& pointX <= (rectX + rectWidth / 2) && pointY <= (rectY - rectHeight / 2)) {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Check rect collision rect
    /// </summary>
    /// <param name="rectX"></param>
    /// <param name="rectY"></param>
    /// <param name="rectHeight"></param>
    /// <param name="rectWidth"></param>
    /// <param name="rect2X"></param>
    /// <param name="rect2Y"></param>
    /// <param name="rect2Height"></param>
    /// <param name="rect2Width"></param>
    /// <returns></returns>
    public bool RectsCollision(int rectX, int rectY, int rectHeight, int rectWidth, int rect2X, int rect2Y, int rect2Height, int rect2Width) {
        if (Mathf.Abs(rectX-rect2X)<(rectWidth/2+rect2Width/2)&& Mathf.Abs(rectY - rect2Y) <= (rectHeight / 2 + rect2Height / 2)) {
            return true;
        }
        return false;
    }

    public bool PointCollisionCircle(int x1,int y1,int circlePointX,int circlePointY, int circlePointR) {
        if (Mathf.Sqrt(Mathf.Pow(x1 - circlePointX, 2) + Mathf.Pow(y1 - circlePointY, 2)) <= circlePointR)
        {
            return true;
        }
        return false;
    }

    public bool CirclesCollisionisCollisionWithCircle(int x1, int y1, int x2, int y2,int r1, int r2)
    {
        if (Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2)) <= r1 + r2)
        {
            return true;
        }
        return false;
    }
}

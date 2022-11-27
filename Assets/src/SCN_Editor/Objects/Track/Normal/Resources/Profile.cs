using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour
{

    public Vector2[] GetTrackProfile(float trackWidth,float texHeight ,float texWidth, float texSlope)
    {
        List<Vector2> vertices = new List<Vector2>();

        vertices.Add(new Vector2(-(trackWidth/2)+texWidth+texSlope,0));
        vertices.Add(new Vector2(-(trackWidth / 2) + texWidth,texHeight));

        vertices.Add(new Vector2((trackWidth / 2) + texWidth + texSlope, 0));
        vertices.Add(new Vector2((trackWidth / 2) + texWidth, texHeight));

        //Magic

        return vertices.ToArray();
    }

    public Vector2[] GetRailProfile()
    {
        List<Vector2> vertices = new List<Vector2>();

        vertices.Add(new Vector2(-0.0625f, 0));
        vertices.Add(new Vector2(-0.007f,0.0275f));
        vertices.Add(new Vector2(-0.007f,0.0975f));
        vertices.Add(new Vector2(-0.035f,0.1092f));
        vertices.Add(new Vector2(-0.0335f,0.135f));
        vertices.Add(new Vector2(-0.0234f,0.149f));

        vertices.Add(new Vector2(0.0234f, 0.149f));
        vertices.Add(new Vector2(0.0335f, 0.135f));
        vertices.Add(new Vector2(0.035f, 0.1092f));
        vertices.Add(new Vector2(0.007f, 0.0975f));
        vertices.Add(new Vector2(0.007f, 0.0275f));
        vertices.Add(new Vector2(0.0625f, 0));


        return vertices.ToArray();
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Math
{
    public float[,] MultiplyTransformMatrix(float[,] A, float[,] B)
    {
        float[,] result = new float[4, 4];
        for (int i = 0; i < 4; i++)
        {
            for (int i2 = 0; i2 < 4; i2++)
            {
                result[i2, i] = A[0, i] * B[i2, 0] + A[1, i] * B[i2, 1] + A[2, i] * B[i2, 2] + A[3, i] * B[i2, 3];
            }
        }
        return result;
    }

    public Vector3 MultiplyTransformMatrixByVector(float[,] A, Vector3 B)
    {
        Vector3 result = new Vector3();
        result.x = A[0, 0] * B.x + A[1, 0] * B.y + A[2, 0] * B.z + A[3, 0] * 1;
        result.y = A[0, 1] * B.x + A[1, 1] * B.y + A[2, 1] * B.z + A[3, 1] * 1;
        result.z = A[0, 2] * B.x + A[1, 2] * B.y + A[2, 2] * B.z + A[3, 2] * 1;
        return result;
    }

    public Vector3 TransformMatrixToPosition(float [,] transformMatrix)
    {
        Vector3 position = new Vector3();
        position.x = transformMatrix[3, 0];
        position.y = transformMatrix[3, 1];
        position.z = transformMatrix[3, 2];
        return position;
    }

    public Vector3 TransformMatrixToScale(float[,] transformMatrix)
    {
        Vector3 scale = new Vector3();
        scale.x = transformMatrix[0, 0];
        scale.x = transformMatrix[1, 1];
        scale.x = transformMatrix[2, 2];
        return scale;
    }

    public Vector3 TransformMatrixToRotation(float[,] transformMatrix)
    {
        Vector3 rotation = new Vector3();
        rotation.x = Mathf.Acos(transformMatrix[1, 1]);
        rotation.y = Mathf.Acos(transformMatrix[0, 0]);
        rotation.z = Mathf.Acos(transformMatrix[0, 0]);
        return rotation;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Math
{
    class TransformMatrix
    {
        public TransformMatrix(float[,] _matrix)
        {
            Debug.Log(_matrix.Length);
            matrix = _matrix;
        }

        private float[,] matrix;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matrix_transforming : MonoBehaviour
{
    /* Matrix-by-Matrix multiplicing
    Macierz-przez-Macierz mnożing */ 
    public float[,] mbmmultiplicing(float[,] a,float[,] b){
        int a2 = a.Rank;
        int b2 = b.Rank;
        int[] wyma = new int[2];
        int[] wymb = new int[2];
        for(int i=1; i<=a2; i++){
            wyma[i-1] = a.GetUpperBound(i-1) + 1;
        }
        for(int i=1; i<=b2; i++){
            wymb[i-1] = b.GetUpperBound(i-1) + 1;
        }
        if(wyma[0] !=wymb[1]){
            Debug.Log("Opie ty jelopie tych macierzy nie pomnozysz");
            return null;
        } else {
            int x = wyma[0];
            int y = wyma[1];
            float[,] c= new float[wyma[0],wymb[1]];
            for(int j2=0; j2<wyma[0]; j2++){
                for(int j1=0; j1<wymb[1]; j1++){
                    c[j1,j2] = 0;
                    for(int k1 = 0; k1<wyma[0]; k1++){
                        c[j1,j2]+=a[k1,k1]*b[k1,k1];
                    } 
                }
            }
            return c;
        }
    }

    /* Matrix-by-scalar multiplicing
    Macierz-przez-rybka mnożing */
    public float[,] sbmmultiplicing(float[,] a, float b){
        int[] wyma = new int[2];
        int a2 = a.Rank;
        for(int i=1; i<=a2; i++){
            wyma[i-1] = a.GetUpperBound(i-1) + 1;
        }
        float[,] c= new float[wyma[0],wyma[1]];
        for(int j2=0; j2<wyma[0]; j2++){
            for(int j1=0; j1<wyma[1]; j1++){
                c[j1,j2] = a[j1,j2]*b;
            }
        }
        return c;
    }

    /* Matrix-by-vector multiplicing
    Macierz-przez-strzałka mnożing */
    public float[] vbmmultiplicing(float[,] a, float[] b){
        int[] wyma = new int[2];
        int a2= a.Rank;
        for(int i=1; i<=a2; i++){
            wyma[i-1] = a.GetUpperBound(i-1) + 1;
        }
        if(wyma[1] == b.Length){
            if(wyma[0]==wyma[1]){
                float[] c = new float[b.Length];
                for(int i=0;i<wyma[0];i++){
                    c[i] = a[i,i] * b[i];
                }
                return c;
            }
            return null;
        }
        else {
            Debug.Log("Tego opie nie pomnożysz");
            return null;
        }
    }

}

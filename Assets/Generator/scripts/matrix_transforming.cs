using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matrix_transforming : MonoBehaviour
{
    /* Matrix-by-Matrix multiplicing
    Macierz-przez-Macierz mnożing */ 
    public int[,] mbmmultiplicing(int[,] a,int[,] b){
        int a1, a2;
        int b1, b2;
        int[] wyma;
        int[] wymb;
        for(int i=1; i<=a2; i++){
            wyma[i-1] = a.GetUpperBound(i-1) + 1;
        }
        for(int i=1; i<=b2; i++){
            wymb[i-1] = b.GetUpperBound(i-1) + 1;
        }
        if(wyma[0] !=wymb[1]){
            Console.writeline("Opie ty jelopie tych macierzy nie pomnozysz");
        } else {
            int x = wyma[0];
            int y = wyma[1];
            int[,] c= new int[wyma[0],wymb[1]];
            for(int j2=0; i2<wyma[0]; i2++){
                for(int j1=0; i1<wymb[1]; i1++){
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
    public int[,] sbmmultiplicing(int[,] a, int b){
        int[] wyma;
        int a2 = a.Rank;
        for(int i=1; i<=a2; i++){
            wyma[i-1] = a.GetUpperBound(i-1) + 1;
        }
        int[,] c= new int[wyma[0],wyma[1]];
        for(int j2=0; i2<wyma[0]; i2++){
            for(int j1=0; i1<wyma[1]; i1++){
                c[j1,j2] = a[j1,j2]*b;
            }
        }
        return c;
    }

    /* Matrix-by-vector multiplicing
    Macierz-przez-strzałka mnożing */
    public int[] vbmmultiplicing(int[,] a, int[] b){
        int[] wyma;
        int a2= a.Rank;
        for(int i=1; i<=a2; i++){
            wyma[i-1] = a.GetUpperBound(i-1) + 1;
        }
        if(wyma[1] == b.Length){
            if(wyma[0]==wyma[1]){
                int[] c = new int[b.Length];
                for(int i=0;i<wyma[0];i++){
                    c[i] = a[i,i] * b[i];
                }
                return c;
            }
        }
        else {
            Console.writeline("Tego opie nie pomnożysz");
        }
    }


}

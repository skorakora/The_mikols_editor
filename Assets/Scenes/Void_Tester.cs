using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;
public class Void_Tester : MonoBehaviour
{
    E3D_model model = new E3D_model(@"C:\Program Files (x86)\Maszyna\dynamic\pkp\en57_v1\en57-840rb.e3d");
    void Start()
    {
        model.ToGameObject();
    }
}

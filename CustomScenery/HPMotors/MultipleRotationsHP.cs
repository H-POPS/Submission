using UnityEngine;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
[Serializable]
public class MultipleRotationsHP : motor {

    public string axisPath;
    public List<string> AxissPath = new List<string>();
    [SerializeField]
    public Transform mainAxis;
    [SerializeField]
    public List<Transform> Axiss = new List<Transform>();
    
   
    public override void Reset()
    {
        if (mainAxis)
        {
            foreach (Transform T in Axiss)
            {
                T.localRotation = mainAxis.localRotation;
            }
        }
    }
    public override void GetAxis(GameObject GO)
    {
        mainAxis = GO.transform.FindChild(axisPath);
        foreach (string path in AxissPath)
        {
            Axiss.Add(GO.transform.FindChild(path));
        }
        base.GetAxis(GO);
    }
    public void tick(float dt)
    {
        if (mainAxis)
        {
            foreach (Transform T in Axiss)
            {
                T.localRotation = mainAxis.localRotation;
            }
        }
    }
}

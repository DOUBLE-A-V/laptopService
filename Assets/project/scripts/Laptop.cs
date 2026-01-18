using System;
using UnityEngine;
using System.Collections.Generic;

public class Laptop : MonoBehaviour
{
    public string laptopName;
    public List<LaptopTarget> targets = new List<LaptopTarget>();

    public int quality = 100;

    private void OnDestroy()
    {
        foreach (LaptopTarget target in targets) Destroy(target.gameObject);
    }

    protected virtual void OnTurn()
    {
        
    }
    
    public void DoTurn()
    {
        OnTurn();
    }
}

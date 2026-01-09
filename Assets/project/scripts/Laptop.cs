using UnityEngine;
using System.Collections.Generic;

public class Laptop : MonoBehaviour
{
    public string laptopName;
    public List<LaptopTarget> targets = new List<LaptopTarget>();

    public int quality = 100;
    
    protected virtual void OnTurn()
    {
        
    }
    
    public void DoTurn()
    {
        OnTurn();
    }
}

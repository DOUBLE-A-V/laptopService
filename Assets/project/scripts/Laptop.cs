using UnityEngine;
using System.Collections.Generic;

public class Laptop : MonoBehaviour
{
    public string laptopName;
    public List<LaptopTarget> targets = new List<LaptopTarget>();

    public float completeness = 0;
}

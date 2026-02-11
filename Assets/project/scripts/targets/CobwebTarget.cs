using UnityEngine;

public class CobwebTarget : LaptopTarget
{
    [SerializeField] private int changeEnergy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnEndTurn()
    {
        Main.obj.workplace.energyBar.Change(changeEnergy);
    }
}

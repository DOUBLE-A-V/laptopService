using UnityEngine;

public class PersistentDustTarget : LaptopTarget
{
    [SerializeField] private int addEnergy;
    
    protected override void OnDeath(Tool deadBy)
    {
        Main.obj.workplace.energyBar.Change(addEnergy);
    }
}

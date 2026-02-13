using UnityEngine;

public class ClockStickerTarget : LaptopTarget
{
    [SerializeField] private int changeMinutes;

    protected override void OnDeath(Tool deadBy)
    {
        Main.obj.workplace.ChangeTime(-1, 0.5f);
    }
}

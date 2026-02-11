using UnityEngine;

public class ClockStickerTarget : LaptopTarget
{
    [SerializeField] private int changeMinutes;

    protected override void OnDeath(Tool deadBy)
    {
        StartCoroutine(Main.obj.workplace.clock.ChangeTime(-1, 0.5f));
    }
}

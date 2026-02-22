using UnityEngine;

public class FriendsStickerTarget : LaptopTarget
{
    [SerializeField] private int changeQuality;

    protected override void OnDeath(Tool deadBy)
    {
        int stack = 0;
        foreach (LaptopTarget t in Main.obj.workplace.currentLaptop.targets)
        {
            t.qualityChangeExisting += changeQuality;
            t.tip.qualityExists = t.qualityChangeExisting;
            stack += changeQuality;
        }

        deadBy.stackedQuality += stack;
    }

    protected override void OnAppear()
    {
        int stack = 0;
        foreach (LaptopTarget t in Main.obj.workplace.currentLaptop.targets)
        {
            t.qualityChangeExisting -= changeQuality;
            t.tip.qualityExists -= t.qualityChangeRemoved;
            stack -= changeQuality;
        }

        Main.obj.workplace.currentLaptop.quality += stack;
        Main.obj.workplace.UpdateQualityText(false);
    }
}

using UnityEngine;

public class FriendsStickerTarget : LaptopTarget
{
    [SerializeField] private int changeQuality;

    protected override void OnDeath(Tool deadBy)
    {
        int stack = 0;
        foreach (LaptopTarget t in Main.obj.workplace.currentLaptop.targets)
        {
            if (t.health <= 0) continue;
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
            if (t == this) continue;
            t.qualityChangeExisting -= changeQuality;
            t.tip.qualityExists = t.qualityChangeExisting;
            stack -= changeQuality;
        }

        Main.obj.workplace.currentLaptop.quality += stack;
        Main.obj.workplace.UpdateQualityText(false);
    }
}

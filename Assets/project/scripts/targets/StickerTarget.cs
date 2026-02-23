using System.Linq;
using UnityEngine;

public class StickerTarget : LaptopTarget
{
    [SerializeField] private int healthFrom;
    [SerializeField] private int healthTo;

    [SerializeField] private LaptopTarget stickyResiduePrefab;

    [SerializeField] private string dontLeaveIfDeadBy;
    
    
    protected override void OnAppear()
    {
        maxHealth = Random.Range(healthFrom, healthTo);
        health = maxHealth;
        UpdateHealth(false);
        tip.health = health;
        tip.maxHealth = maxHealth;
    }

    protected override void OnDeath(Tool deadBy)
    {
        if (deadBy.toolName != dontLeaveIfDeadBy)
        {
            LaptopTarget t = Instantiate(stickyResiduePrefab, Main.obj.workplace.transform);
            Main.obj.workplace.currentLaptop.targets.Add(t);
            t.transform.position = transform.position;
            t.transform.localScale = Vector3.one * t.size;
        }
    }
}

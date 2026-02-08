using UnityEngine;

public class StickerTarget : LaptopTarget
{
    [SerializeField] private int healthFrom;
    [SerializeField] private int healthTo;

    [SerializeField] private LaptopTarget stickyResiduePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        if (deadBy.toolName != "sticker remover")
        {
            Main.obj.workplace.currentLaptop.targets.Add(Instantiate(stickyResiduePrefab, transform.position, Quaternion.identity));
        }
    }
}

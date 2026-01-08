using DG.Tweening;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class LaptopTarget : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    public int health = 0;
    public int maxHealth;
    public string targetName;
    public int stackedDamage = 0;

    public int difficulty = 0;
    
    private SpriteRenderer spriteRenderer;

    public float size = 0.5f;

    public TipDescriptor tip;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tip.maxHealth = maxHealth;
        tip.health = health;
        UpdateHealth();
    }

    public void ApplyTool(Tool tool)
    {
        stackedDamage = tool.globalDamage;
        tool.Use(this);
        Damage(stackedDamage, tool);
        stackedDamage = 0;
        UpdateHealth();
    }

    private void UpdateHealth(bool animate = true)
    {
        healthText.text = health + " / " + maxHealth;
        if (animate)
        {
            healthText.transform.DOKill();
            healthText.transform.localScale = Vector3.one * 0.9f;
            healthText.transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic, 0.5f);
        }
        tip.maxHealth = maxHealth;
        tip.health = health;
    }
    
    protected virtual void OnDamage(int amount, Tool by)
    {
        
    }

    protected virtual void OnDeath(Tool deadBy)
    {
        
    }

    private void Death(Tool by)
    {
        OnDeath(by);
        transform.DOScale(0, 1f).SetEase(Ease.InElastic, 0.5f);
        Main.obj.workplace.currentLaptop.targets.Remove(this);
        Destroy(gameObject, 1);
    }

    public void Damage(int amount, Tool by)
    {
        spriteRenderer.DOKill();
        spriteRenderer.color = Color.red;
        spriteRenderer.DOColor(Color.green, 0.5f);
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            Death(by);
            return;
        }
        OnDamage(amount, by);
    }
}

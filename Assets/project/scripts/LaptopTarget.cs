using DG.Tweening;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Mono.Cecil.Cil;

public class LaptopTarget : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private List<Sprite> sprites;
    public CircleCollider2D collision;

    [SerializeField] private bool randomizeDirection;
    public int health = 0;
    public int maxHealth;
    public string targetName;
    public int stackedDamage = 0;

    public int difficulty = 0;
    
    [SerializeField] private SpriteRenderer spriteRenderer;

    public float size = 0.5f;

    public TipDescriptor tip;

    public int qualityChangeExisting;
    public int qualityChangeRemoved;

    public int highlightLineIndex = -1;

    public virtual void OnEndTurn()
    {
        
    }
    
    private void Awake()
    {
        tip.maxHealth = maxHealth;
        tip.health = health;
        UpdateHealth();
        
        tip.qualityRemoved = qualityChangeRemoved;
        tip.qualityExists = qualityChangeExisting;
        if (randomizeDirection) spriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        if (sprites.Count != 0) spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
    }

    private void Start()
    {
        Main.obj.workplace.currentLaptop.quality += qualityChangeExisting;
        Main.obj.workplace.UpdateQualityText();
        OnAppear();
    }

    public void ApplyTool(Tool tool)
    {
        stackedDamage = tool.globalDamage;
        tool.Use(this);
        Damage(stackedDamage, tool);
        stackedDamage = 0;
        UpdateHealth();
    }

    public void UpdateHealth(bool animate = true)
    {
        if (healthText != null)
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
    }
    
    protected virtual void OnDamage(int amount, Tool by)
    {
        
    }

    protected virtual void OnDeath(Tool deadBy)
    {
        
    }

    private void Death(Tool by)
    {
        Main.obj.SpawnEffect("removed", transform.position);
        OnDeath(by);
        by.stackedQuality += qualityChangeRemoved - qualityChangeExisting;
        transform.DOScale(0, 0.9f).SetEase(Ease.InElastic, 0.5f);
        Main.obj.workplace.currentLaptop.targets.Remove(this);
        Destroy(gameObject, 1);
    }

    public void Damage(int amount, Tool by)
    {
        if (health <= 0) return;
        if (amount > 0)
        {
            Main.obj.SpawnEffect("damage", transform.position);
        }
        health -= amount;
        if (health <= 0)
        {
            Main.obj.PlaySound("magic sound");
            health = 0;
            Death(by);
            return;
        }
        OnDamage(amount, by);
    }

    protected virtual void OnAppear()
    {
        
    }
}

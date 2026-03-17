using UnityEngine;
using TMPro;

public class ScribblesTarget : LaptopTarget
{
    private LaptopTarget bonusing;

    [SerializeField] private int addHealth;
    
    [SerializeField] private LineRenderer line;
    [SerializeField] private TMP_Text text;

    protected override void OnAppear()
    {
        line.transform.SetParent(null);
        line.transform.position = Vector3.zero;
        line.transform.localScale = Vector3.one;
        text.transform.SetParent(null);
        text.transform.localScale = Vector3.one;
    }

    private void OnDestroy()
    {
        /*
        if (bonusing != null)
        {
            bonusing.maxHealth -= addHealth;
            bonusing.health -= addHealth;
            bonusing.UpdateHealth();
        }
        */
        Destroy(line.gameObject);
        Destroy(text.gameObject);
    }

    protected override void OnDeath(Tool by)
    {
        if (bonusing != null)
        {
            bonusing.maxHealth -= addHealth;
            Main.obj.poppyTool.globalDamage = addHealth;
            Main.obj.poppyTool.stackedQuality = 0;
            bonusing.ApplyTool(Main.obj.poppyTool);
            Main.obj.workplace.ChangeQuality(Main.obj.poppyTool.stackedQuality);
            bonusing.UpdateHealth(false);
        }
    }

    private void Update()
    {
        if (health <= 0) return;
        LaptopTarget nearestTarget = null;
        float nearestDistance = float.MaxValue;
        foreach (LaptopTarget target in Main.obj.workplace.currentLaptop.targets)
        {
            if (target != this)
            {
                if (Vector2.Distance(transform.position, target.transform.position) < nearestDistance)
                {
                    nearestTarget = target;
                    nearestDistance = Vector2.Distance(transform.position, target.transform.position);
                }
            }
        }

        if (nearestTarget != null && bonusing != nearestTarget)
        {
            if (bonusing != null)
            {
                bonusing.maxHealth -= addHealth;
                bonusing.UpdateHealth(false);
                Main.obj.poppyTool.globalDamage = addHealth;
                Main.obj.poppyTool.stackedQuality = 0;
                bonusing.ApplyTool(Main.obj.poppyTool);
                Main.obj.workplace.ChangeQuality(Main.obj.poppyTool.stackedQuality);
            }
            bonusing = nearestTarget;
            bonusing.maxHealth += addHealth;
            bonusing.health += addHealth;
            bonusing.UpdateHealth(false);
            
            line.positionCount = 2;
            Vector3 tmp = transform.position;
            tmp.z = 0;
            line.SetPosition(0, tmp);
            tmp = bonusing.transform.position;
            tmp.z = 0;
            line.SetPosition(1, tmp);

            text.text = "+" + addHealth + " hp";
            text.transform.position = (bonusing.transform.position + transform.position) / 2;
        }
    }
}

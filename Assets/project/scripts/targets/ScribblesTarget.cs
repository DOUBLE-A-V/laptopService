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
    }

    private void Update()
    {
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
                bonusing.Damage(addHealth, Main.obj.poppyTool);
            }
            bonusing = nearestTarget;
            bonusing.maxHealth += addHealth;
            bonusing.Damage(-addHealth, Main.obj.poppyTool);
            
            line.positionCount = 2;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, bonusing.transform.position);

            text.text = "+" + addHealth + " hp";
            text.transform.localPosition = (bonusing.transform.position - transform.position) / 2;
        }
    }
}

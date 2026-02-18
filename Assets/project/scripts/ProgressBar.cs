using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image realBar;
    [SerializeField] private Image visualBar;
    [SerializeField] private TMP_Text text;
    
    private float timer = 0;
    public bool waiting = false;

    public float maxValue;

    public float value = 0;

    public void Change(float amount)
    {
        value += amount;
        if (value > maxValue) value = maxValue;
        if (value < 0) value = 0;
        realBar.DOKill();
        visualBar.DOKill();
        
        realBar.DOFillAmount(value/maxValue, 0.5f).SetEase(Ease.OutExpo);
        timer = 1f;
        waiting = true;
        text.text = (int)value + " / " + (int)maxValue;
    }

    public void Set(float newValue)
    {
        value = newValue;
        realBar.DOKill();
        visualBar.DOKill();
        
        realBar.fillAmount = value / maxValue;
        visualBar.fillAmount = value / maxValue;
        text.text = (int)value + " / " + (int)maxValue;
        waiting = false;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && waiting)
        {
            waiting = false;
            visualBar.DOKill();
            visualBar.DOFillAmount(value / maxValue, 1).SetEase(Ease.OutExpo);
        }
    }
}

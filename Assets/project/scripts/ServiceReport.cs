using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;

public class ServiceReport : MonoBehaviour
{
    public bool showed = false;
    [SerializeField] private Interactable closeButton;
    private List<Interactable> disabledInteractables = new List<Interactable>();

    public class ServiceQualities
    {
        public int quality;
    }
    
    public ServiceQualities qualities = new ServiceQualities();
    [SerializeField] private TMP_Text text;
    public void Show()
    {
        foreach (Interactable i in Main.interactables)
        {
            if (i.active) disabledInteractables.Add(i);
            i.active = false;
        }
        if (!Main.interactables.Contains(closeButton)) Main.interactables.Add(closeButton);
        closeButton.active = true;
        
        transform.DOKill();
        transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        showed = true;
        text.text = "<color=#efff00>REPORT</color>\nquality: " + qualities.quality +  "%\n";
        text.text += "";
    }

    public void Hide()
    {
        foreach (Interactable i in disabledInteractables) i.active = true;
        disabledInteractables.Clear();
        closeButton.active = false;
        transform.DOKill();
        transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
        showed = false;
    }
}

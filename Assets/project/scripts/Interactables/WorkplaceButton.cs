using DG.Tweening;
using UnityEngine;

public class WorkplaceButton : Interactable
{
    [SerializeField] private string target;
    protected override void Interact()
    {
        transform.DOKill();
        transform.localScale = Vector3.one * 0.9f;
        transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);

        if (target == "finish service") Main.obj.workplace.FinishService();
        else StartCoroutine(Main.obj.workplace.EndTurn());
    }
}

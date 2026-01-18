using DG.Tweening;
using UnityEngine;
using UnityEngine.Analytics;

public class CloseServiceReportButton : Interactable
{
    protected override void Interact()
    {
        transform.DOKill();
        transform.localScale = Vector3.one * 0.9f;
        transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        Main.obj.postMachine.serviceReport.Hide();
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GoToButton : Interactable
{
    [SerializeField] private Image arrowsImage;

    [SerializeField] private string goToPlace;
    
    protected override void OnHover()
    {
        arrowsImage.transform.DOKill();
        arrowsImage.transform.DOScale(new Vector3(1.5f, 1, 1), 0.5f).SetEase(Ease.OutElastic, 0.5f);
    }

    protected override void OnHoverExit()
    {
        arrowsImage.transform.DOKill();
        arrowsImage.transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic, 0.5f);
    }

    protected override void Interact()
    {
        StartCoroutine(Main.GoTo(goToPlace));
    }
}

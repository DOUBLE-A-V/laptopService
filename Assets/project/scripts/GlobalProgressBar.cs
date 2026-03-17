using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class GlobalProgressBar : MonoBehaviour
{
    public SpriteRenderer pointer;
    public SpriteRenderer sprite;

    public List<Transform> points;

    public void UpdateProgress()
    {
        pointer.transform.DOMove(points[Mathf.FloorToInt(Main.obj.reputation/100f)].position, 0.5f).SetEase(Ease.OutExpo);
    }

    public void Show()
    {
        pointer.DOFade(1, 0.5f);
        sprite.DOFade(1, 0.5f);
    }

    public void Hide()
    {
        pointer.DOFade(0, 0.5f);
        sprite.DOFade(0, 0.5f);
    }
}

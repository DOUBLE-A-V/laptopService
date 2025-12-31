using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Blackscreen : MonoBehaviour
{
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    public void Show(float time = 0.5f)
    {
        sprite.DOKill();
        sprite.DOFade(1, time);
    }

    public void Hide(float time = 0.5f)
    {
        sprite.DOKill();
        sprite.DOFade(0, time);
    }
}

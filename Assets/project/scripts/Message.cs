using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    private TMP_Text msgText;
    [SerializeField] private SpriteRenderer effect;
    private float sincounter = 0;
    public bool active = false;
    public bool canSkip = true;

    private void Start()
    {
        msgText = GetComponent<TMP_Text>();
    }
    
    public void Show(string text)
    {
        msgText.transform.SetAsLastSibling();
        msgText.DOKill();
        //effect.DOKill();
        msgText.transform.DOKill();
        
        msgText.text = text;
        msgText.color = new Color(1, 1, 1, 0);
        msgText.DOColor(Color.white, 0.5f);
        msgText.transform.localScale = new Vector3(0, 1, 1);
        msgText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
        if (canSkip)
        {
            effect.color = new Color(1, 1, 1, 0);
            effect.DOColor(Color.white, 0.5f);
        }
        else
        {
            effect.DOColor(new Color(1, 1, 1, 0), 0.5f);
        }
        active = true;
        //sincounter = 0;
    }

    public void Hide()
    {
        active = false;
        msgText.DOColor(new Color(1, 1, 1, 0), 0.5f);
        msgText.transform.DOScale(new Vector3(0, 1, 1), 0.5f).SetEase(Ease.OutExpo);
        //effect.transform.DOScale(new Vector3(0, 1, 1), 0.5f).SetEase(Ease.OutExpo);
        effect.DOColor(new Color(1, 1, 1, 0), 0.5f);
    }
    
    private void Update()
    {
        if (!active || !canSkip) return;
        sincounter += Time.deltaTime*2;
        effect.transform.localScale = new Vector3(Mathf.Sin(sincounter)*1.5f, 1.5f, 1);
        if (sincounter > 360) sincounter = 0;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            active = false;
            msgText.DOColor(new Color(1, 1, 1, 0), 0.5f);
            msgText.transform.DOScale(new Vector3(0, 1, 1), 0.5f).SetEase(Ease.OutExpo);
            //effect.transform.DOScale(new Vector3(0, 1, 1), 0.5f).SetEase(Ease.OutExpo);
            effect.DOColor(new Color(1, 1, 1, 0), 0.5f);
        }
    }
}

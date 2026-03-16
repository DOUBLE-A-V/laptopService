using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

public class Clock : Interactable
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text doublePointText;
    [SerializeField] private GameObject arrow;
    private float sincounter = 0;

    public int hours = 16;
    public int minutes = 0;

    private bool anim = false;

    [SerializeField] private TipDescriptor tip;


    protected override void OnHover()
    {
        Main.obj.workplace.tip.ShowTip(tip, transform.position);
    }

    protected override void OnHoverExit()
    {
        Main.obj.workplace.tip.HideTip();
    }

    public void ResetClock(int resetHours, int resetMinutes)
    {
        hours = resetHours;
        minutes = resetMinutes;
        UpdateText(hours, minutes);
        arrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void UpdateText(int hours, int minutes)
    {
        timeText.text = hours.ToString("00") + " " + minutes.ToString("00");
        doublePointText.text = hours.ToString("00") + ":" + minutes.ToString("00");
        if (Main.obj.workplace.isFinalLaptop)
        {
            timeText.text = timeText.text.Remove(0, 2);
            timeText.text = timeText.text.Insert(0, "??");
            
            doublePointText.text = doublePointText.text.Remove(0, 2);
            doublePointText.text = doublePointText.text.Insert(0, "??");
        }
    }
    public IEnumerator ChangeTime(float hoursAdd, float duration)
    {
        if (hoursAdd >= 24f - (hours + minutes / 60f) && hours + minutes / 60f < 23f && Random.Range(0, 3) != 0)
        {
            hoursAdd = 24f - (hours + minutes / 60f) - Random.Range(50, 200) / 1000f;
        }
        arrow.transform.DOLocalRotate(new Vector3(0, 0, arrow.transform.localRotation.eulerAngles.z + (hoursAdd > 0 ? -40 : 40)), duration).SetEase(Ease.Linear);
        if (hours == 23 && minutes + hoursAdd * 60f >= 60) Main.obj.noUpdateInteractablesTimer = 9999999;
        for (int i = 0; i < Mathf.Abs(hoursAdd) * 30; i++)
        {
            yield return null;
            minutes += hoursAdd > 0 ? 2 : -2;
            if (minutes == 60)
            {
                hours++;
                minutes = 0;
                if (hours == 24)
                {
                    hours = 0;
                    UpdateText(hours, minutes);
                    break;
                }
            } else if (minutes == -2)
            {
                hours--;
                minutes = 58;
                if (hours == 24)
                {
                    hours = 0;
                    UpdateText(hours, minutes);
                    break;
                }
            }
            UpdateText(hours, minutes);
        }

        if (hours == 0)
        {
            Main.obj.workplace.FinishService();
            transform.DOKill();
            transform.DOScale(1, 0.5f);
            anim = true;
            timeText.text = hours.ToString("00") + ":" + minutes.ToString("00");
            Main.obj.noUpdateInteractablesTimer = 9999999;
            yield return new WaitForSeconds(1);
            Main.obj.noUpdateInteractablesTimer = 9999999;
            Vector3 defPos = transform.localPosition;
            transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutExpo);
            transform.DOScale(Vector3.one * 3, 1f).SetEase(Ease.OutExpo);
            float timer = 0;
            float blinkTimer = 0;
            while (timer < 3.5f)
            {
                timer += Time.deltaTime;
                blinkTimer -= Time.deltaTime;
                if (blinkTimer <= 0)
                {
                    timeText.gameObject.SetActive(!timeText.gameObject.activeSelf);
                    blinkTimer = 0.5f;
                }

                yield return null;
            }

            transform.DOLocalMove(defPos, 0.5f).SetEase(Ease.OutExpo);
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
            timeText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Main.obj.badStampReceive.Show());
            anim = false;
        }
    }
    void Update()
    {
        if (!anim)
        {
            sincounter +=  Time.deltaTime * 2;
            if (sincounter > 360) sincounter = 0;
            doublePointText.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Sin(sincounter)));
        }
        else
        {
            doublePointText.color = new Color(1, 1, 1, 0);
        }
    }
}

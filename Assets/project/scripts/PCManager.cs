using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PCManager : MonoBehaviour
{
    [SerializeField] private AudioSource pcStartSound;
    [SerializeField] private AudioSource pcWorkSound;
    [SerializeField] private PCButton pcButton;
    [SerializeField] private DiskWorkSimulation diskWorkSim;
    [SerializeField] private AudioSource shutdownSound;

    [SerializeField] private Image workingLed;
    
    public AudioSource pipSound;
    
    public bool working = false;

    public bool blocked = false;
    
    public IEnumerator StartPC()
    {
        pcWorkSound.enabled = true;
        blocked = true;
        workingLed.color = Color.white;
        pcStartSound.Play();
        yield return new WaitForSeconds(2.4f);
        pcWorkSound.Play();
        diskWorkSim.SimWork(0.2f);
        yield return new WaitForSeconds(0.4f);
        diskWorkSim.SimWork(0.2f);
        yield return new WaitForSeconds(0.7f);
        diskWorkSim.SimWork(0.2f);
        pipSound.Play();
        working = true;
        blocked = false;
        pcButton.active = true;
    }

    public void Shutdown()
    {
        pcWorkSound.Stop();
        pcWorkSound.enabled = false;
        working = false;
        workingLed.color = new Color(0.5f, 0.5f, 0.5f, 1);
        shutdownSound.Play();
        pcButton.active = true;
    }

    public void Toggle()
    {
        if (working && !blocked) Shutdown();
        else if (!blocked) StartCoroutine(StartPC());
    }
}

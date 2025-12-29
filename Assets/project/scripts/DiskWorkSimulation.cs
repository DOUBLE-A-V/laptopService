using UnityEngine;
using UnityEngine.UI;

public class DiskWorkSimulation : MonoBehaviour
{
    public AudioSource audioSource;
    public SpriteRenderer sprite;

    private float timer = 0;

    public void SimWork(float time)
    {
        timer = time;
        if (audioSource) audioSource.Play();
        sprite.color = Color.white;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            //if (audioSource)audioSource.Stop();
            sprite.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }
}

using UnityEngine;

public class NewMessageButton : Interactable
{
    protected override void Interact()
    {
        StartCoroutine(Main.obj.pcManager.GoToMessage());
    }
}

using System;

public interface InteractModule
{
    public void Awake(Interactable _interactable);
    public bool Activate(Interactable _interactable);
}
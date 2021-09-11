using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBroadcaster : MonoBehaviour
{
    public static EventBroadcaster current;
    public int globalCounter = 0;

    void Awake()
    {
        current = this;
    }

    public event Action<int> onInteract;
    public event Action onChangeUI;

    public void Interact(int id)
    {
        if (onInteract != null)
        {
            onInteract(id);
        }
    }

    public void UiChange()
    {
        if (onChangeUI != null)
        {
            onChangeUI();
        }
    }
}

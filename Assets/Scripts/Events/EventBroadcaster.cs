using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBroadcaster : MonoBehaviour
{
    public static EventBroadcaster current;
    public int globalCounter = 0;
    [SerializeField] AudioSource audioScriptSource;

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
            if(globalCounter == 4)
            {
                audioScriptSource.Stop();
                var clip = Resources.Load<AudioClip>("Sounds/script-2") as AudioClip;
                audioScriptSource.PlayOneShot(clip);
            } else if (globalCounter == 8)
            {
                audioScriptSource.Stop();
                var clip = Resources.Load<AudioClip>("Sounds/script-3") as AudioClip;
                audioScriptSource.PlayOneShot(clip);
            }
            else if (globalCounter == 10)
            {
                audioScriptSource.Stop();
                var clip = Resources.Load<AudioClip>("Sounds/script-4") as AudioClip;
                audioScriptSource.PlayOneShot(clip);
            }
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

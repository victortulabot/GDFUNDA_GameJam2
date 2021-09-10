using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public int id;
    [SerializeField] AudioSource audioTake;

    void Start()
    {
        EventBroadcaster.current.onInteract += ContinueStory;
    }

    public void ContinueStory(int id)
    {
        if (id == this.id)
        {
            audioTake.Play();
        }
    }
}

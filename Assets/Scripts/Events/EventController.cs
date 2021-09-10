using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public GameObject item;
    public int id;

    [SerializeField] private bool isItemCleaner;
    [SerializeField] private AudioSource audioStory;

    void Start()
    {
        EventBroadcaster.current.onInteract += ContinueStory;
    }

    public void ContinueStory(int id)
    {
        if (id == this.id)
        {
            if(isItemCleaner)
            {
                //if item is an object to be cleaned put cleaning code here
            } else
            {
                this.audioStory.Play();
            }
        }
    }
}

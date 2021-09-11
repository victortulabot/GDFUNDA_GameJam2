using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public int id;
    private Animator anim;
    private bool isCleaned = false;

    [SerializeField] AudioSource audioTake;

    GameObject ebTemp;

    void Start()
    {
        EventBroadcaster.current.onInteract += ContinueStory;
        anim = GetComponent<Animator>();
        anim.enabled = false;  //disable animation states by default.  
    }

    public void ContinueStory(int id)
    {
        if (id == this.id)
        {
            if(!isCleaned)
            {
                isCleaned = true;
                anim.enabled = true;
                audioTake.Play();

                GameObject.Find("EventBroadcaster").GetComponent<EventBroadcaster>().globalCounter += 1;
                Debug.Log(GameObject.Find("EventBroadcaster").GetComponent<EventBroadcaster>().globalCounter);

            }
            
        }
    }
}

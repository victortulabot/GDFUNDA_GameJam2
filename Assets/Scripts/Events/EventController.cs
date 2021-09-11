using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public int id;
    private Animator anim;
    [SerializeField] AudioSource audioTake;
    [SerializeField] GameObject toCleanItem;

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
            anim.enabled = true;
            audioTake.Play();
        }
    }
}

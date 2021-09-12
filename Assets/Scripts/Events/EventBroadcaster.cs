using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;


public class EventBroadcaster : MonoBehaviour
{
    public static EventBroadcaster current;
    public int globalCounter = 0;
    [SerializeField] AudioSource audioScriptSource;
    public PlayableDirector playableDirector;

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
                playableDirector.Play();
                StartCoroutine(endGame());
                
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

    public IEnumerator endGame()
    {
        yield return new WaitForSeconds(2.5f);
        var clip = Resources.Load<AudioClip>("Sounds/script-4") as AudioClip;
        audioScriptSource.PlayOneShot(clip);
        yield return new WaitForSeconds(45f);
        SceneManager.LoadScene("MenuScene");
    }
}

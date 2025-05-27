using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PersistantObject : MonoBehaviour
{
   
    private XRGrabInteractable grabInteractable;

    void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // Replacer dans la scène active pour qu'il ne reste pas bloqué dans DontDestroyOnLoad
       // SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }
}

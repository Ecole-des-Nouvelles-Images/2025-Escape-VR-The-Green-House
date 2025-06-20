using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Code.Scripts.Source.XR
{
    public class PersistantObject : MonoBehaviour
    {

        private XRGrabInteractable grab;
        void Awake()
        {
            grab = GetComponent<XRGrabInteractable>();
            grab.selectEntered.AddListener(OnGrab);
            grab.selectExited.AddListener(OnRelease);
        }

        void OnDestroy()
        {
            grab.selectEntered.RemoveListener(OnGrab);
            grab.selectExited.RemoveListener(OnRelease);
        }

        void OnGrab(SelectEnterEventArgs args)
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            DontDestroyOnLoad(gameObject);
        }

        void OnRelease(SelectExitEventArgs args)
        {
            //SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        }

    }
}

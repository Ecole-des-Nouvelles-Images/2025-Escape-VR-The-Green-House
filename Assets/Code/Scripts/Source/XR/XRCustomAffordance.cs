using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

//Mettre le prefabs en enfant de l'objet contenant le meshRenderer
namespace Code.Scripts.Source.XR
{
    public class XRCustomAffordance : MonoBehaviour
    {
        private XRBaseInteractable interactable;
        private Renderer _renderer;
        private MaterialPropertyBlock _materialPropertyBlock;
        private string _highlightProperty = "_isHighLighted";

        void Awake()
        {
            interactable = GetComponentInParent<XRBaseInteractable>();
            _renderer = GetComponentInParent<Renderer>();
            _materialPropertyBlock = new MaterialPropertyBlock();

            ApplyHighlight(false); 
        }

        void OnEnable()
        {
            if (interactable != null)
            {
                interactable.hoverEntered.AddListener(OnHoverEnter);
                interactable.hoverExited.AddListener(OnHoverExit);
            }
        }

        void OnDisable()
        {
            if (interactable != null)
            {
                interactable.hoverEntered.RemoveListener(OnHoverEnter);
                interactable.hoverExited.RemoveListener(OnHoverExit);
            }
        }

        private void OnHoverEnter(HoverEnterEventArgs args)
        {
            ApplyHighlight(true);
        }

        private void OnHoverExit(HoverExitEventArgs args)
        {
            ApplyHighlight(false);
        }

        private void ApplyHighlight(bool enable)
        {
            _renderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetFloat(_highlightProperty, enable ? 1f : 0f);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
    
}
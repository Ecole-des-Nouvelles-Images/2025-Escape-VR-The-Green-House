using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

//Mettre le prefabs en enfant de l'objet contenant le meshRenderer
namespace Code.Scripts.Source.XR
{
    public class XRCustomAffordance : MonoBehaviour
    { 
        [SerializeField] private Material highlightMaterial;

        private XRBaseInteractable interactable;
        private MeshRenderer originalRenderer;
        private GameObject highlightClone;

        void Awake()
        {
            interactable = GetComponentInParent<XRBaseInteractable>();
            originalRenderer = GetComponentInParent<MeshRenderer>();

            CreateHighlightMesh();
            highlightClone.SetActive(false);
        }

        void OnEnable()
        {
            interactable.hoverEntered.AddListener(OnHoverEnter);
            interactable.hoverExited.AddListener(OnHoverExit);
        }

        void OnDisable()
        {
            interactable.hoverEntered.RemoveListener(OnHoverEnter);
            interactable.hoverExited.RemoveListener(OnHoverExit);
        }

        void OnHoverEnter(HoverEnterEventArgs args)
        {
            if (highlightClone != null)
                highlightClone.SetActive(true);
        }

        void OnHoverExit(HoverExitEventArgs args)
        {
            if (highlightClone != null)
                highlightClone.SetActive(false);
        }

        void CreateHighlightMesh()
        {
            if (originalRenderer == null) return;

            highlightClone = new GameObject("HighlighMesh");
            highlightClone.transform.SetParent(originalRenderer.transform, false);
            highlightClone.transform.localPosition = Vector3.zero;
            highlightClone.transform.localRotation = Quaternion.identity;
            highlightClone.transform.localScale = Vector3.one;

            MeshFilter meshFilter = highlightClone.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = highlightClone.AddComponent<MeshRenderer>();

            meshFilter.sharedMesh = originalRenderer.GetComponent<MeshFilter>().sharedMesh;
            Material mat = new Material(highlightMaterial);

            // Récupération de la texture du material initial
            if (originalRenderer.material.HasProperty("_Albedo"))
            {
                Texture albedoTexture = originalRenderer.material.GetTexture("_Albedo");
                mat.SetTexture("_Texture", albedoTexture);
                // texture initial dans le highlightMaterial
            }
            meshRenderer.material = mat;
        }
    }
}
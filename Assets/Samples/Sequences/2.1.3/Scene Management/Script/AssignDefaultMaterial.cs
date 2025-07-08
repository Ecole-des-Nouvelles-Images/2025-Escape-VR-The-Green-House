using UnityEngine;
using UnityEngine.Rendering;

namespace Samples.Sequences._2._1._3.Scene_Management.Script
{
    /// <summary>
    /// Custom script to assign the default material from the active RenderPipeline asset
    /// to the MeshRenderer so the sample stays compatible with URP and HDRP.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshRenderer))]
    class AssignDefaultMaterial : MonoBehaviour
    {
        private void Start()
        {
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();

            RenderPipelineAsset renderPipelineAsset = GraphicsSettings.currentRenderPipeline;
            if (renderPipelineAsset != null && renderer.sharedMaterial != renderPipelineAsset.defaultMaterial)
                renderer.sharedMaterial = renderPipelineAsset.defaultMaterial;
        }
    }
}

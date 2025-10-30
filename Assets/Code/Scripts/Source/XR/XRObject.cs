using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Code.Scripts.Source.XR
{
   
    public class XRObject : XRObjectBase
    {
        [SerializeField] private ParticleSystem _respawnParticules;
       
        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();
        }

        protected override void Respawn()
        {
            if (_respawnParticules) { _respawnParticules.Play(); }
            base.Respawn();
        }
        
        
    }
}

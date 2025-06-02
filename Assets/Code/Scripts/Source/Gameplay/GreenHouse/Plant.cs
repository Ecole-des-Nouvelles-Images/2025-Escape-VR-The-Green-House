using UnityEngine;

namespace Code.Scripts.Source.Gameplay.GreenHouse
{
    public class Plant : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _cuttingVfx;
        [SerializeField] private PlantSlot _plantSlot;

        private void Awake()
        {
            _plantSlot = GetComponentInParent<PlantSlot>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Shears"))
            {
                _plantSlot.OnPlantCut?.Invoke();
                CutPlant();
            }
        }

        private void CutPlant()
        {
            Instantiate(_cuttingVfx,transform.parent);
            Destroy(gameObject);
        }
    
    }
}

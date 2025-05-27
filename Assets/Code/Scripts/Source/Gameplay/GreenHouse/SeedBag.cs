using UnityEngine;

namespace Code.Scripts.Source.Gameplay.GreenHouse
{
    public class SeedBag : MonoBehaviour
    {
        public string PlantName => _plantName;
        [SerializeField] private string _plantName;
        public GameObject PlantPrefab => _plantPrefab;
        [SerializeField] private GameObject _plantPrefab;
    
    }
}

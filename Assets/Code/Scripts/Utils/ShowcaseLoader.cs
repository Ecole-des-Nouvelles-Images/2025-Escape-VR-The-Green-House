using UnityEngine;
using UnityEngine.UI;

using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Types;

namespace Code.Scripts.Utils
{
    public class ShowcaseLoader : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _loadHall;
        [SerializeField] private Button _loadLiving;
        [SerializeField] private Button _loadGreenhouse;
        [SerializeField] private Button _loadLaboratory;

        private void OnEnable()
        {
            _loadHall.onClick.AddListener(LoadHall);
            _loadLiving.onClick.AddListener(LoadLounge);
            _loadGreenhouse.onClick.AddListener(LoadGreenhouse);
            _loadLaboratory.onClick.AddListener(LoadLaboratory);
        }

        private void OnDisable()
        {
            _loadHall.onClick.RemoveListener(LoadHall);
            _loadLiving.onClick.RemoveListener(LoadLounge);
            _loadGreenhouse.onClick.RemoveListener(LoadGreenhouse);
            _loadLaboratory.onClick.RemoveListener(LoadLaboratory);
        }

        private void LoadLaboratory()
        {
            SceneLoader.Instance.SwitchScene(SceneType.Laboratory);
        }

        private void LoadGreenhouse()
        {
            SceneLoader.Instance.SwitchScene(SceneType.Greenhouse);
        }

        private void LoadLounge()
        {
            SceneLoader.Instance.SwitchScene(SceneType.Lounge);
        }

        private void LoadHall()
        {
            SceneLoader.Instance.SwitchScene(SceneType.Hall);
        }
    }
}

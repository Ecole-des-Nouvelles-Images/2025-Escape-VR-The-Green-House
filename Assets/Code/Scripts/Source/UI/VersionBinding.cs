using TMPro;
using UnityEngine;

namespace Code.Scripts.Source.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionBinding : MonoBehaviour
    {
        private TMP_Text _appVersion;

        void Awake()
        {
            _appVersion = GetComponentInChildren<TMP_Text>();

            if (_appVersion)
                _appVersion.text = Application.version;
        }
    }
}

using UnityEngine;
using Code.Scripts.Source.UI;

namespace Code.Scripts.Source.Managers
{
    public class SceneTransitionManager
    {
        public FadeScreen Crossfade { get; private set; }

        public static float FadeDuration { get; private set; }
        public static AnimationCurve FadeCurve { get; private set; }

        public SceneTransitionManager(float fadeDuration, AnimationCurve fadeCurve)
        {
            Crossfade = Object.FindFirstObjectByType<FadeScreen>();
            FadeDuration = fadeDuration;
            FadeCurve = fadeCurve;
        }
    }

}

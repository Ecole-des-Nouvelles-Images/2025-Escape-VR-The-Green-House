using UnityEngine;
using UnityEngine.Playables;

namespace Code.Scripts.Source.Managers
{
    public class CinematicManager : MonoBehaviour
    {
     // [SerializeField] List<PlayableDirector> _directors = new List<PlayableDirector>();
     
      public void PlayCinematic(PlayableDirector director)
      {
          director.Play();
      }
      
      public void StopCinematic(PlayableDirector director)
      {
          director.Stop();
      }
    }
}

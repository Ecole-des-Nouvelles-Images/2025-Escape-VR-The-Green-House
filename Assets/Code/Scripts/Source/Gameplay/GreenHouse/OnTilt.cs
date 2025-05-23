using UnityEngine;

public class OnTilt : MonoBehaviour
{
    private enum ToolType
    {
        WateringCan,
        SeedBag
    }
    
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private ToolType _toolType;
    [Range(0,180)] [SerializeField] private float _orientationTreshold = 40f;
    private float angle;

    private void Update()
    {
        CheckOrientation();
    }

    private void CheckOrientation()
    {
        switch (_toolType)
        {
            case ToolType.SeedBag:
                angle = Vector3.Angle(transform.up, Vector3.down);
                break;
            case ToolType.WateringCan:
                angle = Vector3.Angle(transform.forward, Vector3.down);
                break;
        }

        if (angle < _orientationTreshold)
        {
           OnWaterBegin();
        }
        else
        {
           OnWaterEnd();
        }
    }
    
    private void OnWaterBegin()
    {
        if (!_particle.isPlaying) _particle.Play();
    }
    
    private void OnWaterEnd()
    {
        if (_particle.isPlaying) _particle.Stop();
    }
    
}

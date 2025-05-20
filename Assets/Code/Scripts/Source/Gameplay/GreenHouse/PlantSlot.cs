using System;
using Code.Scripts.Source.GameFSM.States;
using UnityEngine;


public class PlantSlot : MonoBehaviour
{
    public Action OnPlantCut;

    public bool PlantGrowed { get; private set; }
    [SerializeField] private Transform _plantSpawnPoint;
    [SerializeField] private GameObject _DirtHill;
    private SeedBag _currentSeed;
    private GameObject CurrentPlantPrefab;
    private bool SeedPlanted;
    private string _currentPlantName;

    private void OnEnable()
    {
        OnPlantCut += ResetPlantSlot;
    }

    private void OnDisable()
    {
        OnPlantCut -= ResetPlantSlot;
    }

    private void PlantSeed(SeedBag seedBag)
    {
        Debug.Log("seed Plant");
        _DirtHill.SetActive(true);
        _currentSeed = seedBag;
        _currentPlantName = _currentSeed.PlantName;
        CurrentPlantPrefab = _currentSeed.PlantPrefab;
        SeedPlanted = true;
    }

    private void GrownPlant()
    {
        PlantGrowed = true;
        Debug.Log("Water Plant");
        Instantiate(CurrentPlantPrefab,_plantSpawnPoint);
        //PlantPuzzle.OnPlantGrown?.Invoke();
        GameStateGreenhouseInProgress.OnPlantGrown?.Invoke();
    }

    public string GetPlantLatinName()
    {
        return _currentPlantName;
    }

    private void ResetPlantSlot()
    {
        if (!PlantGrowed) return;

        _DirtHill.SetActive(false);
        PlantGrowed = false;
        SeedPlanted = false;
        _currentPlantName = null;
        _currentSeed = null;
        CurrentPlantPrefab = null;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (PlantGrowed) return;

        if (SeedPlanted && other.CompareTag("Water"))
        {
            GrownPlant();
        }
        else if (!SeedPlanted && other.CompareTag("Seed"))
        {
            SeedBag seedBag = other.GetComponentInParent<SeedBag>();
            if (seedBag == null) return;
            PlantSeed(seedBag);
        }
    }
}

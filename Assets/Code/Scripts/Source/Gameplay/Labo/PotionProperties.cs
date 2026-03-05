using System;
using UnityEngine;

namespace Code.Scripts.Source.Gameplay.Labo
{
    [Serializable] public class PotionProperties
    {
        public int Index { get; private set; }
        public float CorrectDose { get; private set; }
        public float CurrentDose { get; set; }
        
        public PotionProperties(PotionType type)
        {
            CurrentDose = 0.5f;
            
            switch (type)
            { 
                case PotionType.Tulénium :
                    Index = 5;
                    CorrectDose = 0.25f;
                    break;
                case PotionType.Yzora:
                    Index = 6;
                    CorrectDose = 0.75f;
                    break;
                case PotionType.Morazium:
                    Index = 7;
                    CorrectDose = 0.50f;
                    break;
                case PotionType.Veridox:
                    Index = 8;
                    CorrectDose = 1f;
                    break;
                case PotionType.Agent:
                    Index = 11;
                    CorrectDose = 0.5f;
                    break;
                case PotionType.Base:
                    Index = 15;
                    CorrectDose = 1f;
                    break;
                default:
                    Index = 0;
                    CorrectDose = 0f;
                    break;
            }
            
            Debug.Log($"PotionProperties: Index = {Index}, CorrectDose = {CorrectDose}");
        }
    }
}
using System;
using UnityEngine;
using XRI.Examples.Scripts;

namespace Code.Scripts.Source.Gameplay.Labo
{
    public class VialMixer : MonoBehaviour
    {
        [SerializeField] private Vial _vial1;
        [SerializeField] private Vial _vial2;
        [SerializeField] private XRKnob _dial1;
        [SerializeField] private XRKnob _dial2;


        private Vial _resultVial;

        private void Start()
        {
            _dial1.onValueChange.AddListener((value) => OnDialMoved(_dial1, _vial1, value));
            _dial2.onValueChange.AddListener((value) => OnDialMoved(_dial2, _vial2, value));
        }

        [ContextMenu("Mix Vials")]
        private void MixVials()
        {
            try
            {
                TryMix(_vial1, _vial2);
            }
            catch (Exception)
            {
                Debug.LogWarning("ERREUR");
            }
        }

        private PotionType TryMix(Vial vial1, Vial vial2)
        {
            int resultVial = vial1.Properties.Index + vial2.Properties.Index;

            if (Mathf.Approximately(vial1.Properties.CurrentDose, vial1.Properties.CorrectDose)
                && Mathf.Approximately(vial2.Properties.CurrentDose, vial2.Properties.CorrectDose))
            {
                switch (resultVial)
                {
                    case 11 :
                        return PotionType.Agent;
                    case 15 :
                        return PotionType.Base;
                    case 26:
                        // TODO: PotionType.Final;
                        break;
                    default:
                        throw new Exception("TryMix result in trash vial"); // TODO: Implement PotionType.Trash;
                };
            }
            throw new Exception("TryMix result bad dosage"); // TODO: Implement PotionType.Trash; // Replace when ready;
        }


        private void OnDialMoved(XRKnob dial, Vial vial, float value)
        {
            value = Mathf.Clamp(value, 0f, 1f);
            int stepIndex = Mathf.RoundToInt(value * 4);
            Debug.Log("stepIndex = " + stepIndex);
            Debug.Log("value = " + value);
            switch (stepIndex)
            {
                case 0:
                    ChangeDose(vial, dial);
                    Debug.Log("Le dial : " + dial + "a modifié "+ vial+"en Rien");
                    break;
                case 1:
                    ChangeDose(vial, dial);
                    Debug.Log("Le dial : " + dial + "a modifié "+ vial+"en Tenu");
                    break;
                case 2:
                    ChangeDose(vial, dial);
                    Debug.Log("Le dial : " + dial + "a modifié "+ vial+"en Réactif");
                    break;
                case 3:
                    ChangeDose(vial, dial);
                    Debug.Log("Le dial : " + dial + "a modifié "+ vial+"en Volatil");
                    break;
                case 4:
                    ChangeDose(vial, dial);
                    Debug.Log("Le dial : " + dial + "a modifié "+ vial+"en Corrosif");
                    break;
            }
        }

        private void ChangeDose(Vial vial, XRKnob dial)
        {
            vial.Properties.CurrentDose = dial.value;
        }
    }
}

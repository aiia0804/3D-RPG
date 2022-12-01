using UnityEngine;
using GameDevTV.Saving;
using System;

namespace RPG.Stats
{
    public class Experience:MonoBehaviour, ISaveable

    {
        [SerializeField] float experiencePoints = 0;

        public event Action onExperiencedGained;
       

        public void GainExperiencce(float experience)
        {
            experiencePoints += experience;
            onExperiencedGained();
        }

        public float ReturnExperiencePoints()
        {
            return experiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}

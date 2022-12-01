using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BasicStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject LevelUPVFX;
        [SerializeField] bool shouldHaveModifier = false;

        LazyValue<int> currentLevel;

        public event Action onLevelUPeffect;
        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(GetInIntialLevel);

        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperiencedGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperiencedGained -= UpdateLevel;
            }
        }


        private int GetInIntialLevel()
        {
            return CalculateLevel();
        }

        private void Start()
        {
            currentLevel.ForceInit();

        }

        public void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
            }
        }

        private void LevelUpEffect()
        {
            GameObject LevelVFX = Instantiate(LevelUPVFX, transform);
            onLevelUPeffect();
        }

        public float GetStats(Stat stat)
        {
            float StatValue = progression.GetStat(stat, characterClass, CalculateLevel());// 最基本的STATS
            return (StatValue + GetAdditiveModifier(stat)) * (1 + GetPercintageModifier(stat) / 100); // 其它附加數值 + 加乘百分比 ; 
        }



        public int CalculateLevel()
        {
            Experience expeience = GetComponent<Experience>();
            if (expeience == null)
            {
                return startingLevel;
            }

            float CurrentExperience = expeience.ReturnExperiencePoints();
            int penultimateLevel = progression.GetLevel(Stat.ExperienceToLevelUP, characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPtoLevelUP = progression.GetStat(Stat.ExperienceToLevelUP, characterClass, level);
                if (CurrentExperience < XPtoLevelUP)
                {
                    return level;
                }

            }
            return penultimateLevel + 1;


        }

        public int GetLevel()
        {
            if (currentLevel.value < 1)
            {
                currentLevel.value = CalculateLevel();
            }
            return currentLevel.value;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldHaveModifier) { return 0; }
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float moidifier in provider.GetAdditiveModifier(stat))
                {
                    total += moidifier;

                }

            }
            return total;

        }
        private float GetPercintageModifier(Stat stat)
        {
            if (!shouldHaveModifier) { return 0; }
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float moidifier in provider.GetPercintageModifier(stat))
                {
                    total += moidifier;

                }

            }
            return total;
        }
    }
}



using UnityEngine;
using System.Collections.Generic;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using ImportPack.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        //float HP = -1;
        bool isDeath = false;
        [SerializeField] float PerectageOfHPRecover = 50f;

        [SerializeField] UnityEvent takeDeath;
        [SerializeField] TakeDamageEvent takeDamage;
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

        LazyValue<float> HP;

        private void Awake()
        {
            HP = new LazyValue<float>(GetInIntialHealth);
        }

        private float GetInIntialHealth()
        {
            return GetComponent<BasicStats>().GetStats(Stats.Stat.Health);
        }
        public bool isdeah()
        {
            return isDeath;
        }

        private void Start()
        {
            HP.ForceInit();
        }
        private void OnEnable()
        {
            GetComponent<BasicStats>().onLevelUPeffect += ReturnHPDuringLevelUp;
        }
        private void OnDisable()
        {
            GetComponent<BasicStats>().onLevelUPeffect -= ReturnHPDuringLevelUp;

        }

        private void ReturnHPDuringLevelUp()
        {
            float regenHealth = HP.value + GetComponent<BasicStats>().GetStats(Stats.Stat.Health) * (PerectageOfHPRecover / 100);
            HP.value = Mathf.Clamp(Mathf.Max(HP.value, regenHealth), 0, GetComponent<BasicStats>().GetStats(Stats.Stat.Health));
        }

        public void GetDamage(float damage, GameObject instigator)
        {
            HP.value = Mathf.Max(HP.value - damage, 0);
            if (HP.value <= 0)
            {
                takeDeath.Invoke();
                Die();

                if (instigator.tag == "Player")
                {
                    GainExperience(instigator);
                }
                else { }
            }

            else
            {
                takeDamage.Invoke(damage);   // TODO 修正剛好死亡時不會出現浮動數值
            }
        }

        private void GainExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            experience.GainExperiencce(GetComponent<BasicStats>().GetStats(Stats.Stat.ExperienceGain));
        }

        public float returnHealthPercentage()
        {
            return (HP.value / GetComponent<BasicStats>().GetStats(Stats.Stat.Health)) * 100;
        }

        public float returnHPpoints()
        {
            return HP.value;
        }

        public float returnMaxHP()
        {
            return GetComponent<BasicStats>().GetStats(Stats.Stat.Health);
        }

        private void Die()
        {
            if (isDeath) { return; }
            GetComponent<Animator>().SetBool("Death", true);
            isDeath = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return HP.value;
        }

        public void RestoreState(object state)
        {
            HP.value = (float)state;
            if (HP.value <= 0)
            {
                Die();
            }
        }
    }
}

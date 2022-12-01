using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System.Collections;
using System;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using ImportPack.Utils;
using ImportPack.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] Transform RighHandTransform;
        [SerializeField] Transform LeftHandtransform;
        [SerializeField] string defaultWeaponName = "Unarmed";

        Health target;
        Equipment equipment;
        float timeSinceLastAttack = Mathf.Infinity;

        WeaponConfig currentWeaponConfig = null;

        LazyValue<Weapon> currentweapon;

        private void Awake()
        {
            equipment = GetComponent<Equipment>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
            currentWeaponConfig = defaultWeapon;
            currentweapon = new LazyValue<Weapon>(GetInIntialWeapon);
        }

        private void UpdateWeapon()
        {
            var weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (weapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
            }
        }

        private void Start()  
        {
            currentweapon.ForceInit();

            GetComponent<Animator>().enabled = false;
            GetComponent<Animator>().enabled = true;
        }

        private Weapon GetInIntialWeapon()
        {
            return EquipWeapon(defaultWeapon);
        }


        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) { return; }
            if (target.isdeah()) { return; }


            bool isInRange = GetIsInRange(target.transform);
            if (!isInRange)
            {
                GetComponent<Move>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                ProcessAttackBehavior();
                GetComponent<Move>().Cancel();
            }
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) <= currentWeaponConfig.GetWeaponRange();
        }

        public Weapon EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            Animator animator = GetComponent<Animator>();
            Weapon currentWeaponData = currentWeaponConfig.Spawn(RighHandTransform, LeftHandtransform, animator);
            currentweapon.value = currentWeaponData;
            return currentWeaponData;
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            if (!GetComponent<Move>().CanMove(combatTarget.transform.position)
                && !GetIsInRange(combatTarget.transform))
            {
                return false;
            }
            Health tryToAttack = combatTarget.GetComponent<Health>();
            return tryToAttack != null && !tryToAttack.isdeah();

        }

        private void ProcessAttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > currentWeaponConfig.GettimeBetweenAttack()) // This will trigger the hit Event
            {
                GetComponent<Animator>().ResetTrigger("StopAttack");
                GetComponent<Animator>().SetTrigger("Attack");
                timeSinceLastAttack = 0;
            }

        }

        void Hit()  // Call by Animation event
        {
            float playerDamage = GetComponent<BasicStats>().GetStats(Stats.Stat.Damage);

            if (currentweapon.value != null)
            {
                currentweapon.value.onHit();
            }

            if (target != null)
            {
                if (currentWeaponConfig.hasProjectille())
                {
                    currentWeaponConfig.LaunchProjectille(RighHandTransform, LeftHandtransform, target, gameObject, playerDamage);
                }
                else
                {
                    target.GetDamage(playerDamage, gameObject);
                }
            }

        }

        void Shoot()  // Call by Animation event
        {
            Hit();
        }


        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public Health GetTarget()
        {
            return target;
        }

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("StopAttack");
            target = null;
            GetComponent<Move>().Cancel();
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }


    }
}


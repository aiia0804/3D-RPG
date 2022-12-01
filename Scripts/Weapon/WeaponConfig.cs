using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using GameDevTV.Inventories;
using RPG.Stats;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName ="Weapon",menuName="Weapons/Make New Weapon", order=0)]
    public class WeaponConfig : EquipableItem, IModifierProvider
    {
        [SerializeField] Weapon equippedWeapon = null;
        [SerializeField] AnimatorOverrideController animatorOverWrite= null;
        [SerializeField] float WeaponRange;
        [SerializeField] float timeBetweenAttack;
        [SerializeField] float WeaponDamage;
        [SerializeField] float percentageBouns = 0f;
        [SerializeField] bool isRightHand;
        [SerializeField] Projectile Projectile = null;

        const string weaponName = "Weapon";

        public Weapon Spawn(Transform RightHand, Transform LeftHand, Animator animator)
        {
            DestroyOldWeapon(RightHand, LeftHand);
            Transform transform;
            transform = GetHand(RightHand, LeftHand);
            Weapon weapon = null;

            if (equippedWeapon != null)
            {
                weapon= Instantiate(equippedWeapon, transform);
                weapon.gameObject.name = weaponName;
            }
            if (animatorOverWrite != null)
            {
                animator.runtimeAnimatorController = animatorOverWrite;
            }

            return weapon;

        }

        private void DestroyOldWeapon(Transform RightHand, Transform LeftHand)
        {
            Transform oldWeapon = RightHand.Find(weaponName);
            if(oldWeapon==null)
            {
                oldWeapon = LeftHand.Find(weaponName);
            }
            if (oldWeapon == null) { return; }

            oldWeapon.name = "Destroy";

            Destroy(oldWeapon.gameObject);


        }

        public bool hasProjectille()
        {
            if (Projectile != null)
            {
                return true;
            }
            else { return false; }
        }

        public void LaunchProjectille(Transform RighHand, Transform Lefthand, Health target,GameObject instigator,float caculatedDamage)
        {
            Projectile arrowProjectileInstance= Instantiate(Projectile, GetHand(RighHand, Lefthand).transform.position, Quaternion.identity);
            arrowProjectileInstance.SetTarget(target, caculatedDamage, instigator);

            
        }

        private Transform GetHand(Transform RightHand, Transform LeftHand)
        {
            Transform transform;
            if (isRightHand) { transform = RightHand; }
            else { transform = LeftHand; }

            return transform;
        }

        public float GetWeaponRange()
        {
            return WeaponRange;
        }

        public float GettimeBetweenAttack()
        {
            return timeBetweenAttack;
        }

        public float GetWeaponDamage()
        {
            return WeaponDamage;
        }

        public float GetPercentageBouns()
        {
            return percentageBouns;
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if(stat==Stat.Damage)
            {
                yield return WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercintageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return percentageBouns;
            }
        }
    }

}
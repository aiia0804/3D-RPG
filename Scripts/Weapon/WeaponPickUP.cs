using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

namespace RPG.Combat
{
    public class WeaponPickUP : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon;
        [SerializeField] float secToHide = 7f;

        Fighter figher;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                PickUPWeapon(other.GetComponent<Fighter>());
            }

        }

        private void PickUPWeapon(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForsec(secToHide));
        }

        private IEnumerator HideForsec(float sec)
        {
            PickUPShowUP(false);
            yield return new WaitForSeconds(sec);
            PickUPShowUP(true);
        }

        private void PickUPShowUP(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRayCast(PlayerController controller)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickUPWeapon(controller.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.PickUp;
        }
    }

}
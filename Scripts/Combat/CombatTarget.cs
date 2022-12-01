using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRayCast(PlayerController controller)
        {

            if (controller.GetComponent<Fighter>().CanAttack(this.gameObject) == false)
            {
                return false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                controller.GetComponent<Fighter>().Attack(this.gameObject);
            }
            return true;

        }
    }
}

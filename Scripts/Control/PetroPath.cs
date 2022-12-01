using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PetroPath : MonoBehaviour
    {
        [SerializeField] float wayPointRadious = 1f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(GetWayPoint(i), wayPointRadious);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            }

        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;

        }

        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }




    }
}

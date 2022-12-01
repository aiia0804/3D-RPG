using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Fighter fighter;
        Health health;
        bool isDragginUI = false;


        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D CursorObject;
            public Vector2 hotSPot;
        }

        [SerializeField] CursorMapping[] mappings;

        //從中心開始1F內有沒有MESHAGENT 的路徑
        [SerializeField] float maxNavmeshProjectionDistance = 1f;
        //增加 InteractWithCompoment 與其它物件互動距離
        [SerializeField] float defaultRaycastRadious = 0.3f;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }
        void Update()
        {
            if (InteractWithUI())
            {
                return;
            }
            if (health.isdeah())
            {
                SetCursor(CursorType.Dead);
                return;
            }
            if (InteractWithCompoment()) { return; }
            if (InteractWithMovement()) { return; }
            SetCursor(CursorType.none);

        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDragginUI = false;
            }
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDragginUI = true;
                }
                SetCursor(CursorType.UI);
                return true;

            }
            if (isDragginUI)
            {
                return true;
            }


            return false;
        }


        private bool InteractWithCompoment()
        {
            RaycastHit[] hits = RaycastAllSorted();

            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastabls = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastabls)
                {
                    if (raycastable.HandleRayCast(this))
                        SetCursor(raycastable.GetCursorType()); ;
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 整理所有 GetMouseRay 照到的物體 (會讓物品可以被先後順序整理)
        /// </summary>
        /// <returns></returns>
        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), defaultRaycastRadious);
            float[] distances = new float[hits.Length];
            for (int hit = 0; hit < hits.Length; hit++)
            {
                distances[hit] = hits[hit].distance;
            }


            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithMovement()
        {
            Vector3 target; // 此TARGET 為滑鼠RAYCAST照到的點 (要MOVE TO的點)
            bool hasHit = RaycastNavMesh(out target);
            if (!GetComponent<Move>().CanMove(target)) { return false; }
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Move>().StartToMove(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)  //檢測是否為 NAVMESH可以到達區域
        {
            target = new Vector3();

            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out raycastHit);
            if (!hasHit) { return false; }

            NavMeshHit navMeshHit;
            bool hasCastToNavmesh = NavMesh.SamplePosition(raycastHit.point, out navMeshHit, maxNavmeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavmesh) return false;
            target = navMeshHit.position;


            return true;

        }



        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = SetCurosrMapping(type);
            Cursor.SetCursor(mapping.CursorObject, mapping.hotSPot, CursorMode.Auto);
        }

        private CursorMapping SetCurosrMapping(CursorType type)
        {
            foreach (CursorMapping mapping in mappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }

            }
            return mappings[0];
        }


        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }


    }
}

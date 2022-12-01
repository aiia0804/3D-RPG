using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;
using RPG.Attributes;
using System;
using ImportPack.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chasingRange = 3f;
        [SerializeField] float timeToSuspicion = 3f;
        [SerializeField] PetroPath petroPath;
        [SerializeField] float wayPointTolerance = 3f;
        [SerializeField] float dwelingTimeBetweenPeriod = 1f;
        [Range(0, 1)]
        [SerializeField] float petroSpeedFraction = 0.2f;
        [SerializeField] float coolDownTimeForAggrevated = 8f;
        [SerializeField] float shoutDistance = 5f;

        GameObject player;
        Fighter fighter;
        Health health;
        LazyValue<Vector3> guardPosition;
        Move move;
        bool playerDeath;
        float lastTimeSeePlayer = Mathf.Infinity;
        float lastTimeToWayPoint = Mathf.Infinity;
        [SerializeField] float lastTImeOfAggraavted = Mathf.Infinity;
        int currentWayPointIndex = 0;

        private void Awake()
        {
            move = GetComponent<Move>();
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            guardPosition = new LazyValue<Vector3>(GetInIntialguardPosition);
        }
        private Vector3 GetInIntialguardPosition()
        {
            return this.transform.position;
        }
        void Update()
        {
            playerDeath = player.GetComponent<Health>().isdeah();

            if (health.isdeah()) { return; }

            if (InTheAggreivated() && player && !playerDeath)
            {
                lastTimeSeePlayer = 0;
                AttackBehavior();
                AggraivatedNearbyEnemies();

            }
            else if (lastTimeSeePlayer <= timeToSuspicion)
            {
                SuspicionBehavior();
            }
            else
            {
                PetroBehavior();
            }
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            lastTimeToWayPoint += Time.deltaTime;
            lastTimeSeePlayer += Time.deltaTime;
            lastTImeOfAggraavted += Time.deltaTime;
        }

        private void PetroBehavior()
        {
            Vector3 NextPosition;

            if (petroPath != null)
            {
                if (AtWayPoint())
                {
                    CycleWayPoint();
                    lastTimeToWayPoint = 0;
                }
                NextPosition = GetCurrentPosition();

                if (lastTimeToWayPoint >= dwelingTimeBetweenPeriod)
                {
                    move.MoveToin2D(NextPosition, petroSpeedFraction);
                }
            }
            else
            {
                move.StartToMove(guardPosition.value, petroSpeedFraction);
            }
        }

        public void ActiveAggreviatedMode()
        {
            lastTImeOfAggraavted = 0;
        }

        private Vector3 GetCurrentPosition()
        {
            return petroPath.GetWayPoint(currentWayPointIndex);
        }

        private void CycleWayPoint()
        {
            currentWayPointIndex = petroPath.GetNextIndex(currentWayPointIndex);
        }

        private bool AtWayPoint()
        {
            Vector3 A = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 B = new Vector3(GetCurrentPosition().x, 0, GetCurrentPosition().z);

            float distanceToWayPoint = Vector3.Distance(A, B);
            return distanceToWayPoint < wayPointTolerance;
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            GetComponent<Fighter>().Attack(player);
        }

        private void AggraivatedNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController aiController = hit.transform.GetComponent<AIController>();
                if (aiController == null) { continue; }

                aiController.ActiveAggreviatedMode();

            }
        }

        private bool InTheAggreivated()
        {

            float ChaseDistance = Vector3.Distance(player.transform.position, this.transform.position);
            bool inTheCHasingRange = ChaseDistance < chasingRange;
            bool inTheAggreivatedTimeZone = lastTImeOfAggraavted < coolDownTimeForAggrevated;
            return inTheAggreivatedTimeZone || inTheCHasingRange;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 1, 0, 0.75F);
            Gizmos.DrawWireSphere(transform.position, chasingRange);
        }
    }
}



using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using GameDevTV.Saving;
using System.Collections.Generic;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Move : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float MaxMovingSpeed = 6f;
        [SerializeField] float maxNavPathLength = 30f;


        NavMeshAgent navMeshAgent;
        Health health;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        void Update()
        {
            navMeshAgent.enabled = !health.isdeah();
            UpdateAnimator();
        }

        public void StartToMove(Vector3 raycastHit, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);

            MoveTo(raycastHit, speedFraction);

        }

        public bool CanMove(Vector3 target)
        {

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);

            if (!hasPath) { return false; }
            if (path.status != NavMeshPathStatus.PathComplete) { return false; }


            if (GetPathLength(path) > maxNavPathLength) { return false; }
            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) { return total; }
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += (Vector3.Distance(path.corners[i], path.corners[i + 1]));
            }

            return total;
        }

        public void MoveTo(Vector3 raycastHit, float speedFraction)
        {
            navMeshAgent.speed = MaxMovingSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.SetDestination(raycastHit);
            navMeshAgent.isStopped = false;
        }

        public void MoveToin2D(Vector3 raycastHit, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            Vector3 Movein2D = new Vector3(raycastHit.x, 0, raycastHit.z);
            MoveTo(Movein2D, speedFraction);

        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotaion"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotaion"]).ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}


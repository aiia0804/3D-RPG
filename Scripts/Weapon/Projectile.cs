using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;


namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float FlyingSpeed = 1f;
        [SerializeField] bool ishoming = true;
        [SerializeField] GameObject ImpactVFX = null;
        [SerializeField] GameObject[] destroyOnHit;
        [SerializeField] float EffectLeftTime = 0.5f;
        [SerializeField] UnityEvent ProjectileHit=null;
        [SerializeField] UnityEvent ProjectleFire = null;

        Health target = null;
        float damage;
        GameObject instigator = null;

        private void Start()
        {
            transform.LookAt(aimTarget());

        }

        void Update()
        {
            if (ishoming && !target.isdeah())
            {
                transform.LookAt(aimTarget());
            }
            transform.Translate(Vector3.forward * Time.deltaTime * FlyingSpeed);

        }



        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Health>() != target) { return; }
            if (target.isdeah()) { return; }
            ProjectileHit.Invoke();
            FlyingSpeed = 0;
            target.GetDamage(damage, instigator);
            GenerateImpactVFX();

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, EffectLeftTime);


        }

        private void GenerateImpactVFX()
        {
            if (ImpactVFX == null) { return; }
            GameObject VFX = Instantiate(ImpactVFX, aimTarget(), Quaternion.identity);
            Destroy(VFX, 1);

        }

        public void SetTarget(Health target, float damage, GameObject instigator)
        {
            ProjectleFire.Invoke();
            this.target = target;
            this.damage = damage;
            Destroy(gameObject, 5f);
            this.instigator = instigator;


        }


        private Vector3 aimTarget()
        {

            CapsuleCollider targetCapusleCollider = target.GetComponent<CapsuleCollider>();
            if (targetCapusleCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapusleCollider.height / 2;
        }
    }
}


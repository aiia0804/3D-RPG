using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] GameObject target;
        void LateUpdate()
        {
            this.transform.position = target.transform.position;
        }
    }

}

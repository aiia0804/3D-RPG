using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Saving;

namespace RPG.Cinematic
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {

        PlayableDirector playable;
        bool ActiveAlready = false;
        private void Start()
        {
            playable = GetComponent<PlayableDirector>();
        }
        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == "Player" && !ActiveAlready)
            {
                playable.Play();
                ActiveAlready = true;
            }

        }

        public object CaptureState()
        {
            return ActiveAlready;
        }

        public void RestoreState(object state)
        {
            bool status = (bool)state;
            ActiveAlready = status;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{    public class AudioController : MonoBehaviour
    {
        [SerializeField] AudioClip[] hurtSFX;
        [SerializeField] AudioClip[] deathSFX;
        [SerializeField] AudioClip[] FireBallHitSFX;
        [SerializeField] AudioClip FireBallFireSFX;
        [SerializeField] AudioClip[] SwordHitSFX;
        [SerializeField] AudioClip[] PuhcnSFX;

        public void PlayHurtVFX()
        {
            AudioClip clip = hurtSFX[UnityEngine.Random.Range(0, hurtSFX.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position);

        }

        public void PlayDeathVFX()
        {
            AudioClip clip = deathSFX[UnityEngine.Random.Range(0, deathSFX.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }

        public void PlayFireBallHit()
        {
            AudioClip clip = FireBallHitSFX[UnityEngine.Random.Range(0, FireBallHitSFX.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position);

        }
        public void PlayFireBallFire()
        {
            //AudioClip clip = hurtSFX[UnityEngine.Random.Range(0, FireBallHitSFX.Length)];
            //print("TEST");
            AudioSource.PlayClipAtPoint(FireBallFireSFX, transform.position);

        }

        public void PlaySwordHit()
        {
            AudioClip clip = SwordHitSFX[UnityEngine.Random.Range(0, SwordHitSFX.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
        public void PlayPunchHit()
        {
            AudioClip clip = PuhcnSFX[UnityEngine.Random.Range(0, PuhcnSFX.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }


    }
}

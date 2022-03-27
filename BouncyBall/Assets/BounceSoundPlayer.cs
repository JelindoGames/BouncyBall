using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceSoundPlayer : MonoBehaviour
{
    public enum BounceType
    {
        Landing,
        Bounce,
        PerfectBounce,
        Jump
    }

    [SerializeField] AudioClip perfectBounce;
    [SerializeField] AudioClip stdBounce;
    [SerializeField] AudioClip jumpBounce;
    [SerializeField] AudioClip landing;
    [SerializeField] GameObject audioPlayer;
    [SerializeField] float minTimeBtwn; // Minimum time between plays (stops multiple frames in a row w/same sfx)
    bool playable = true;

    public void Play(BounceType bt)
    {
        if (!playable)
        {
            return;
        }

        switch (bt)
        {
            case BounceType.Landing:
                SpawnSound(landing);
                break;
            case BounceType.Bounce:
                SpawnSound(stdBounce);
                break;
            case BounceType.Jump:
                SpawnSound(jumpBounce);
                break;
            case BounceType.PerfectBounce:
                SpawnSound(perfectBounce);
                break;
            default:
                break;
        }

        playable = false;
        Invoke("MakePlayable", minTimeBtwn);
    }

    void MakePlayable()
    {
        playable = true;
    }

    void SpawnSound(AudioClip sound)
    {
        AudioSource audio = ((GameObject)Instantiate(audioPlayer, transform.position, Quaternion.identity)).GetComponent<AudioSource>();
        audio.clip = sound;
        audio.Play();
    }
}

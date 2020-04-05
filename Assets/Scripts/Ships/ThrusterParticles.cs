using System;
using UnityEngine;

[Serializable]
public class ThrusterParticles : IShipThruster
{
    public ParticleSystem particles;
    public Vector3 positiveRotation;
    public Vector3 negativeRotation = new Vector3(0, 0, 180);
    public void EngageThruster(float input)
    {
        if (particles != null && particles.isPlaying == false)
        {
            particles.Play(true);
            if(input >= 0)particles.transform.rotation = Quaternion.Euler(positiveRotation);
            else particles.transform.rotation = Quaternion.Euler(negativeRotation);
        }
    }

    public void DisengageThruster()
    {
        if (particles != null && particles.isPlaying) particles.Stop(true);
    }
}
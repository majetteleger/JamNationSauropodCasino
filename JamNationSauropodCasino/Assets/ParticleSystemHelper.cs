using UnityEngine;

public class ParticleSystemHelper : MonoBehaviour
{
    public ParticleSystem Target;

    public void DisableEmission()
    {
        var emission = Target.emission;
        emission.enabled = false;
    }
}

using UnityEngine;

public class RocketController : MonoBehaviour
{
    [Header("Movement")]
    public float flySpeed = 15f;
    public float acceleration = 5f;
    private bool isLaunching = false;
    private float currentSpeed = 0f;

    [Header("Visual Effects")]
    public ParticleSystem fireParticles;
    public ParticleSystem smokeParticles;
    public GameObject tractorBeam;

    void Start()
    {
        if (fireParticles) fireParticles.Stop();
        if (smokeParticles) smokeParticles.Stop();

        if (tractorBeam) tractorBeam.SetActive(true);
    }

    void Update()
    {
        if (isLaunching)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, flySpeed);

            transform.Translate(currentSpeed * Time.deltaTime * Vector3.up, Space.World);

            if (transform.position.y > 500f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void IgniteAndLaunch()
    {
        if (isLaunching) return;

        if (tractorBeam) tractorBeam.SetActive(false);

        if (fireParticles) fireParticles.Play();
        if (smokeParticles) smokeParticles.Play();

        isLaunching = true;
    }
}
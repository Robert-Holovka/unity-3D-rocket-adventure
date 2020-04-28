using UnityEngine;

namespace Scripts
{
    internal class Rocket : MonoBehaviour
    {
        [Header("Engine parameters")]
        [SerializeField] float rcsThrust = 100f;
        [SerializeField] float mainThrust = 100f;

        [Header("Engine Effects")]
        [SerializeField] AudioClip mainEngineAudio = default;
        [SerializeField] ParticleSystem mainEngineParticles = default;

        // Cached 
        private Rigidbody rigidBody;
        private AudioSource audioSource;
        private LevelManager levelManager;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            levelManager = FindObjectOfType<LevelManager>();
        }

        private void Update()
        {
            if (LevelManager.IsTranscending) return;

            Thrust();
            Rotate();
        }

        private void Thrust()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rigidBody.AddRelativeForce(Vector3.up * mainThrust);
                // Prevent layering
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(mainEngineAudio);
                    mainEngineParticles.Play();
                }
            }
            else
            {
                audioSource.Stop();
                mainEngineParticles.Stop();
            }
        }

        private void Rotate()
        {
            rigidBody.freezeRotation = true;
            float rotationSpeed = rcsThrust * Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(Vector3.forward * rotationSpeed);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(-Vector3.forward * rotationSpeed);
            }

            rigidBody.freezeRotation = false;
        }

        private void OnLevelFinish(bool success) => levelManager.OnLevelFinish(success, transform.position);

        private void OnCollisionEnter(Collision collision)
        {
            if (LevelManager.IsTranscending || !DebugManager.IsCollisionEnabled)
            {
                return;
            }

            switch (collision.gameObject.tag)
            {
                case "Finish":
                    OnLevelFinish(true);
                    break;
                case "Friendly":
                    break;
                default:
                    OnLevelFinish(false);
                    break;
            }
        }
    }
}

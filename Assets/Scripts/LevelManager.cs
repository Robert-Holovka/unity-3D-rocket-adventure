using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    internal class LevelManager : MonoBehaviour
    {
        [SerializeField] float levelLoadDelay = 1f;

        [Header("Audio Files")]
        [SerializeField] AudioClip successAudio = default;
        [SerializeField] AudioClip deathAudio = default;

        [Header("Particle Systems")]
        [SerializeField] ParticleSystem successParticles = default;
        [SerializeField] ParticleSystem deathParticles = default;

        internal static bool IsTranscending { get; set; }
        private AudioSource audioSource;

        private void Awake()
        {
            if (FindObjectsOfType<LevelManager>().Length > 1)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelLoaded;
        }

        private void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            IsTranscending = false;
        }

        internal void OnLevelFinish(bool success, Vector3 playerPosition)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(success ? successAudio : deathAudio);
            Instantiate(success ? successParticles : deathParticles,
                        playerPosition,
                        Quaternion.identity);

            IsTranscending = true;
            StartCoroutine((success) ? LoadNextScene(levelLoadDelay)
                                     : LoadFirstScene(levelLoadDelay));
        }

        internal static IEnumerator LoadFirstScene(float levelLoadDelay)
        {
            yield return new WaitForSeconds(levelLoadDelay);
            SceneManager.LoadScene(0);
        }

        internal static IEnumerator LoadNextScene(float levelLoadDelay)
        {
            yield return new WaitForSeconds(levelLoadDelay);

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneindex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextSceneindex);
        }
    }
}

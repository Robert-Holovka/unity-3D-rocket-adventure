using UnityEngine;

namespace Scripts
{
    [DisallowMultipleComponent]
    internal class Oscillator : MonoBehaviour
    {
        [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
        [SerializeField] float period = 2f;

        [Range(0, 1)]
        [Tooltip("0 == don't move, 1 == fully move")]
        [SerializeField] // For visualization purposes only
        float movementFactor;

        private Vector3 startingPosition;
        private const float tau = 2 * Mathf.PI;

        void Start()
        {
            startingPosition = transform.position;
        }

        void Update()
        {
            if (period <= Mathf.Epsilon)
            {
                return;
            }

            float cycles = Time.time / period;
            float rawSinWave = Mathf.Sin(cycles * tau);

            movementFactor = rawSinWave / 2f + 0.5f; // [0, 1] sin wave
            Vector3 offset = movementFactor * movementVector;
            transform.position = startingPosition + offset;
        }
    }
}
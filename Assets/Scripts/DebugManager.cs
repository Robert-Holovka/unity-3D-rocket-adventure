using UnityEngine;

namespace Scripts
{
    internal class DebugManager : MonoBehaviour
    {
        internal static bool IsCollisionEnabled { get; set; } = true;

        private void Update()
        {
            if (Debug.isDebugBuild)
            {
                RespondToDebugKeys();
            }
        }

        private void RespondToDebugKeys()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                StartCoroutine(LevelManager.LoadNextScene(0));
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(LevelManager.LoadFirstScene(0));
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                IsCollisionEnabled = !IsCollisionEnabled;
            }
        }
    }
}

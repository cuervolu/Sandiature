using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance { get; private set; }

        [Header("Scene Transition")] [SerializeField]
        private Animator transitionAnim;

        [Header("Loading Screen")] [SerializeField]
        private GameObject loadingScreen;

        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text progressText;

        #region Hashed Properties

        private static readonly int End = Animator.StringToHash("End");
        private static readonly int Start = Animator.StringToHash("Start");

        #endregion
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void NextLevel()
        {
            StartCoroutine(LoadLevel());
        }

        private IEnumerator LoadLevel()
        {
            transitionAnim.SetTrigger(End);
            yield return new WaitForSeconds(1);
            var operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            loadingScreen.SetActive(true);
            while (operation.isDone == false)
            {
                var progress = Mathf.Clamp01(operation.progress / .9f);
                slider.value = progress;
                progressText.text = progress * 100f + "%";
                yield return null;
            }

            transitionAnim.SetTrigger(Start);
            loadingScreen.SetActive(false);
        }
    }
}
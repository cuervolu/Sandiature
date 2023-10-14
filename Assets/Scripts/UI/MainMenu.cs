using Core;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneController.Instance.NextLevel();
        }
        
        public void Quit()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}

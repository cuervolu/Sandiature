using UnityEngine;

namespace Core
{
    /// <summary>
    /// Clase que gestiona la reproducción de sonidos en el juego.
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        /// <summary>
        /// Instancia única del SoundManager.
        /// </summary>
        public static SoundManager Instance { get; private set; }

        private AudioSource _source; // Componente AudioSource para reproducir sonidos.

        private void Awake()
        {
            // Configura la instancia única.
            Instance = this;

            // Obtiene el componente AudioSource en el objeto actual.
            _source = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Reproduce un sonido una sola vez.
        /// </summary>
        /// <param name="sound">El AudioClip que se va a reproducir.</param>
        public void PlaySound(AudioClip sound)
        {
            // Reproduce el sonido una sola vez.
            _source.PlayOneShot(sound);
        }
    }
}
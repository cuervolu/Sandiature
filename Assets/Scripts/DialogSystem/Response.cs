using UnityEngine;

namespace DialogSystem
{
    /// <summary>
    /// Clase que representa una respuesta en un cuadro de diálogo.
    /// </summary>
    [System.Serializable]
    public class Response
    {
        [Header("Configuración de Respuesta")] [SerializeField]
        private string responseText; // El texto de la respuesta.

        [SerializeField] private DialogueObject dialogueObject; // El objeto de diálogo asociado a esta respuesta.
        [SerializeField] private Sprite characterSprite; // La imagen del personaje asociado a esta respuesta.
        [SerializeField] private AudioClip audioClip; // El clip de audio asociado a esta respuesta.

        /// <summary>
        /// Obtiene el texto de la respuesta.
        /// </summary>
        public string ResponseText => responseText;

        /// <summary>
        /// Obtiene el objeto de diálogo asociado a esta respuesta.
        /// </summary>
        public DialogueObject DialogueObject => dialogueObject;

        /// <summary>
        /// Obtiene la imagen del personaje asociado a esta respuesta.
        /// </summary>
        public Sprite CharacterSprite => characterSprite;

        /// <summary>
        /// Obtiene el clip de audio asociado a esta respuesta.
        /// </summary>
        public AudioClip AudioClip => audioClip;
    }
}
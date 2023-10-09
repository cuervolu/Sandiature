using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem
{
    /// <summary>
    /// Clase que define un evento de respuesta en un diálogo.
    /// </summary>
    [System.Serializable]
    public class ResponseEvent
    {
        [HideInInspector] public string name; // Nombre oculto para identificación en el editor.

        [SerializeField]
        private UnityEvent onPickedResponse; // Evento Unity que se dispara cuando se elige una respuesta.

        /// <summary>
        /// Evento que se dispara cuando se elige una respuesta en el diálogo.
        /// </summary>
        public UnityEvent OnPickedResponse => onPickedResponse;
    }
}
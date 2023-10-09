using System;
using UnityEngine;

namespace DialogSystem
{
    /// <summary>
    /// Clase que gestiona los eventos de respuesta asociados a un objeto de diálogo.
    /// </summary>
    public class DialogueResponseEvents : MonoBehaviour
    {
        [SerializeField] private DialogueObject dialogueObject;
        [SerializeField] private ResponseEvent[] events;

        /// <summary>
        /// Obtiene el objeto de diálogo asociado a esta instancia.
        /// </summary>
        public DialogueObject DialogueObject => dialogueObject;

        /// <summary>
        /// Obtiene los eventos de respuesta asociados a este objeto de diálogo.
        /// </summary>
        public ResponseEvent[] Events => events;

        /// <summary>
        /// Método llamado en el editor para validar y sincronizar los eventos de respuesta con las respuestas del diálogo.
        /// </summary>
        public void OnValidate()
        {
            if (dialogueObject == null) return;
            if (dialogueObject.Responses == null) return;
            if (events != null && events.Length == dialogueObject.Responses.Length) return;

            if (events == null)
                events = new ResponseEvent[dialogueObject.Responses.Length];
            else
                Array.Resize(ref events, dialogueObject.Responses.Length);

            for (var i = 0; i < dialogueObject.Responses.Length; i++)
            {
                var response = dialogueObject.Responses[i];

                if (events[i] != null)
                {
                    events[i].name = response.ResponseText;
                    continue;
                }

                events[i] = new ResponseEvent() { name = response.ResponseText };
            }
        }
    }
}
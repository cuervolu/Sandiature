using Player;
using UnityEngine;

namespace DialogSystem
{
    /// <summary>
    /// Clase que activa un diálogo cuando el jugador interactúa con un objeto en el juego.
    /// </summary>
    public class DialogueActivator : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueObject dialogueObject;

        /// <summary>
        /// Actualiza el objeto de diálogo asociado al activador.
        /// </summary>
        /// <param name="newDialogueObject">El nuevo objeto de diálogo.</param>
        public void UpdateDialogueObject(DialogueObject newDialogueObject)
        {
            this.dialogueObject = newDialogueObject;
        }

        /// <summary>
        /// Método llamado cuando el jugador interactúa con el objeto.
        /// </summary>
        /// <param name="player">El controlador del jugador.</param>
        public void Interact(PlayerController player)
        {
            // Busca y agrega eventos de respuesta asociados al diálogo.
            foreach (var responseEvents in GetComponents<DialogueResponseEvents>())
            {
                if (responseEvents.DialogueObject == dialogueObject)
                {
                    player.DialogueUi.AddResponseEvents(responseEvents.Events);
                    break;
                }
            }

            // Muestra el diálogo en la interfaz de usuario del jugador.
            player.DialogueUi.ShowDialogue(dialogueObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Verifica si el jugador entra en el área del activador y lo establece como interactuable.
            if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
            {
                player.Interactable = this;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // Verifica si el jugador sale del área del activador y lo deshabilita como interactuable.
            if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
            {
                if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
                    player.Interactable = null;
            }
        }
    }
}
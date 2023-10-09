using System.Collections;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogSystem
{
    /// <summary>
    /// Clase que gestiona la interfaz de usuario del sistema de diálogo en el juego.
    /// </summary>
    public class DialogueUI : MonoBehaviour
    {
        [Header("UI Elements")] [SerializeField]
        private GameObject dialogueBox;

        [SerializeField] private TMP_Text textLabel;
        [SerializeField] private PlayerController playerInput;
        [SerializeField] private Image imageHolder;

        /// <summary>
        /// Indica si el cuadro de diálogo está abierto.
        /// </summary>
        public bool IsOpen { get; private set; }

        private TypewriterEffect _typewriterEffect;
        private ResponseHandler _responseHandler;

        private void Start()
        {
            _typewriterEffect = GetComponent<TypewriterEffect>();
            _responseHandler = GetComponent<ResponseHandler>();
            imageHolder.gameObject.SetActive(false);
            CloseDialogBox();
        }

        /// <summary>
        /// Muestra un diálogo en la interfaz de usuario.
        /// </summary>
        /// <param name="dialogueObject">El objeto de diálogo que se va a mostrar.</param>
        public void ShowDialogue(DialogueObject dialogueObject)
        {
            IsOpen = true;
            dialogueBox.SetActive(true);
            imageHolder.gameObject.SetActive(true);
            imageHolder.sprite = dialogueObject.Sprite;
            imageHolder.preserveAspect = true;
            StartCoroutine(StepThroughDialogue(dialogueObject));
        }

        /// <summary>
        /// Agrega eventos de respuesta al manejador de respuestas.
        /// </summary>
        /// <param name="responseEvents">Los eventos de respuesta que se agregarán.</param>
        public void AddResponseEvents(ResponseEvent[] responseEvents)
        {
            _responseHandler.AddResponseEvents(responseEvents);
        }

        /// <summary>
        /// Realiza el proceso de avanzar a través del diálogo, mostrando cada línea de diálogo de manera progresiva.
        /// </summary>
        /// <param name="dialogueObject">El objeto de diálogo que se va a mostrar.</param>
        private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
        {
            for (var i = 0; i < dialogueObject.Dialogue.Length; i++)
            {
                var dialogue = dialogueObject.Dialogue[i];

                // Ejecuta el efecto de máquina de escribir en el texto del diálogo.
                yield return RunTypewriterEffect(dialogue, dialogueObject.FontAsset, dialogueObject.TextColor,
                    dialogueObject.AudioClip);

                // Actualiza el texto del diálogo.
                textLabel.text = dialogue;

                // Reinicia la señal de avanzar al siguiente diálogo.
                playerInput.NextDialogue = false;

                // Si es la última línea de diálogo y hay respuestas, detén el ciclo.
                if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

                yield return null;

                // Espera hasta que se detecte la señal de avanzar al siguiente diálogo.
                yield return new WaitUntil(() => playerInput.OnNextDialogue());
            }

            // Muestra las respuestas si existen, de lo contrario, cierra el cuadro de diálogo.
            if (dialogueObject.HasResponses)
                _responseHandler.ShowResponses(dialogueObject.Responses);
            else
            {
                CloseDialogBox();
            }
        }

        /// <summary>
        /// Ejecuta el efecto de máquina de escribir en el texto del diálogo.
        /// </summary>
        /// <param name="dialogue">El texto del diálogo que se va a mostrar.</param>
        /// <param name="fontAsset">La fuente de texto a aplicar al diálogo.</param>
        /// <param name="textColor">El color del texto del diálogo.</param>
        /// <param name="audioClip">El clip de audio asociado al diálogo.</param>
        private IEnumerator RunTypewriterEffect(string dialogue, TMP_FontAsset fontAsset, Color textColor,
            AudioClip audioClip)
        {
            // Inicia el efecto de máquina de escribir en el texto del diálogo.
            _typewriterEffect.Run(dialogue, textLabel, fontAsset, textColor, audioClip);

            // Espera hasta que el efecto de máquina de escribir haya terminado.
            while (_typewriterEffect.IsRunning)
            {
                yield return null;

                // Si se solicita saltar el diálogo, detén el efecto de máquina de escribir.
                if (playerInput.SkipDialogue)
                {
                    _typewriterEffect.Stop();
                }
            }
        }


        /// <summary>
        /// Cierra el cuadro de diálogo y restablece la interfaz de usuario.
        /// </summary>
        public void CloseDialogBox()
        {
            IsOpen = false;
            dialogueBox.SetActive(false);
            textLabel.text = string.Empty;
            imageHolder.gameObject.SetActive(false);
            imageHolder.sprite = null;
        }
    }
}
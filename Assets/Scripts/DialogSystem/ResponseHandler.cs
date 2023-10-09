using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogSystem
{
    /// <summary>
    /// Clase encargada de gestionar las respuestas en un cuadro de diálogo.
    /// </summary>
    public class ResponseHandler : MonoBehaviour
    {
        [SerializeField] private RectTransform responseBox;
        [SerializeField] private RectTransform responseButtonTemplate;
        [SerializeField] private RectTransform responseContainer;
        private DialogueUI _dialogueUi; // La interfaz de usuario de diálogo.
        private ResponseEvent[] _responseEvents; // Los eventos de respuesta.
        
        // Lista temporal de botones de respuesta.
        private List<GameObject> _tempResponseButtons = new();

        private void Start()
        {
            _dialogueUi = GetComponent<DialogueUI>();
        }

        /// <summary>
        /// Agrega eventos de respuesta al sistema de manejo de respuestas.
        /// </summary>
        /// <param name="responseEvents">Los eventos de respuesta a agregar.</param>
        public void AddResponseEvents(ResponseEvent[] responseEvents)
        {
            _responseEvents = responseEvents;
        }

        /// <summary>
        /// Muestra las respuestas en el cuadro de diálogo.
        /// </summary>
        /// <param name="responses">Las respuestas a mostrar.</param>
        public void ShowResponses(Response[] responses)
        {
            
            var responseBoxHeight = 0f;
            for (var i = 0; i < responses.Length; i++)
            {
                var response = responses[i];
                response.DialogueObject.Sprite = response.CharacterSprite;
                response.DialogueObject.AudioClip = response.AudioClip;
                var responseIndex = i;
                var responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
                responseButton.gameObject.SetActive(true);
                responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
                responseButton.GetComponent<Button>().onClick
                    .AddListener(() => OnPickedResponse(response, responseIndex));

                _tempResponseButtons.Add(responseButton);

                responseBoxHeight += responseButtonTemplate.sizeDelta.y;
            }

            responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
            responseBox.gameObject.SetActive(true);
        }

        /// <summary>
        /// Maneja la selección de una respuesta.
        /// </summary>
        /// <param name="response">La respuesta seleccionada.</param>
        /// <param name="responseIndex">El índice de la respuesta seleccionada.</param>
        private void OnPickedResponse(Response response, int responseIndex)
        {
            responseBox.gameObject.SetActive(false);
            foreach (var button in _tempResponseButtons)
            {
                Destroy(button);
            }

            _tempResponseButtons.Clear();

            if (_responseEvents != null && responseIndex <= _responseEvents.Length)
            {
                _responseEvents[responseIndex].OnPickedResponse?.Invoke();
            }

            _responseEvents = null;
            if (response.DialogueObject)
            {
                _dialogueUi.ShowDialogue(response.DialogueObject);
                
            }
            else
            {
                _dialogueUi.CloseDialogBox();
            }
        }
    }
}
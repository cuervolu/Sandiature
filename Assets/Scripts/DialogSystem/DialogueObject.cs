using TMPro;
using UnityEngine;

namespace DialogSystem
{
    /// <summary>
    /// Clase que define un objeto de diálogo en el juego.
    /// </summary>
    [CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue/DialogueObject")]
    public class DialogueObject : ScriptableObject
    {
        [Header("Dialogue Settings")] [SerializeField] [TextArea]
        private string[] dialogue;

        [SerializeField] private Response[] responses;
        [SerializeField] private Sprite sprite;

        [Header("Dialogue Properties")] [SerializeField]
        private Color textColor = Color.white;

        [SerializeField] private TMP_FontAsset fontAsset;
        [SerializeField] private AudioClip audioClip;


        /// <summary>
        /// Determina si el objeto de diálogo tiene respuestas.
        /// </summary>
        public bool HasResponses => Responses is { Length: > 0 };

        /// <summary>
        /// Obtiene las líneas de diálogo como un arreglo de cadenas.
        /// </summary>
        public string[] Dialogue => dialogue;

        /// <summary>
        /// Obtiene las respuestas de diálogo como un arreglo de respuestas.
        /// </summary>
        public Response[] Responses => responses;

        /// <summary>
        /// Obtiene o establece la imagen asociada al diálogo.
        /// </summary>
        public Sprite Sprite
        {
            get => sprite;
            set => sprite = value;
        }

        /// <summary>
        /// Obtiene el color del texto del diálogo.
        /// </summary>
        public Color TextColor => textColor;

        /// <summary>
        /// Obtiene la fuente de texto del diálogo.
        /// </summary>
        public TMP_FontAsset FontAsset => fontAsset;

        /// <summary>
        /// Obtiene o establece el clip de audio asociado al diálogo.
        /// </summary>
        public AudioClip AudioClip
        {
            get => audioClip;
            set => audioClip = value;
        }
    }
}
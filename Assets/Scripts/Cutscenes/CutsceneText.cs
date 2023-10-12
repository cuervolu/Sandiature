using System.Collections;
using DialogSystem;
using TMPro;
using UnityEngine;

namespace Cutscenes
{
    public class CutsceneText : MonoBehaviour
    {
        [SerializeField] private TypewriterEffect typewriterEffect;
        [SerializeField] private DialogueObject dialogueObject;
        [SerializeField] private TMP_Text textLabel;
        [SerializeField] private TMP_FontAsset fontAsset;
        [SerializeField] private Color textColor = Color.white;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private float delayBetweenLines = 2.0f;
        
        private Coroutine _typingCoroutine;
        
        private void Start()
        {
            Run();
        }

        private IEnumerator StepThroughCutsceneDialogue()
        {
            for (var i = 0; i < dialogueObject.Dialogue.Length; i++)
            {
                var dialogue = dialogueObject.Dialogue[i];

                // Ejecuta el efecto de máquina de escribir en el texto del diálogo.
                yield return RunTypewriterEffect(dialogue);

                // Actualiza el texto del diálogo.
                textLabel.text = dialogue;
                
                // Si es la última línea de diálogo, detén el ciclo.
                if (i == dialogueObject.Dialogue.Length - 1) break;

                yield return null;

                // Espera hasta que se detecte la señal de avanzar al siguiente diálogo.
                yield return new WaitForSeconds(delayBetweenLines);
            }
        }

        private IEnumerator RunTypewriterEffect(string dialogue)
        {
            // Inicia el efecto de máquina de escribir en el texto del diálogo.
            typewriterEffect.Run(dialogue, textLabel, fontAsset, textColor, audioClip);

            // Espera hasta que el efecto de máquina de escribir haya terminado.
            while (typewriterEffect.IsRunning)
            {
                yield return null;
            }
        }

        private void Run()
        {
            StartCoroutine(StepThroughCutsceneDialogue());
        }
    }
}

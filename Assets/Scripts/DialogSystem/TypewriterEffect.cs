using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;

namespace DialogSystem
{
    /// <summary>
    /// Clase que implementa un efecto de máquina de escribir para el texto del diálogo.
    /// </summary>
    public class TypewriterEffect : MonoBehaviour
    {
        [SerializeField] private float typewriterSpeed = 50f;
        public bool IsRunning { get; private set; }

        private readonly List<Punctuation> _punctuations = new()
        {
            new Punctuation(new HashSet<char>() { '.', '!', '?' }, 0.3f),
            new Punctuation(new HashSet<char>() { ',', ';', ':' }, 0.3f)
        };

        private Coroutine _typingCoroutine;

        /// <summary>
        /// Inicia el efecto de máquina de escribir en el texto del diálogo.
        /// </summary>
        /// <param name="dialogue">El texto del diálogo que se va a mostrar.</param>
        /// <param name="textLabel">El objeto de texto donde se mostrará el diálogo.</param>
        /// <param name="fontAsset">La fuente de texto a aplicar al diálogo.</param>
        /// <param name="textColor">El color del texto del diálogo.</param>
        /// <param name="audioClip">El clip de audio asociado al diálogo.</param>
        public void Run(string dialogue, TMP_Text textLabel, TMP_FontAsset fontAsset, Color textColor,
            AudioClip audioClip)
        {
            _typingCoroutine = StartCoroutine(TypeText(dialogue, textLabel, fontAsset, textColor, audioClip));
        }

        /// <summary>
        /// Detiene el efecto de máquina de escribir en curso.
        /// </summary>
        public void Stop()
        {
            StopCoroutine(_typingCoroutine);
            IsRunning = false;
        }

        /// <summary>
        /// Ejecuta el efecto de máquina de escribir en el texto del diálogo.
        /// </summary>
        /// <param name="dialogue">El texto del diálogo que se va a mostrar.</param>
        /// <param name="textLabel">El objeto de texto donde se mostrará el diálogo.</param>
        /// <param name="fontAsset">La fuente de texto a aplicar al diálogo.</param>
        /// <param name="textColor">El color del texto del diálogo.</param>
        /// <param name="audioClip">El clip de audio asociado al diálogo.</param>
        private IEnumerator TypeText(string dialogue, TMP_Text textLabel, TMP_FontAsset fontAsset, Color textColor,
            AudioClip audioClip)
        {
            IsRunning = true;
            textLabel.text = string.Empty;
            textLabel.font = fontAsset;
            textLabel.color = textColor;
            float t = 0;
            var charIndex = 0;

            while (charIndex < dialogue.Length)
            {
                var lastCharIndex = charIndex;

                t += Time.deltaTime * typewriterSpeed;

                charIndex = Mathf.FloorToInt(t);
                charIndex = Mathf.Clamp(charIndex, 0, dialogue.Length);

                for (var i = lastCharIndex; i < charIndex; i++)
                {
                    var isLast = i >= dialogue.Length - 1;
                    
                    // Reproduce el sonido asociado al diálogo.
                    SoundManager.Instance.PlaySound(audioClip);
                    
                    // Actualiza el texto del diálogo con los caracteres escritos hasta el momento.
                    textLabel.text = dialogue[..(i + 1)];

                    // Si el carácter actual es puntuación y el siguiente también lo es,
                    // espera un tiempo determinado antes de mostrar el siguiente carácter.
                    if (IsPunctuation(dialogue[i], out var waiTime) && !isLast && IsPunctuation(dialogue[i + 1], out _))
                    {
                        yield return new WaitForSeconds(waiTime);
                    }
                }

                yield return null;
            }

            IsRunning = false;
        }

        private bool IsPunctuation(char character, out float waitTime)
        {
            foreach (var punctuationCategory in _punctuations)
            {
                if (punctuationCategory.Punctuations.Contains(character))
                {
                    waitTime = punctuationCategory.WaitTime;
                    return true;
                }
            }

            waitTime = default;
            return false;
        }


        /// <summary>
        /// Estructura que define una categoría de puntuación y su tiempo de espera.
        /// </summary>
        private readonly struct Punctuation
        {
            public readonly HashSet<char> Punctuations;
            public readonly float WaitTime;

            public Punctuation(HashSet<char> punctuations, float waitTime)
            {
                Punctuations = punctuations;
                WaitTime = waitTime;
            }
        }
    }
}
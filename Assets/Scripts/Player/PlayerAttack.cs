using UnityEditor;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Attack")] [SerializeField] private float offsetAmount = 0.5f;
        [SerializeField] private Collider2D attackCollider;
        private Vector3 _normalPosition;

        private void Start()
        {
            attackCollider = GetComponent<Collider2D>();
            _normalPosition = transform.position;
        }

        public void Attack(Vector2 direction)
        {
            attackCollider.enabled = true;

            Vector3 attackPosition = direction;

            // Establecemos la posición del objeto de ataque
            transform.localPosition = attackPosition * offsetAmount;
        }

        private void StopAttack()
        {
            // Restablecemos la posición del objeto de ataque
            transform.localPosition = _normalPosition;
            attackCollider.enabled = false;
        }

        #region Gizmos

        private void OnDrawGizmos()
        {
            // Guardamos el color actual de los Gizmos
            var originalColor = Gizmos.color;

            // Cambiamos el color de los Gizmos para hacerlos más visibles
            Gizmos.color = Color.red;

            // Calculamos el centro del Gizmo como la posición del objeto de ataque
            var center = transform.position;

            // Calculamos el tamaño del Gizmo en función del tamaño del collider
            var size = attackCollider.bounds.size;

            // Dibujamos un cubo relleno con el centro y el tamaño calculados
            Gizmos.DrawCube(center, size);

            // Calculamos la distancia entre el jugador y el objeto de ataque
            var position = transform.parent.position;
            var distance = Vector3.Distance(center, position);

            // Cambiamos el color para las líneas que representan la distancia
            Gizmos.color = Color.green;

            // Dibujamos una línea desde el jugador hasta el centro del Gizmo
            Gizmos.DrawLine(position, center);

            // Ajustamos la posición vertical del etiquetado para que esté más arriba
            var labelPosition = center + Vector3.up * 0.8f; // Puedes ajustar el valor "0.5f" según tus preferencias

            // Dibujamos la distancia en la escena con la posición ajustada
            var style = new GUIStyle
            {
                normal =
                {
                    textColor = Color.green
                }
            };
            Handles.Label(labelPosition, "Distance: " + distance.ToString("F2"), style);

            // Restauramos el color original de los Gizmos
            Gizmos.color = originalColor;
        }



        #endregion
    }
}
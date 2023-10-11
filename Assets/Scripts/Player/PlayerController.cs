using System.Collections.Generic;
using DialogSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// Controlador del jugador.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] private float speed = 8f;
        [SerializeField] private ContactFilter2D movementFilter;
        [SerializeField] private float collisionOffset = 0.05f;
        private readonly List<RaycastHit2D> _castCollision = new();
        private Vector2 _movementInput;
        private Vector2 _lastPosition = Vector2.zero;
        private bool _canMove = true;

        [Header("Attack")] [SerializeField] private PlayerAttack playerAttack;

        [Header("Dialogue")] [SerializeField] private DialogueUI dialogueUI;
        public DialogueUI DialogueUi => dialogueUI;
        public bool SkipDialogue { get; private set; }

        public IInteractable Interactable { get; set; }

        private Rigidbody2D _rb;
        private Animator _animator;
        private bool _activeDialogue;
        public bool NextDialogue { get; set; }
        private TypewriterEffect _typewriterEffect;


        #region Hashed Properties

        private static readonly int Speed = Animator.StringToHash("speed");
        private static readonly int Horizontal = Animator.StringToHash("horizontal");
        private static readonly int Vertical = Animator.StringToHash("vertical");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

        #endregion

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _typewriterEffect = dialogueUI.GetComponent<TypewriterEffect>();
        }

        private void Update()
        {
            if (SkipDialogue)
                _typewriterEffect.Stop();

            if (dialogueUI.IsOpen)
            {
                _animator.SetFloat(Speed, 0);
                LockMovement();
            }else
                UnlockMovement();
        }

        private void FixedUpdate()
        {
            if (!_canMove) return;
            if (_movementInput != Vector2.zero)
            {
                var success = TryMove(_movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(_movementInput.x, 0));

                    if (!success)
                        TryMove(new Vector2(0, _movementInput.y));
                }

                _animator.SetFloat(Speed, _movementInput.sqrMagnitude);
            }
            else
            {
                _animator.SetFloat(Horizontal, _lastPosition.x);
                _animator.SetFloat(Vertical, _lastPosition.y);
                _animator.SetFloat(Speed, 0);
            }
        }


        #region Movement

        /// <summary>
        /// Intenta mover al jugador en una dirección especificada.
        /// </summary>
        /// <param name="direction">La dirección en la que se intenta mover al jugador.</param>
        /// <returns>Devuelve verdadero si el movimiento fue exitoso, falso si hubo una colisión.</returns>
        private bool TryMove(Vector2 direction)
        {
            // Verifica si la dirección es cero (sin movimiento).
            if (direction == Vector2.zero) return false;

            // Realiza un raycast para verificar si hay colisiones en la dirección de movimiento.
            var count = _rb.Cast(
                direction,
                movementFilter,
                _castCollision,
                speed * Time.fixedDeltaTime + collisionOffset);

            // Si se encuentra una colisión, devuelve falso para indicar que el movimiento no fue exitoso.
            if (count != 0) return false;

            // Mueve al jugador en la dirección especificada.
            _rb.MovePosition(_rb.position + direction * (speed * Time.fixedDeltaTime));

            // Actualiza la animación de movimiento del jugador.
            _animator.SetFloat(Horizontal, _movementInput.x);
            _animator.SetFloat(Vertical, _movementInput.y);
            _animator.SetFloat(Speed, _movementInput.sqrMagnitude);

            // Registra la última posición válida.
            _lastPosition = direction;

            // Devuelve verdadero para indicar que el movimiento fue exitoso.
            return true;
        }


        public void OnMove(InputAction.CallbackContext movementValue)
        {
            _movementInput = movementValue.ReadValue<Vector2>();
        }

        private void LockMovement()
        {
            _canMove = false;
        }

        private void UnlockMovement()
        {
            _canMove = true;
        }

        #endregion

        #region Attack

        public void OnFire(InputAction.CallbackContext attackValue)
        {
            if (DialogueUi.IsOpen) return;
            _animator.SetTrigger(IsAttacking);
            Attack();
        }

        private void Attack()
        {
            LockMovement();
            playerAttack.Attack(_lastPosition);
            UnlockMovement();
        }

        #endregion

        #region Dialogue

        public void OnDialogue(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            Interactable?.Interact(this);
        }

        public void OnSkipDialogue(InputAction.CallbackContext context)
        {
            SkipDialogue = true;
            _typewriterEffect.Stop();
        }

        public bool OnNextDialogue()
        {
            return NextDialogue;
        }

        public void OnStepThroughDialogue(InputAction.CallbackContext context)
        {
            NextDialogue = context.performed;
        }

        #endregion
    }
}
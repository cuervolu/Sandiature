using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] private float speed = 8f;
        [SerializeField] private ContactFilter2D movementFilter;
        [SerializeField] private float collisionOffset = 0.05f;
        private readonly List<RaycastHit2D> _castCollision = new();
        private Vector2 _movementInput;
        private Vector2 _lastPosition = Vector2.zero;
        private bool _canMove = true;

        private Rigidbody2D _rb;
        private Animator _animator;


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

        private bool TryMove(Vector2 direction)
        {
            if (direction == Vector2.zero) return false;
            var count = _rb.Cast(
                direction,
                movementFilter,
                _castCollision,
                speed * Time.fixedDeltaTime + collisionOffset);
            if (count != 0) return false;
            _rb.MovePosition(_rb.position + direction * (speed * Time.fixedDeltaTime));
            _animator.SetFloat(Horizontal, _movementInput.x);
            _animator.SetFloat(Vertical, _movementInput.y);
            _animator.SetFloat(Speed, _movementInput.sqrMagnitude);
            _lastPosition = direction;
            return true;
        }

        public void OnMove(InputValue movementValue)
        {
            _movementInput = movementValue.Get<Vector2>();
        }

        public void LockMovement()
        {
            _canMove = false;
        }

        public void UnlockMovement()
        {
            _canMove = true;
        }

        #endregion

        #region Attack

        public void OnFire(InputValue attackValue)
        {
            _animator.SetTrigger(IsAttacking);
        }

        #endregion
    }
}
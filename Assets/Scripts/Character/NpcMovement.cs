using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class NpcMovement : CharacterBehaviour
    {
        [SerializeField] private Transform _pointAPosition;
        [SerializeField] private Transform _pointBPosition;
        [SerializeField] private Transform _destination;
        [SerializeField] private float _speed;
        [SerializeField] private float _waitBeforeMovingAgainTime;
        [SerializeField] private float _minDistanceToChangeDestination;
        private IFacingStrategy facingStrategy = new NpcFacingStrategy();
        private float _currentSpeed;
        private float _timeLeftUntilWaitingFinished;

        void Awake()
        {
            if (_destination == null) _destination = _pointAPosition;
            IsWalking = true;
            transform.position = _destination.position;
            _currentSpeed = _speed;
            _timeLeftUntilWaitingFinished = _waitBeforeMovingAgainTime;
            SetFacingDirection(_destination.localPosition - gameObject.transform.localPosition);
        }

        void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, _destination.localPosition, _currentSpeed * Time.deltaTime);
            if ((_destination.localPosition - transform.localPosition).magnitude < _minDistanceToChangeDestination)
            {
                IsWalking = false;
                _currentSpeed = 0f;
                _timeLeftUntilWaitingFinished -= Time.deltaTime;
                if (_timeLeftUntilWaitingFinished <= 0)
                {
                    IsWalking = true;
                    _timeLeftUntilWaitingFinished = _waitBeforeMovingAgainTime;
                    SwapDestination();
                }
            }
        }

        private void SwapDestination()
        {
            if (_destination == _pointAPosition)
            {
                _destination = _pointBPosition;
            }
            else
            {
                _destination = _pointAPosition;
            }
            _currentSpeed = _speed;
            SetFacingDirection(_destination.localPosition - gameObject.transform.localPosition);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                IsWalking = false;
                _currentSpeed = 0f;
                SetFacingDirection(facingStrategy.Execute(transform.position, other.transform.position));
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            IsWalking = true;
            _currentSpeed = _speed;
            SetFacingDirection(_destination.localPosition - gameObject.transform.localPosition);
        }
    }
}

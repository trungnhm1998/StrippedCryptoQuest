using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class NpcMovement : CharacterBehaviour
    {
        [SerializeField] private GameObject _characterObject;
        [SerializeField] private Transform _pointAPosition;
        [SerializeField] private Transform _pointBPosition;
        [SerializeField] private Transform _destination;
        [SerializeField] private float _speed;
        [SerializeField] private float _waitBeforeMovingAgainTime;
        [SerializeField] private float _minDistanceToChangeDestination;
        private IFacingStrategy facingStrategy = new NpcFacingStrategy();
        private float _currentSpeed;
        private float _timeLeftUntilWaitingFinished;
        private Vector3 _characterLocalPosition;

        void Awake()
        {
            if (_destination == null) _destination = _pointAPosition;
            IsWalking = true;
            _characterObject.transform.position = _destination.position;
            _currentSpeed = _speed;
            _timeLeftUntilWaitingFinished = _waitBeforeMovingAgainTime;
        }

        void Update()
        {
            Move();
        }

        private void Move()
        {
            _characterLocalPosition = _characterObject.transform.localPosition;
            _characterObject.transform.localPosition = Vector2.MoveTowards(_characterLocalPosition, _destination.localPosition, _currentSpeed * Time.deltaTime);
            if ((_destination.localPosition - _characterLocalPosition).magnitude < _minDistanceToChangeDestination)
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
            SetFacingDirection(_destination.localPosition - _characterLocalPosition);
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                IsWalking = false;
                _currentSpeed = 0f;
                SetFacingDirection(facingStrategy.Execute(_characterObject.transform.position, other.transform.position));
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            IsWalking = true;
            _currentSpeed = _speed;
            SetFacingDirection(_destination.localPosition - _characterLocalPosition);
        }
    }
}

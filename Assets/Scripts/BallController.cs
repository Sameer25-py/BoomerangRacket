using UnityEngine;
using UnityEngine.Events;

public class BallController : MonoBehaviour
{
    public  float            MoveSpeed = 1f;
    private Vector2          _direction;
    private CircleCollider2D _collider;
    public  bool             IsGamePaused, IsGameStarted;

    public static readonly UnityEvent EndGame     = new();
    public static readonly UnityEvent BallTouched = new();

    private void OnEnable()
    {
        GameManager.StartGame.AddListener(OnStartGameCalled);
        GameManager.PauseGame.AddListener(OnPauseGameCalled);
        GameManager.UnPauseGame.AddListener(OnUnpauseGameCalled);
    }

    private void OnDisable()
    {
        GameManager.StartGame.RemoveListener(OnStartGameCalled);
        GameManager.PauseGame.RemoveListener(OnPauseGameCalled);
        GameManager.UnPauseGame.RemoveListener(OnUnpauseGameCalled);
    }

    private void OnUnpauseGameCalled()
    {
        _collider.enabled = true;
        IsGamePaused      = false;
    }

    private void OnPauseGameCalled()
    {
        _collider.enabled = false;
        IsGamePaused      = true;
    }

    private void OnStartGameCalled()
    {
        _direction         = Random.insideUnitCircle.normalized;
        _collider          = GetComponent<CircleCollider2D>();
        _collider.enabled  = true;
        transform.position = Vector2.zero;
        IsGameStarted      = true;
        IsGamePaused       = false;
    }

    private void TriggerEndGame()
    {
        IsGameStarted     = false;
        _collider.enabled = false;
        EndGame?.Invoke();
    }

    private void Update()
    {
        if (IsGamePaused || !IsGameStarted) return;
        transform.position += (Vector3)_direction * (MoveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, Vector2.zero) >= 3f)
        {
            TriggerEndGame();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collider.isTrigger = true;
        Invoke(nameof(EnableCollider), 0.5f);
        _direction = Vector2.Reflect(_direction, collision.contacts[0]
                .normal * 2f)
            .normalized;
        BallTouched?.Invoke();
    }

    private void EnableCollider()
    {
        if (_collider)
        {
            _collider.isTrigger = false;
        }
    }
}
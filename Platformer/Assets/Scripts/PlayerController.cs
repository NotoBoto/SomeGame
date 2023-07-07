using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel _playerModel;
    private PlayerView _playerView;
    private Rigidbody2D _playerRigidbody;
    private SpriteRenderer _playerSpriteRenderer;
    private Animator _playerAnimator;
    private Camera _mainCamera;
    private LvlController _lvlController;

    public float PlayerSpeed;
    public float PlayerJumpForce;

    private void Awake()
    {
        _playerModel = new PlayerModel();
        _playerView = GetComponent<PlayerView>();
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<Animator>();
        _mainCamera = Camera.main;
        _lvlController = FindAnyObjectByType<LvlController>();

        _playerModel.JumpForce = PlayerJumpForce;
        _playerModel.Speed = PlayerSpeed;
        _playerModel.IsGameOn = true;
    }

    private void Update()
    {
        if (_playerModel.IsGameOn)
        {
            Movement(_playerModel.Speed, _playerModel.JumpForce);
            CameraFollow(_mainCamera);
        }

    }

    private void Movement(float speed, float jumpForce)
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0f, 0f);
            if (Input.GetKeyDown(KeyCode.A) && Mathf.Abs(_playerRigidbody.velocity.y) < 0.001f)
                _playerRigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0f, 0f);
            if (Input.GetKeyDown(KeyCode.D) && Mathf.Abs(_playerRigidbody.velocity.y) < 0.001f)
                _playerRigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void CameraFollow(Camera camera)
    {
        Vector3 target = new Vector3(transform.position.x + 2f, transform.position.y * 0.25f, camera.transform.position.z);
        Vector3 currentPosition = Vector3.Lerp(camera.transform.position, target, 8f * Time.deltaTime);
        camera.transform.position = currentPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _playerView.Die();
            Debug.Log("asd");
            _playerModel.IsGameOn = false;
        }
        else if (collision.gameObject.CompareTag("Spawner"))
        {
            _lvlController.NewLvl();
        }
    }
}

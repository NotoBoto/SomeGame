using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerModel _playerModel;
    private Rigidbody2D _playerRigidbody;
    private Animator _playerAnimator;
    private AudioSource _playerAudioSource;
    private Camera _mainCamera;
    private LvlController _lvlController;
    private Canvas _canvas;

    private AudioClip[] _audioClips;

    private void Awake()
    {
        Cursor.visible = false;

        _playerModel = new PlayerModel();
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerAudioSource = GetComponent<AudioSource>();
        _mainCamera = Camera.main;
        _lvlController = FindAnyObjectByType<LvlController>();
        _canvas = FindObjectOfType<Canvas>();

        _audioClips = Resources.LoadAll<AudioClip>("Sound/FX");

        _playerModel.JumpForce = 6f;
        _playerModel.Speed = 6f;
        _playerModel.IsGameOn = false;
        _playerModel.Score = 0;
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
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            if (Mathf.Abs(_playerRigidbody.velocity.y) < 0.001f) 
            {
                _playerRigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                _playerAnimator.SetBool("IsJumping", true);
                PlayJumpSound();    
            }

        }
        else if (Mathf.Abs(_playerRigidbody.velocity.y) == 0f && _playerAnimator.GetBool("IsJumping"))
        {
            PlayLandSound();
            _playerAnimator.SetBool("IsJumping", false);
        }
        _playerAnimator.SetFloat("Movement", Input.GetAxis("Horizontal"));
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
            StartCoroutine(Die());
        }
        else if (collision.gameObject.CompareTag("Spawner") && _playerModel.IsGameOn)
        {
            _lvlController.NewLvl();
            _playerModel.Score += 10;
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator Die()
    {
        _playerModel.IsGameOn = false;
        _playerAnimator.SetFloat("Movement", 0);
        _playerAnimator.SetBool("IsJumping", false);
        _playerAnimator.SetBool("IsDead", true);
        _canvas.gameObject.SetActive(true);
        _canvas.transform.Find("DieText").gameObject.SetActive(true);
        _canvas.transform.Find("Score").gameObject.SetActive(true);
        _canvas.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = _playerModel.Score.ToString();
        yield return new WaitForSeconds(0.5f);
        _lvlController.IsWaitingForInputToRestart = true;
    }
    private void PlayStepSound()
    {
        _playerAudioSource.PlayOneShot(_audioClips[Random.Range(6, 16)]);
    }

    private void PlayJumpSound()
    {
        _playerAudioSource.PlayOneShot(_audioClips[Random.Range(3, 5)]);
    }

    private void PlayLandSound()
    {
        _playerAudioSource.PlayOneShot(_audioClips[Random.Range(0, 2)]);
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlController : MonoBehaviour
{
    private AudioSource _lvlAudioSource;

    private AudioClip[] _lvlMusicClips;
    private GameObject[] _lvlPrefabs;
    private List<GameObject> _inGameLvl;
    private Canvas _canvas;

    private PlayerController _playerController;

    private int _currentOffsetX;
    private bool _isWaitingForInputToStart = true;
    [HideInInspector]
    public bool IsWaitingForInputToRestart = false;

    private void Awake()
    {
        _lvlAudioSource = GetComponent<AudioSource>();

        _currentOffsetX = 1;

        _playerController = FindAnyObjectByType<PlayerController>();
        _canvas = FindObjectOfType<Canvas>();

        _lvlMusicClips = Resources.LoadAll<AudioClip>("Sound/Music");
        _lvlPrefabs = Resources.LoadAll<GameObject>("LvlPrefabs");
        _inGameLvl = new List<GameObject>();

        GameObject LvlStart1 = Instantiate(_lvlPrefabs[Random.Range(0, 3)]);
        LvlStart1.transform.SetParent(this.transform);
        LvlStart1.name = "LvlStart1";
        LvlStart1.transform.localPosition = new Vector3(53, 8f, 0f);

        GameObject LvlStart2 = Instantiate(_lvlPrefabs[Random.Range(0, 3)]);
        LvlStart2.transform.SetParent(this.transform);
        LvlStart2.name = "LvlStart2";
        LvlStart2.transform.localPosition = new Vector3(73, 8f, 0f);
    }

    private void Start()
    {
        PlayMusicClip();
    }

    private void Update()
    {
        if (_isWaitingForInputToStart)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.anyKeyDown)
            {
                _isWaitingForInputToStart = false;
                _playerController._playerModel.IsGameOn = true;
                _canvas.transform.Find("PressKey").gameObject.SetActive(false);
                _canvas.gameObject.SetActive(false);
            }
        }


        if(!_isWaitingForInputToStart && IsWaitingForInputToRestart && Input.anyKey)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void NewLvl()
    {
        GameObject NewLvl = Instantiate(_lvlPrefabs[Random.Range(0, _lvlPrefabs.Length)]);
        NewLvl.transform.SetParent(this.transform);
        NewLvl.name = "NewLvl" + _currentOffsetX;
        NewLvl.transform.localPosition = new Vector3(73 + _currentOffsetX * 20, 8f, 0f);
        _currentOffsetX++;
        _inGameLvl.Add(NewLvl);
        if (_inGameLvl.Count >= 5)
        {
            GameObject oldLvl = _inGameLvl[0];
            _inGameLvl.RemoveAt(0);
            Destroy(oldLvl);
        }
    }

    private void PlayMusicClip()
    {
        if(_lvlMusicClips.Length > 0)
        {
            int r = Random.Range(0, _lvlMusicClips.Length);
            _lvlAudioSource.clip = _lvlMusicClips[r];
            _lvlAudioSource.Play();

            Invoke("PlayMusicClip", _lvlMusicClips[r].length);
        }
    }
}

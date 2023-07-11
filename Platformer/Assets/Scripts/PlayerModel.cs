using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    private float _speed;
    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    private float _jumpForce;
    public float JumpForce
    {
        get { return _jumpForce; }
        set { _jumpForce = value; }
    }

    private bool _isGameOn;
    public bool IsGameOn
    {
        get { return _isGameOn; }
        set { _isGameOn = value; }
    }

    private int _score;
    public int Score 
    { 
        get { return _score; } 
        set { _score = value; }
    }
}

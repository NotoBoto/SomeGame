using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerView : MonoBehaviour
{
    public void Die()
    {
        SceneManager.LoadScene(0);
    }
}

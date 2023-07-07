using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LvlController : MonoBehaviour
{
    private GameObject[] _lvlPrefabs;

    private int _currentOffsetX;

    private void Awake()
    {
        _currentOffsetX = 1;

        _lvlPrefabs = Resources.LoadAll<GameObject>("LvlPrefabs");

        GameObject LvlStart1 = Instantiate(_lvlPrefabs[Random.Range(0, 3)]);
        LvlStart1.transform.SetParent(this.transform);
        LvlStart1.name = "LvlStart1";
        LvlStart1.transform.localPosition = new Vector3(53, 8f, 0f);

        GameObject LvlStart2 = Instantiate(_lvlPrefabs[Random.Range(0, 3)]);
        LvlStart2.transform.SetParent(this.transform);
        LvlStart2.name = "LvlStart2";
        LvlStart2.transform.localPosition = new Vector3(73, 8f, 0f);
    }

    public void NewLvl()
    {
        GameObject NewLvl = Instantiate(_lvlPrefabs[Random.Range(0, _lvlPrefabs.Length)]);
        NewLvl.transform.SetParent(this.transform);
        NewLvl.name = "NewLvl" + _currentOffsetX;
        NewLvl.transform.localPosition = new Vector3(73 + _currentOffsetX * 20, 8f, 0f);
        _currentOffsetX++;
    }
}

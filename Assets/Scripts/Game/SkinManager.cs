using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public Material[] material;
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[PlayerPrefs.GetInt("playerskin")];

    }
    private void Update()
    {
        rend.sharedMaterial = material[PlayerPrefs.GetInt("playerskin")];

    }
}

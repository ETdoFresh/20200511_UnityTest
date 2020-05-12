using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ConsoleUI : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    private void OnValidate()
    {
        if (!textMesh) textMesh = textMesh.GetComponent<TextMeshProUGUI>();
    }

    public void WriteLine(string format, params object[] args)
    {
        textMesh.text = string.Format(format + "\n", args) + textMesh.text;
    }

    public void Clear()
    {
        textMesh.text = "";
    }
}

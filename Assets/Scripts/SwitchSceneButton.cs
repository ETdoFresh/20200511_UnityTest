using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SwitchSceneButton : MonoBehaviour
{
    public Button button;
    public string sceneName;

    private void OnValidate()
    {
        if (!button) button = GetComponent<Button>();
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, LoadScene);
#else
        button.onClick.AddListener(LoadScene);
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        UnityEditor.Events.UnityEventTools.RemovePersistentListener(button.onClick, LoadScene);
#else
        button.onClick.RemoveListener(LoadScene);
#endif
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
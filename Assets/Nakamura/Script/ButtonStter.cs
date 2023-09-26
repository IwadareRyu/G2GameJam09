using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStter : MonoBehaviour
{
    [Tooltip("�J�ڂ������V�[����")]
    [SerializeField] private string _sceneName = null;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.SceneChange(_sceneName));
    }
}

using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;

    public static LoadingScreen instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadSceneAsync(1));
    }


    
    

    private IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            loadingText.text = (progress * 100).ToString("F0") + "%";
            yield return null;
        }
    }
}

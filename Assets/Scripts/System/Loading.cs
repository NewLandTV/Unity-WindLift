using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Enums
public enum Scenes
{
    Title,
    Room,
    Main
}

public class Loading : Singleton<Loading>
{
    // UI
    [Header("UI"), SerializeField]
    private GameObject group;
    [SerializeField]
    private Image progressBarImage;

    public float ProgressBarFillAmount
    {
        get => progressBarImage.fillAmount;
        set => progressBarImage.fillAmount = value;
    }

    // Flags
    private bool isLoading;
    public bool IsLoading
    {
        get => isLoading;
        set
        {
            isLoading = value;

            group.SetActive(value);
        }
    }

    private void Awake()
    {
        InitializeSingleton(this);
    }

    public void LoadScene(Scenes nextScene)
    {
        SceneManager.LoadScene((int)nextScene);
    }
}
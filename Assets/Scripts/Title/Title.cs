using UnityEngine;

public class Title : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Button click events
    public void OnStartButtonClick()
    {
        Loading.Instance.LoadScene(Scenes.Main);
    }
}

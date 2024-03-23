using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;

    protected void InitializeSingleton(T instance)
    {
        if (Singleton<T>.instance == null)
        {
            Singleton<T>.instance = instance;

            DontDestroyOnLoad(gameObject);

            return;
        }

        Destroy(gameObject);
    }
}

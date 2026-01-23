using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorStartSceneLoader : MonoBehaviour
{
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadStartScene()
    {
        SceneManager.LoadScene("Level_02");
    }
#endif
}

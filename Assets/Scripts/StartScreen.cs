using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;  // Needed to access SceneAsset and AssetDatabase in the Editor
#endif

public class StartGameButton : MonoBehaviour
{
    // This is only available in the Editor; you can drag the scene here in the Inspector
    #if UNITY_EDITOR
    [Header("Drag a scene into this field in the Editor")]
    [SerializeField] private SceneAsset sceneAsset;
    #endif

    
    private string sceneName = "";
    // Whenever something changes in the Editor, if we have a SceneAsset assigned,
    // we update the sceneName (so it works in a standalone build as well).
    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            // Gets the path of the scene asset
            string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
            // Extracts just the scene name (no extension, no directories)
            sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
        }
    }
    #endif

    // This function is called by the button's OnClick event
    public void starting()
    {
        Invoke("StartGame", 0.5f);
    }
    public void StartGame()
    {

        if (!string.IsNullOrEmpty(sceneName))
        {
            
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("No scene is assigned or the scene name is empty. Make sure you dragged a Scene Asset and added it to Build Settings!");
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        // �ڱ༭��ģʽ��ֹͣ����
        EditorApplication.isPlaying = false;
#else
            // �ڱ�����Ӧ�����˳�����
            Application.Quit();
#endif
    }
}

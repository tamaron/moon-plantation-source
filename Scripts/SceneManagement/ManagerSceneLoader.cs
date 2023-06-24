using UnityEngine;
using UnityEngine.SceneManagement;
 
namespace U1W.MoonPlant
{
    public class ManagerSceneLoader : MonoBehaviour
    {
        private static bool Loaded { get; set; } = false;
        
        void Awake()
        {
            if(Loaded) return;
            Loaded = true;
            SceneManager.LoadScene(SceneName.Manager.ToString(), LoadSceneMode.Additive);
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VitsehLand.Scripts.Manager
{
    public class MySceneManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SceneManager.LoadScene("Assault_Rifle_01_Demo");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
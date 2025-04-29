using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class main_menu : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene("level");
    }
    
    public void quit()
    {
        Application.Quit();
    }
}

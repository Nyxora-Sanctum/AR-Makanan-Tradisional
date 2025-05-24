using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class main_menu : MonoBehaviour
{
    public GameObject mainmenu, info;
    public void play()
    {
        mainmenu.SetActive(false);
        info.SetActive(true);
    }
    
    public void quit()
    {
        Application.Quit();
    }

    public void play_asli()
    {
        SceneManager.LoadScene("level");
    }
}

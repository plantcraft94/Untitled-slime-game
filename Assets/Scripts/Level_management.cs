using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_management : MonoBehaviour
{
    [SerializeField] GameObject Spawnpoint;
    Transform player;
    private void Start()
    {
        

        if (GetCurrectLevel() == 0||GetCurrectLevel() == 6)
        {
            return;
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.position = Spawnpoint.transform.position;
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public int GetCurrectLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Continue()
    {
        SceneManager.LoadScene(Save_load_system.Load());
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

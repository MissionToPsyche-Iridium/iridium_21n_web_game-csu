using UnityEngine;

using UnityEngine;

public class GameObjectActive : MonoBehaviour
{
    public GameObject startMenu;
    public  GameObject creditMenu;
    public static GameObjectActive instance;

    void Start()
    {
        instance=this;
    }

}

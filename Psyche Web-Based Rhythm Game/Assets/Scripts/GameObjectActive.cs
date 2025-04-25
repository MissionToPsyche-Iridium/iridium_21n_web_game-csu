using TMPro;
using UnityEngine;

public class GameObjectActive : MonoBehaviour
{
    public GameObject startMenu;
    public  GameObject creditMenu;
    public static GameObjectActive instance;
    public  TextMeshProUGUI buttonLabelText;

    void Start()
    {
        instance=this;
    }

}

using TMPro;
using UnityEngine;

public class GameObjectActive : MonoBehaviour
{
    public static GameObjectActive instance;
    public TextMeshProUGUI buttonLabelText;

    void Start()
    {
        instance=this;  
    }

}

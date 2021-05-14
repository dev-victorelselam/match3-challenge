using UnityEngine;

public class AppStart : MonoBehaviour
{
    private void Start()
    {
        var context = new Context.Context();
        context.GameController.StartGame();
    }
}
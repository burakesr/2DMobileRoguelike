using UnityEngine;

public class ScreenCursor : MonoBehaviour
{
    private void Update()
    {
        transform.position = GameManager.Instance.GetPlayer().playerInput.look;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private bool _isRight, _isLeft;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider is null) return;
            if (hit.collider.CompareTag("Card"))
            {
                Debug.Log("asdasd");
                float screenMiddle = Screen.width / 2f;
                if (mousePos.x > screenMiddle)
                {
                    transform.eulerAngles = new Vector3(0, 0, -30);
                }
                else if (mousePos.x < screenMiddle)
                {
                    transform.eulerAngles = new Vector3(0, 0, 30);
                }
            }
        
            
            
        }
            
            
        
    }
       
}

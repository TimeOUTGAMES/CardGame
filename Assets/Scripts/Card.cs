using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    private bool isDragging = false;

    private void Start()
    {

    }

    void Update()
    {
        CardMovement();
    }

    void CardMovement()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Card"))
                isDragging = true;

            
        }


        if (isDragging && Input.GetMouseButton(0))
        {
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            transform.position = mousePos;
            RotateCard();

        }


        if (Input.GetMouseButtonUp(0))
            isDragging = false;
        
    }


    void RotateCard()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        float screenWidth = Screen.width;
        if (screenPosition.x > screenWidth / 2)
            transform.eulerAngles = new Vector3(0, 0, 30);
        else
            transform.eulerAngles = new Vector3(0, 0, -30);
    }

}

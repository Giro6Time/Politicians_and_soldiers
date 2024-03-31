using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndButton : MonoBehaviour
{
    BattleEndPanel battleEndPanel;

    void Update()
    {
        //���¹ر�
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                gameObject.SetActive(false);
                battleEndPanel.battleResultPanel.SetActive(false);
                //�˻ص����˵�
                //ReturnToMainMenu();
            }
        }
    }
}

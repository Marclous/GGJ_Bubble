using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponentInParent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.DisableMovement();
                playerMovement.enabled = false;
            }

            PlayerAnimation playerAnim = collision.GetComponentInParent<PlayerAnimation>();
            if (playerAnim != null)
            {
                playerAnim.SetDead(true);
            }

            GameManager.Instance.ShowGameOverUI();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;

    public event EventHandler OnSpacePressed;
    public event EventHandler OnKeyCodeEPressed;
    public class MouseSelectedEventHandlerArgs : EventArgs
    {
        public CardBase selectedCard;
    }
    public event EventHandler<MouseSelectedEventHandlerArgs> OnMouseLeftClickedOnCard;
    public event EventHandler<MouseSelectedEventHandlerArgs> OnKeyCodeFPressedOnCard;

    private CardBase selectedCard;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed?.Invoke(this, EventArgs.Empty);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnKeyCodeEPressed?.Invoke(this, EventArgs.Empty);
        }

        UpdateMouse();
    }

    private void UpdateMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics2D.Raycast(ray.origin, ray.direction))
        {
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider.TryGetComponent(out CardBase card))
            {
                selectedCard = card;
                if (Input.GetMouseButtonDown(0))
                {
                    OnMouseLeftClickedOnCard?.Invoke(this, new MouseSelectedEventHandlerArgs
                    {
                        selectedCard = selectedCard
                    }) ;
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    OnKeyCodeFPressedOnCard?.Invoke(this, new MouseSelectedEventHandlerArgs
                    {
                        selectedCard = selectedCard
                    });
                }
            }
        }
    }
}

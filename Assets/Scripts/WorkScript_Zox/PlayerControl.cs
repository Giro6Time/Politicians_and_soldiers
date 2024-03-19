using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;

    public event EventHandler OnSpacePressed;
    public event EventHandler OnKeyCodeEPressed;
    public class MouseSelectedEventArgs : EventArgs
    {
        public CardBase selectedCard;
    }
    public event EventHandler<MouseSelectedEventArgs> OnMouseLeftClickedOnCard;
    public event EventHandler<MouseSelectedEventArgs> OnKeyCodeFPressedOnCard;

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

    //2D Logic
    /*private void UpdateMouse()
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
                    OnMouseLeftClickedOnCard?.Invoke(this, new MouseSelectedEventArgs
                    {
                        selectedCard = selectedCard
                    }) ;
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    OnKeyCodeFPressedOnCard?.Invoke(this, new MouseSelectedEventArgs
                    {
                        selectedCard = selectedCard
                    });
                }
            }
        }
    }*/
    private void UpdateMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out CardBase card))
            {
                selectedCard = card;
                if (Input.GetMouseButtonDown(0))
                {
                    OnMouseLeftClickedOnCard?.Invoke(this, new MouseSelectedEventArgs
                    {
                        selectedCard = selectedCard
                    });
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    OnKeyCodeFPressedOnCard?.Invoke(this, new MouseSelectedEventArgs
                    {
                        selectedCard = selectedCard
                    });
                }
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    public virtual void Show()
    {
        if (!gameObject.activeSelf) 
        {
            gameObject.SetActive(true);
            OpenAnimation();
        }
    }

    public virtual void Close()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            CloseAnimation();
        }
    }

    protected virtual void OpenAnimation() { }

    protected virtual void CloseAnimation() { }
}

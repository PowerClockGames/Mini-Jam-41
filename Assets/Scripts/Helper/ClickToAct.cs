using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToAct: MonoBehaviour
{
    private int _clickCount = 0;

    public void SetMultiClickAction(int clicks, System.Action onClicksReached, System.Action onClicksNotReached)
    {
        if (_clickCount == clicks)
        {
            _clickCount = 0;
            onClicksReached();
        }
        else
        {
            _clickCount++;
            onClicksNotReached();
        }
    }
}

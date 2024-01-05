using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandRecipient
{
    void OnSetCommand(string cmd);
}
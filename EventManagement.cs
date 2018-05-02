using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Attached to an PlayerCamera
/// Holds a delegate object which allows for events to be implemented
/// Seen delegate type events like IsVisibletoCamera allow methods to be added or removed 
/// methods added to IsVisibleCamera are constantly called, kinda like an independent list of functions
/// Draw is constantly being called in the DetermineAllSeeableInteractableObjects.cs
/// </summary>
public class EventManagement : MonoBehaviour
{

    public delegate void Seen();
    public static event Seen IsVisibleToCamera;

    /// <summary> Draw():
    /// a callable function that is constantly called to activate events
    /// IsVisibleToCamera is an event that has subscribed functions
    /// -works like a Subscribeable/Unsubscribeable Messenger pattern
    /// </summary>
    public void Draw()
    {
        if (IsVisibleToCamera!=null) // Condition: IsVisibleToCamera created?
        {
            IsVisibleToCamera();//Call all functions subscribed to event called IsVisibleCamera
        }
       
    }
}

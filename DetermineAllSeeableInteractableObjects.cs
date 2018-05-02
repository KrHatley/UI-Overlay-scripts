using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// Attached to the player gameobject
/// finds the child camera
/// used to collect all the interactable game objects into an array
/// if the object can be seen by the camera.
/// Subscribes and Unsubscribes the Interactable objects to an Event system
/// </summary>

public class DetermineAllSeeableInteractableObjects : MonoBehaviour
{
    [Tooltip("Script generated list of Camera-visible-Interactable GameObjects")]
    [SerializeField] private List<GameObject> InteractableObjects;
    [Tooltip("Designer uses this to set the distance at which players can see if an object can be interacted with")]
    [SerializeField] private double CanSeeFromHere;
    private GameObject[] SceneObjects;
    private Plane[] planes;
    private Camera Cam;
    private EventManagement Em;
    private Queue<GameObject> NeedsToUnsubscribe;
    


    private void Awake()
    {
        Em = GetComponentInChildren<Camera>().GetComponent<EventManagement>();
        Cam = GetComponentInChildren<Camera>();
        NeedsToUnsubscribe = new Queue<GameObject>();
       
    }

    // Use this for initialization
    void Start ()
    {
        getAllinteractableObjectsinScene();  
	}

    /// <summary> getAllinteractableObjectsinScene():
    /// collects all objects in the Unity scene in an array
    /// determines if the object has the IInteractable interface attached
    /// adds to a dynamic List called InteractableObjects
    /// Clears the array, so as not to waste memory.
    /// </summary>
    private void getAllinteractableObjectsinScene()
    {
        SceneObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject go in SceneObjects)
        {
            if (go.activeInHierarchy == true)
            {
                if (go.GetComponent<IInteractable>() != null)
                {
                    InteractableObjects.Add(go);
                }
            }
        }
        Array.Clear(SceneObjects, 0, SceneObjects.Length);
    }

    // Update is called once per frame
    void Update ()
    {
        DetermineViewableInteractableObjects();//Finds screen rendered objects and subscribes to event
        Em.Draw();// Eventmanager calls event objects under the Draw method 
        UnSubscribeFromEvents(); // unsubscribes all objects that subscribed.
    }

    /// <summary> DetermineViewableInteractableObjects():
    /// Determines whether an Interactable object is currently "Seen" by the Main Camera
    /// If True -> calls the object's Subscribe method(which adds it to the IsVisibleToCamera event in EventManagent.cs)
    ///     Nested condition: is Initizialed.cs attached to object?
    ///         ->yes: then subscribe
    ///         ->no: add Initialized.cs to object then call subscribe 
    /// Adds object to Queue
    /// </summary>
    private void DetermineViewableInteractableObjects()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(Cam);

        for (int i = 0; i < InteractableObjects.Count; i++)
        {
            if (IsThisCloseEnough(InteractableObjects[i]))
            {
                if (GeometryUtility.TestPlanesAABB(planes, InteractableObjects[i].GetComponent<Collider>().bounds))
                {
                    if (InteractableObjects[i].GetComponent<InitializeInteractableObject>() != null)
                    {
                        InteractableObjects[i].GetComponent<InitializeInteractableObject>().Subscribe();
                        NeedsToUnsubscribe.Enqueue(InteractableObjects[i]);
                    }
                    else
                    {
                        InteractableObjects[i].AddComponent<InitializeInteractableObject>();
                        InteractableObjects[i].GetComponent<InitializeInteractableObject>().Subscribe();
                        NeedsToUnsubscribe.Enqueue(InteractableObjects[i]);
                    }
                }
            }
        }
    }

    /// <summary> UnSubscribeFromEvents():
    /// While removing Interactable object from queue object, (that was subscribed to the IsVisibleToCamera event)
    /// the Interactable object is also unsubscribed from the Event.
    /// </summary>
    private void UnSubscribeFromEvents()
    {
        for (int i = 0; i < NeedsToUnsubscribe.Count; i++)
        {
            NeedsToUnsubscribe.Dequeue().GetComponent<InitializeInteractableObject>().UnSubscribe();
		}
	}

    /// <summary>
    /// determines whether an object is within the desired range
    /// if so, draws shader
    /// </summary>
    private bool IsThisCloseEnough(GameObject go)
    {
        bool withinRange = false;
        Vector3 Player2ObjectDistance = go.transform.position - this.transform.position;

        if (Player2ObjectDistance.magnitude <= CanSeeFromHere)
        {
            withinRange = true;
        }

        return withinRange;
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializeInteractableObject : MonoBehaviour
{
    private Canvas canvas;
    private Sprite OverlaySprite;
    private GameObject Overlay;
    private Image OverlayTexture;
	private float bringToFont;

    /// <summary>
    /// Initializes and Instantiates objects necessary for UI
    /// </summary>
    private void Awake()
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        OverlaySprite = Resources.Load("SpriteOverlay",typeof(Sprite)) as Sprite;
        // OverlayTexture.sprite = OverlaySprite;
        Overlay = (GameObject)Instantiate(Resources.Load("InteractableOverlay"));
        Overlay.GetComponent<Image>().sprite = OverlaySprite;
        Overlay.transform.parent = canvas.transform;
    }

    /// <summary> Subscribe():
    /// Adds the DrawOverlay method to the event object in EventManagement.cs
    /// </summary>
    public void Subscribe()
    {
        EventManagement.IsVisibleToCamera += DrawOverlay;
    }

    /// <summary> UnSubscribe():
    /// Removes the DrawOverlay method to the event object in EventManagement.cs
    /// </summary>
    public void UnSubscribe()
    {
        EventManagement.IsVisibleToCamera -= DrawOverlay;
    }

    /// <summary> DrawOverlay():
    /// Draws UI element
    /// </summary>
    private void DrawOverlay()
    {
        Overlay.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(this.transform.position.x+.8f, 
            this.transform.position.y, this.transform.position.z), new Quaternion(0.8f,0.0f,0.8f,0.0f));
	}
}

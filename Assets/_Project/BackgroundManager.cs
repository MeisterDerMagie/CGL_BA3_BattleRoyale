//(c) copyright by by Patrick Handwerk, CGL Th Koeln, Matrikelnummer 11135936

using UnityEngine;

// Background manager used to ensure a repeating background image by moving the not active image to the top once
// not in frame anymore. Moving two background images ontop of each other to preserve resources and ensure endless repeating
// background image for upwards moving camera.
public class BackgroundManager : MonoBehaviour
{
    // Reference to the main camera
    public GameObject gameCamera;
    
    // Reference to the background active behind the camera
    public GameObject backgroundActive;
    
    // Reference to the background previously active and waiting to be moved above the current active background once out of frame
    public GameObject backgroundWaiting;

    #region private members

    // Stores of the not active background has moved already. Used to prevent multiple execution of the same code.
    private bool waitingBackgroundMoved;
    
    // reference of the sprite renderer of the current active background to access viewport height to determine when and if the not active background has dropped out of frame
    private SpriteRenderer renderedSpriteActive;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        renderedSpriteActive = backgroundActive.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameCamera.transform.position.y >= backgroundActive.transform.position.y && !waitingBackgroundMoved)
        {
            MoveWaitingBackground();
        }
        else if (gameCamera.transform.position.y >= backgroundActive.transform.position.y && waitingBackgroundMoved)
        {
            if (gameCamera.transform.position.y >= (backgroundActive.transform.position.y + (renderedSpriteActive.bounds.size.y / 2)))
            {
                SwitchActiveBackground();
            }
        }
    }
    
    // Switches "backgroundActive" & "backgroundWaiting" values and sets the new reference to the current active backgrounds sprite renderer
    void SwitchActiveBackground()
    {
        (backgroundActive, backgroundWaiting) = (backgroundWaiting, backgroundActive);
        waitingBackgroundMoved = false;
        renderedSpriteActive = backgroundActive.GetComponent<SpriteRenderer>();
    }
    
    // Calculates new position Y for not active background below and moves it above the current active background
    void MoveWaitingBackground()
    {
        float newHeight = backgroundActive.transform.position.y + (renderedSpriteActive.bounds.size.y);
        Vector3 backgroundWaitingPosition = backgroundWaiting.transform.position;
        waitingBackgroundMoved = true;
        backgroundWaiting.transform.position = new Vector3(backgroundWaitingPosition.x, newHeight, backgroundWaitingPosition.z);
    }
}

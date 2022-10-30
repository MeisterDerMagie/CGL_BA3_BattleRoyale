using UnityEngine;
public class BackgroundManager : MonoBehaviour
{
    [SerializeField] public GameObject gameCamera;
    [SerializeField] public GameObject backgroundActive;
    [SerializeField] public GameObject backgroundWaiting;

    private bool waitingBackgroundMoved;
    private SpriteRenderer renderedSpriteActive;
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
    

    void SwitchActiveBackground()
    {
        (backgroundActive, backgroundWaiting) = (backgroundWaiting, backgroundActive);
        waitingBackgroundMoved = false;
        renderedSpriteActive = backgroundActive.GetComponent<SpriteRenderer>();
    }

    void MoveWaitingBackground()
    {
        float newHeight = backgroundActive.transform.position.y + (renderedSpriteActive.bounds.size.y);
        Vector3 backgroundWaitingPosition = backgroundWaiting.transform.position;
        waitingBackgroundMoved = true;
        backgroundWaiting.transform.position = new Vector3(backgroundWaitingPosition.x, newHeight, backgroundWaitingPosition.z);
    }
}

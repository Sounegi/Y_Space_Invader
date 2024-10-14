using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    SpriteRenderer render;

    private float screenWidth;
    private float screenHeight;
    Transform[] ghosts = new Transform[2];

    bool isWrappingX = false;
    bool isWrappingY = false;
    // Start is called before the first frame update
    void Start()
    {
        render = transform.GetComponent<SpriteRenderer>();

        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));
        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.y - screenBottomLeft.y;

        //CreateGhostShips();
    }

    // Update is called once per frame
    void Update()
    {
        ScreenWrapping();

        //SwapShips();
    }


    void ScreenWrapping()
    {
        var isVisible = render.isVisible;
        if (isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }
        if (isWrappingX && isWrappingY)
        {
            return;
        }
        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        var newPosition = transform.position;
        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;
            isWrappingX = true;
        }
        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;
            isWrappingY = true;
        }
        transform.position = newPosition;
    }

    void CreateGhostShips()
    {
        for (int i = 0; i < 2; i++)
        {
            ghosts[i] = Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;
            DestroyImmediate(ghosts[i].GetComponent<ScreenWrap>());
        }
    }

    void PositionGhostShips()
    {
        var ghostPosition = transform.position;

        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[0].position = ghostPosition;

        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[1].position = ghostPosition;

        for (int i = 0; i < 2; i++)
        {
            ghosts[i].rotation = transform.rotation;
        }

    }

    void SwapShips()
    {
        foreach (var ghost in ghosts)
        {
            if (ghost.position.x < screenWidth && ghost.position.x > -screenWidth &&
                ghost.position.y < screenHeight && ghost.position.y > -screenHeight)
            {
                transform.position = ghost.position;
                break;
            }
        }
        PositionGhostShips();
    }

    public void DestroyGhost() 
    {
        foreach(var ghost in ghosts)
        {
            //
        }
    }
}

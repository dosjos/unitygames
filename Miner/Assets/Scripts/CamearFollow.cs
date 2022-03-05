using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamearFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Vector3 offset;

    public float LeftLimit, RightLimit, BottomLimit;

    private bool MoveX;
    private bool MoveY;

    private float X, Y;

    void Start()
    {
        X = RightLimit;

        Y = player.position.y + offset.y;

        transform.position = new Vector3(X, Y, -10);
    }
    void Update()
    {

        MoveX = true;
        MoveY = true;
        if (player.position.x < LeftLimit)
        {
            MoveX = false;
        }
        if (player.position.x > RightLimit)
        {
            MoveX = false;
        }


        if (player.position.y < BottomLimit)
        {
            MoveY = false;
        }

        if (MoveX)
        {
            X = player.position.x + offset.x;
        }

        if (MoveY)
        {
            Y = player.position.y + offset.y;
        }

        transform.position = new Vector3(X, Y, -10); // Camera follows the player with specified offset position
    }
}

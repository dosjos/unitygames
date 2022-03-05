using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    public Sprite[] frameArray;
    public GameObject player;

    private PlayerMovement playerScript;

    private int currentFrame;
    private float timer;
    public float frameRate;
    private SpriteRenderer spriteRenderer;

    private bool enabled;

    private int part;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerScript = player.GetComponent<PlayerMovement>();
    }

    public void SetDynamite(Vector3 position)
    {
        part = 1;
        transform.position = position;
        enabled = true;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {

            timer += Time.deltaTime;

            if (timer >= frameRate)
            {
                timer -= frameRate;
                currentFrame = (currentFrame + 1) % frameArray.Length;
                spriteRenderer.sprite = frameArray[currentFrame];

                if (currentFrame == 8)
                {
                    gameObject.SetActive(false);
                    playerScript.Detonate(transform.position);
                }
            }

        }
    }
}

using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public PlayerController playerController;

    public Sprite[] idleSprites = new Sprite[3]; // Sprites 1-3
    public Sprite[] forwardUpSprites = new Sprite[2]; // Sprites 4,5
    public Sprite[] forwardDownSprites = new Sprite[2]; // Sprites 6,7
    public Sprite[] backwardUpSprites = new Sprite[2]; // Sprites 8,9
    public Sprite[] backwardDownSprites = new Sprite[2]; // Sprites 10,11

    public float animationSpeed = 0.1f; // Speed of Sprite swapping, seconds per frame
    private float animationTimer;
    private int currentFrameIndex = 0;
    private Sprite[] currentAnimation;

    private const float deadZone = 0.05f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError ("PlayerAnimation missing component");
        }
        if (playerController == null)
        {
            Debug.LogError("PlayerAnimation needs PlayerController assigned");
        }

        currentAnimation = idleSprites;
    }

    void Update()
    {
        Vector2 move = playerController.GetMoveInput(); // Animation state analyzer
        DetermineAnimation(move);
        Animate();
    }

     void DetermineAnimation(Vector2 move)
    {
        Sprite[] nextAnimation = idleSprites; // default sprites
        const float strictDeadZone = 0.25f;

        if (move.sqrMagnitude > strictDeadZone * strictDeadZone)
        {
            if (move.x >= strictDeadZone) // Forward
            {
                if (move.y >= strictDeadZone) // Forward & Up
                {
                    nextAnimation = forwardUpSprites;
                }
                else if (move.y <= -strictDeadZone) // Forward  & Down
                {
                    nextAnimation = forwardDownSprites;
                }
            }
            else if (move.x <= -strictDeadZone) // Backwards
            {
                if (move.y >= strictDeadZone) // Backwards & Up
                {
                    nextAnimation = backwardUpSprites;
                }
                else if (move.y <= -strictDeadZone) // Backwards & Down
                {
                    nextAnimation = backwardDownSprites;
                }
                // If moving backwards on X with Y in deadzone, it defaults to idle
            }
            else if (Mathf.Abs(move.y) >= strictDeadZone)
            {
                nextAnimation = idleSprites;
            }
        }
        if (nextAnimation != currentAnimation)
        {
            currentAnimation = nextAnimation;
            currentFrameIndex = 0;
            
            if (spriteRenderer != null && currentAnimation.Length > 0)
            {
                spriteRenderer.sprite = currentAnimation[currentFrameIndex];
            }
        }
    }

    void Animate()
    {
        if (currentAnimation ==  null || currentAnimation.Length == 0) return;
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationSpeed)
        {
            animationTimer -= animationSpeed;
            currentFrameIndex = (currentFrameIndex +1) % currentAnimation.Length;
            spriteRenderer.sprite = currentAnimation[currentFrameIndex];
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UISpriteAnimator : MonoBehaviour
{

    [System.Serializable]
    public class AnimatedImage
    {
        public Image targetUI; // Image component here
        public Sprite spriteA; // frame 1
        public Sprite spriteB; // frame 2
         }

    [Header("Settings")]
    public float changeInterval = 0.5f; // speed control
    public List<AnimatedImage> animatedImages = new List<AnimatedImage>();

    private float timer;
    private bool showingSpriteA = true;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            timer = 0;
            showingSpriteA = !showingSpriteA;
            UpdateAllSprites();
        }
    }

    void UpdateAllSprites()
    {
        foreach (var item in animatedImages)
        {
            if (item.targetUI != null)
            {
                item.targetUI.sprite = showingSpriteA ? item.spriteA : item.spriteB;
            }
        }
    }
}

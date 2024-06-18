using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public Canvas canvas; // Reference to the Canvas
    public RectTransform canvasRectTransform; // Reference to the Canvas's RectTransform
    public RectTransform imageRectTransform; // Reference to the Image's RectTransform
    public GameObject gameObjectToMove; // Reference to the GameObject to teleport
    private float animationDuration = 0.5f; // Duration of the animation
    private float distanceBetweenTargetCurrent;
    public float PlayerSpeed = 1f;
    public float flipDuration = 0.25f; 
    private Animator animator;
    private bool isMoving = false; // Flag to check if the player is currently moving
    private Coroutine moveCoroutine; // Reference to the current move coroutine
    private Vector2? nextTargetPosition = null;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void transformPlayer()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Convert the screen position to the local position of the Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            mousePosition,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        // Check if the local point is within the bounds of the image
        if (RectTransformUtility.RectangleContainsScreenPoint(imageRectTransform, mousePosition, canvas.worldCamera))
        {
            // Convert the local point to the local position within the image
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                imageRectTransform,
                mousePosition,
                canvas.worldCamera,
                out localPoint
            );

            if (isMoving)
            {
                // If already moving, stop the current coroutine
                StopCoroutine(moveCoroutine);
            }

            // Start the new movement coroutine
            moveCoroutine = StartCoroutine(SlideToPosition(localPoint));
        }
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Blocker"))
        {
            StopCoroutine(moveCoroutine);
            animator.SetBool("IsWalking", false);
        }
    }
    
    public void CollisionDetected ()
    {
         
            StopCoroutine(moveCoroutine);
            animator.SetBool("IsWalking", false);
  
    }



    IEnumerator SlideToPosition(Vector2 targetPosition)
    {
        isMoving = true; // Set moving flag to true
        animator.SetBool("IsWalking", true);

        Vector2 startPosition = gameObjectToMove.transform.localPosition;
        float elapsedTime = 0f;


        // Determine the direction and flip the character
        if (targetPosition.x > startPosition.x)
        {
            StartCoroutine(FlipCharacter(new Vector3(1, 1, 1)));
        }
        else
        {
            StartCoroutine(FlipCharacter(new Vector3(-1, 1, 1)));
        }

        distanceBetweenTargetCurrent = Vector2.Distance(startPosition, targetPosition);
        animationDuration = distanceBetweenTargetCurrent / (PlayerSpeed * 100);

        while (elapsedTime < animationDuration)
        {
            gameObjectToMove.transform.localPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObjectToMove.transform.localPosition = targetPosition;
        animator.SetBool("IsWalking", false);

        isMoving = false; // Reset moving flag

        // Check if there is a next target position
        if (nextTargetPosition.HasValue)
        {
            Vector2 nextTarget = nextTargetPosition.Value;
            nextTargetPosition = null; // Reset the next target position
            moveCoroutine = StartCoroutine(SlideToPosition(nextTarget));
        }
    }

    IEnumerator FlipCharacter(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < flipDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / flipDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}

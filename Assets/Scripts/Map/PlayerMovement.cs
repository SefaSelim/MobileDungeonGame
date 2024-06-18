using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//ANİMASYON SÜRESİNDEN ONCE CİFT TIKLANDIGINDA PROBLEM OLUSUYOR COZULECEK

public class MousePositionOnImage : MonoBehaviour
{
    public Canvas canvas; // Reference to the Canvas
    public RectTransform canvasRectTransform; // Reference to the Canvas's RectTransform
    public RectTransform imageRectTransform; // Reference to the Image's RectTransform
    public GameObject gameObjectToMove; // Reference to the GameObject to teleport
    private float animationDuration = 0.5f; // Duration of the animation
    private float distanceBetweenTargetCurrent;
    public float PlayerSpeed = 1f;

    private Animator animator;

 void Start() {
    animator = GetComponent<Animator>();
}

    public void transformPlayer()
    {
        animator.SetBool("IsWalking",true);

                    Vector3 mousePosition = Input.mousePosition;

            // Convert the screen position to the local position of the Canvas
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform, 
                mousePosition, 
                canvas.worldCamera, 
                out localPoint
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

                StartCoroutine(SlideToPosition(localPoint));
            }
    }



    IEnumerator SlideToPosition(Vector2 targetPosition)
    {
        Vector2 startPosition = gameObjectToMove.transform.localPosition;
        float elapsedTime = 0f;

        if (targetPosition.x > startPosition.x)
        {
            FlipCharacter(1);
        }
        else
        {
            FlipCharacter(-1);
        }

    distanceBetweenTargetCurrent = Mathf.Sqrt(Mathf.Pow((startPosition.x-targetPosition.x),2)+Mathf.Pow((startPosition.y-targetPosition.y),2));
    animationDuration = PlayerSpeed*distanceBetweenTargetCurrent/100;

        while (elapsedTime < animationDuration)
        {
            gameObjectToMove.transform.localPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObjectToMove.transform.localPosition = targetPosition;
        animator.SetBool("IsWalking",false);
    }

        public void FlipCharacter(int rotate)
    {
        // Get the current local scale
        Vector3 currentScale = transform.localScale;
        // Flip the X scale
        if (rotate == 1)
        {
            if (currentScale.x<0)
            {
                currentScale.x *= -1;
            }
        }
        else if (rotate == -1)
        {
            if (currentScale.x>0)
            {
                currentScale.x *= -1;
            }
        }
        // Apply the new scale
        transform.localScale = currentScale;
    }


}

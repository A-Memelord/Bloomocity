using UnityEngine;

public class Headbobbing : MonoBehaviour
{
    public GameObject Camera;

    public float ySpeed;
    public float yScalar;

    public float speed;
    public float scalar;
    public AnimationCurve xOverTime;
    public AnimationCurve yOverTime;
    public AnimationCurve zOverTime;
    private float moveTime;

    public PlayerMovement pm;
    void Update()
    {
        if (GetComponent<PlayerMovement>().grounded == true)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                Camera.transform.localPosition = new Vector3(
                    xOverTime.Evaluate(Mathf.Repeat(moveTime, 1)) * scalar,
                    yOverTime.Evaluate(Mathf.Repeat(moveTime * ySpeed, 1)) * yScalar,
                    zOverTime.Evaluate(Mathf.Repeat(moveTime, 1)) * scalar
                );

                moveTime += Time.deltaTime * speed;
            }
            else
            {
                Camera.transform.localPosition = Vector3.Lerp(Camera.transform.localPosition, new Vector3(0, 0, 0), Time.deltaTime * 5);

                moveTime = 0;
            }
        }
    }
}

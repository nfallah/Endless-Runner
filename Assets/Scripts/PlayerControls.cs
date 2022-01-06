using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [HideInInspector] public bool isJumping;

    [SerializeField] float jumpHeight, speedDown, speedUp;

    [SerializeField] Manager manager;

    private float currentSpeed, percent, timer;

    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = this.transform.position;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            timer = speedUp;
            currentSpeed = speedUp;
            JumpUp();
        }
    }

    private void JumpUp()
    {
        if (!manager.gameEnded)
        {
            Timer();

            this.transform.position = Vector3.Lerp(startingPosition, startingPosition + jumpHeight * Vector3.up, Mathf.Pow(percent, 2f / 3f));

            if (percent < 1 && !manager.gameEnded)
            {
                Invoke("JumpUp", Time.deltaTime);
            }

            else if (!manager.gameEnded)
            {
                timer = speedDown;
                currentSpeed = speedDown;
                Invoke("JumpDown", Time.deltaTime);
            }
        }
    }

    private void JumpDown()
    {
        if (!manager.gameEnded)
        {
            Timer();

            this.transform.position = Vector3.Lerp(startingPosition + jumpHeight * Vector3.up, startingPosition, Mathf.Pow(percent, 7f / 2f));

            if (percent < 1)
            {
                Invoke("JumpDown", Time.deltaTime);
            }

            else if (Input.GetKey(KeyCode.Space))
            {
                timer = speedUp;
                currentSpeed = speedUp;
                Invoke("JumpUp", Time.deltaTime);
            }

            else
            {
                isJumping = false;
            }
        }
    }

    private void Timer()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, currentSpeed);
        percent = 1 - (timer / currentSpeed);
    }
}
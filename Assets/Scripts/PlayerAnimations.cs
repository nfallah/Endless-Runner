using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public float originalMaxRotation;

    [HideInInspector] public float maxRotation;

    [SerializeField] float originalSpeed;

    [SerializeField] PlayerControls playerControls;

    [SerializeField] Manager manager;

    [SerializeField] GameObject[] bodyParts;

    [SerializeField] int[] directions;

    private float percent, timer, speed;

    private int direction = 1;

    private void Start()
    {
        timer = speed = originalSpeed;
        maxRotation = originalMaxRotation;
        Move();
    }

    private void Move()
    {
        if (!manager.gameEnded)
        {
            if (!playerControls.isJumping)
            {
                Timer();

                for (int i = 0; i < directions.Length; i++)
                {
                    bodyParts[i].transform.eulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, maxRotation * direction * directions[i]), percent);
                }

                if (percent < 1)
                {
                    Invoke("Move", Time.deltaTime);
                }

                else
                {
                    speed = originalSpeed * Mathf.Pow(manager.multiplier, 1 / 2f);
                    maxRotation = originalMaxRotation;
                    timer = speed;
                    Invoke("Reset", Time.deltaTime);
                }
            }

            else
            {
                Invoke("Move", Time.deltaTime);
            }
        }
    }

    private void Reset()
    {
        if (!manager.gameEnded)
        {
            if (!playerControls.isJumping)
            {
                Timer();

                for (int i = 0; i < directions.Length; i++)
                {
                    bodyParts[i].transform.eulerAngles = Vector3.Lerp(new Vector3(0, 0, maxRotation * direction * directions[i]), Vector3.zero, percent);
                }

                if (percent < 1)
                {
                    Invoke("Reset", Time.deltaTime);
                }

                else
                {
                    speed = originalSpeed * Mathf.Pow(manager.multiplier, 1 / 2f);
                    maxRotation = originalMaxRotation;
                    timer = speed;
                    direction *= -1;
                    Invoke("Move", Time.deltaTime);
                }
            }

            else
            {
                Invoke("Reset", Time.deltaTime);
            }
        }
    }

    private void Timer()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, speed);
        percent = 1 - (timer / speed);
    }
}
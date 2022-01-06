using UnityEngine;

public class CloudGeneration : MonoBehaviour
{
    [SerializeField] float horizontalMax, horizontalMin, originalSpeed, verticalMax, verticalMin;

    [SerializeField] GameObject cloud, cloudSprite;

    [SerializeField] int cloudIndexMax, cloudIndexMin, cloudLength;

    [SerializeField] Manager manager;

    [SerializeField] Sprite[] cloudsRef;

    private float timer, percent, speed;

    private int cloudIndex;

    private GameObject[] clouds, cloudsCopy;

    private void Start()
    {
        clouds = cloudsCopy = new GameObject[cloudLength];
        cloudIndex = Random.Range(cloudIndexMin, cloudIndexMax + 1);
        timer = speed = originalSpeed;

        for (int i = 0; i < clouds.Length; i++)
        {
            cloudIndex--;

            if (cloudIndex == 0)
            {
                clouds[i] = Instantiate(cloud, new Vector3Int(i * 10, -1, 0), Quaternion.identity);
                clouds[i].name = "Cloud";
                cloudIndex = Random.Range(cloudIndexMin, cloudIndexMax + 1);
                GenerateCloud(clouds[i]);
            }
        }
    }

    private void Update()
    {
        Timer();

        for (int i = 0; i < clouds.Length; i++)
        {
            if (clouds[i] != null)
            {
                clouds[i].transform.position = Vector3.Lerp(new Vector3Int(i * 10, -1, 0), new Vector3Int(i * 10 - 10, -1, 0), percent);
            }
        }

        if (timer == 0)
        {
            if (clouds[0] != null)
            {
                Destroy(clouds[0]);
            }

            cloudsCopy = clouds;

            for (int i = 0; i < clouds.Length - 1; i++)
            {
                cloudsCopy[i] = clouds[i + 1];
            }

            cloudIndex--;

            if (cloudIndex == 0)
            {
                cloudsCopy[clouds.Length - 1] = Instantiate(cloud, new Vector3Int((clouds.Length - 1) * 10, -1, 0), Quaternion.identity);
                cloudsCopy[clouds.Length - 1].name = "Cloud";
                cloudIndex = Random.Range(cloudIndexMin, cloudIndexMax + 1);
                GenerateCloud(clouds[clouds.Length - 1]);
            }

            else
            {
                cloudsCopy[clouds.Length - 1] = null;
            }

            clouds = cloudsCopy;
            speed = originalSpeed * manager.multiplier;
            timer = speed;
        }
    }

    private void GenerateCloud(GameObject cloudChunk)
    {
        GameObject cloud = Instantiate(cloudSprite, cloudChunk.transform);
        cloud.name = "Cloud Sprite";
        cloud.GetComponent<SpriteRenderer>().sprite = cloudsRef[Random.Range(0, cloudsRef.Length)];
        cloud.transform.localPosition = new Vector3(Random.Range(horizontalMin, horizontalMax), Random.Range(verticalMin, verticalMax), 0);
    }

    private void Timer()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, speed);
        percent = 1 - (timer / speed);
    }
}
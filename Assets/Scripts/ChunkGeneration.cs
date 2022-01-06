using UnityEngine;

public class ChunkGeneration : MonoBehaviour
{
    [SerializeField] float horizontalMax, horizontalMin, originalSpeed;

    [SerializeField] GameObject chunk, obstacle;

    [SerializeField] int obstacleIndexMax, obstacleIndexMin, chunkLength;

    [SerializeField] Manager manager;

    [SerializeField] Material[] obstacles;

    private float timer, percent, speed;

    private int obstacleIndex;

    private GameObject[] chunks, chunksCopy;

    private void Start()
    {
        chunks = chunksCopy = new GameObject[chunkLength];
        obstacleIndex = Random.Range(obstacleIndexMin, obstacleIndexMax + 1);
        timer = speed = originalSpeed;

        for (int i = 0; i < chunks.Length; i++)
        { 
            chunks[i] = Instantiate(chunk, new Vector3Int(i * 10, -1, 0), Quaternion.identity);
            chunks[i].name = "Chunk";
        }
    }

    private void Update()
    {
        Timer();

        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].transform.position = Vector3.Lerp(new Vector3Int(i * 10, -1, 0), new Vector3Int(i * 10 - 10, -1, 0), percent);
        }

        if (timer == 0)
        {
            Destroy(chunks[0]);

            if (manager.gameStarted)
            {
                manager.UpdateScore();
            }

            chunksCopy = chunks;

            for (int i = 0; i < chunks.Length - 1; i++)
            {
                chunksCopy[i] = chunks[i + 1];
            }

            if (manager.gameStarted)
            {
                obstacleIndex--;
            }

            chunksCopy[chunks.Length - 1] = Instantiate(chunk, new Vector3Int((chunks.Length - 1) * 10, -1, 0), Quaternion.identity);
            chunksCopy[chunks.Length - 1].name = "Chunk";

            if (obstacleIndex == 0)
            {
                obstacleIndex = Random.Range(obstacleIndexMin, obstacleIndexMax + 1);
                GenerateObstacle(chunks[chunks.Length - 1]);
            }

            chunks = chunksCopy;
            speed = originalSpeed * manager.multiplier;
            timer = speed;
        }
    }

    private void GenerateObstacle(GameObject currentChunk)
    {
        GameObject chunk = Instantiate(obstacle, currentChunk.transform);
        chunk.name = "Obstacle";
        chunk.transform.Find("Quad").GetComponent<MeshRenderer>().material = obstacles[Random.Range(0, obstacles.Length)];
        chunk.transform.localPosition = new Vector3(Random.Range(horizontalMin, horizontalMax), 1, 0);
    }

    private void Timer()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, speed);
        percent = 1 - (timer / speed);
    }
}
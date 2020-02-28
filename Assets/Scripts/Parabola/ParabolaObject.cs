using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaObject : MonoBehaviour
{
	public float speed = 2f;
	public float height = 5f;
	public Vector3 endPosition;

	float prevTime;
	float time;

	int index;

	bool isEnd;

	public float timer;

    // Start is called before the first frame update
    void Start()
    {
		List<Vector3>  path = new List<Vector3>();

		float distance = 0;
		float pathTime = 0;
		for (int i = 0; i < 100; i++)
		{
			time += Time.deltaTime * speed;
			time %= 5f;
			pathTime = time / 5f;

			if (time < prevTime)
				break;

			Vector3 point = MathParabola.Parabola(Vector3.zero, endPosition, height, time / 5f);
			path.Add(point);

			if (i > 0)
				distance += Vector3.Distance(point, path[i - 1]);

			prevTime = time;
		}
		time = prevTime = 0;

		//float pathTime = distance / (speed * Time.deltaTime);
		print(pathTime);
	}

    // Update is called once per frame
    void Update()
    {
		time += Time.deltaTime * speed;
		time %= 5f;

		if (time < prevTime)
			return;

		Vector3 point = MathParabola.Parabola(Vector3.zero, endPosition, height, time / 5f);
		transform.position = point;

		prevTime = time;

		timer += Time.deltaTime;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    public GameObject card;

    void Start()
    {
        int[] arr = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };
        //arr.OrderBy(x => Random.Range(1f, 5f)).ToArray();
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
        for (int i = 0; i < 10; i++)
        {
           GameObject go = Instantiate(card, this.transform);

            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.0f;

            go.transform.position = new Vector2(x, y);
            go.GetComponent<Card>().Setting(arr[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    public GameObject card;
    Transform[] children; 
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

            // float x = (i % 4) * 1.4f - 2.1f;
            // float y = (i / 4) * 1.4f - 3.0f;
            // go.transform.position = new Vector2(x, y);
            go.transform.position = new Vector2(0, -5);

            go.GetComponent<Card>().Setting(arr[i]);
        }
        int childCount = this.transform.childCount;
        children = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            children[i] = this.transform.GetChild(i);
            StartCoroutine(CardMove(i));
        }
       
    }
    IEnumerator CardMove(int index)
    {
        yield return new WaitForSeconds(1.0f);
        float x = (index % 4) * 1.4f - 2.1f;
        float y = (index / 4) * 1.4f - 3.0f;
        Vector2 startPos = children[index].position;
        Vector2 targetPos = new Vector2(x, y);

    float duration = 0.5f; 
    float elapsed = 0f;

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration); 
        children[index].position = Vector2.Lerp(startPos, targetPos, t);
        yield return null;
    }

    children[index].position = targetPos; 


    }

}

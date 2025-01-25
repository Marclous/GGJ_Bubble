using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public int health = 10;

    private void Start()
    {
        StartCoroutine(DecreaseHealthOverTime());
    }

    private IEnumerator DecreaseHealthOverTime()
    {
        while (health > 0)
        {
            yield return new WaitForSeconds(1f);
            health--;

            if (health <= 0)
                Disappear();
        }
    }

    private void Disappear()
    {
        if (BubbleTrigger.instance != null)
            BubbleTrigger.instance.bubbleList.Remove(this);

        Destroy(gameObject);
    }
}

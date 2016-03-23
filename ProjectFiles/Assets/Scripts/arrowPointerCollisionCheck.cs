using UnityEngine;
using System.Collections;

public class arrowPointerCollisionCheck : MonoBehaviour {

    // Use this for initialization

    public Transform vkit;
    public Material[] originalMats = new Material[15];
    public Material newMat;

    private bool kitIsInsideWallNow = false;

	public void InsideWall()
    {
        kitIsInsideWallNow = true;
    }
    public void notInsideWall()
    {
        kitIsInsideWallNow = false;
    }
	
	void OnTriggerEnter(Collider sth)
    {
        if (kitIsInsideWallNow)
        {
            int index = 0;
            foreach (Renderer e in GetComponentsInChildren<Renderer>())
            {
                //originalMats[index] = e.material;
                e.material = newMat;
                index++;
            }
            StartCoroutine(scale(true));
        }
    }

    void OnTriggerExit(Collider sth)
    {
        int index = 0;
        foreach (Renderer e in GetComponentsInChildren<Renderer>())
        {
            e.material = originalMats[index];
            index++;
        }
        Debug.Log("in the trigger");
        StartCoroutine(scale(false));
    }

    IEnumerator scale(bool k)
    {
        float currentTime = 0f;
        if (k)
        {
            while (currentTime <= 1f)
            {
                vkit.localScale = Vector3.Lerp(vkit.localScale, Vector3.zero, currentTime / 0.2f);
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (currentTime <= 1f)
            {
                vkit.localScale = Vector3.Lerp(vkit.localScale, Vector3.one, currentTime / 0.2f);
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
    }

}

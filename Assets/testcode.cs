using UnityEngine;
using System.Collections;

public class testcode : MonoBehaviour
{
    public int des;
    CapsuleCollider co;
    public int tep;
    // Use this for initialization
    void Start()
    {
        co = this.GetComponent<CapsuleCollider>();
        StartCoroutine(test());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator test()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            //Debug.Log(this.transform.position + new Vector3(0, 0, co.height / 2));
            //Debug.Log(this.transform.position + new Vector3(0, 0, -co.height / 2));
            //Debug.Log(co.radius);
            //Debug.Log(new Vector3(0, 0, -co.height));
            //Debug.Log(co.height);

            Debug.Log(Physics.CapsuleCast(
                this.transform.position - new Vector3(0, 0, des),
                this.transform.position + new Vector3(0, 0, des),
                co.radius,
                new Vector3(0, -1, 0)
                ));


        }

    }
    public void touch()
    {
        SceneMaker._instance.CreateDataStart(1);
    }
}
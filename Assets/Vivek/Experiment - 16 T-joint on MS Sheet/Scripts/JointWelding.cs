using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum JointTools
{
    tPlate, chappingHammer
}
public class JointWelding : MonoBehaviour
{
    public static JointWelding instance;
    public int countdotpoint, CurrentLine, countlinePoint;
    public bool isChappingHammer;
    public GameObject[] WeldingLine;
    public GameObject weldingModel, tackPoint,plateRedMat;
    public Material redMaterial, blackMaterial;
    public GameObject fireRedFilerMesh;
    public SoundPlayer chippingHummer;
    public bool isWelding, isFiller;
    public ParticleSystem starParticle;
    public GameObject sparkLight;
    public JointTools jointTools;
    public void Awake()
    {

        instance = this;
        CurrentLine = 0;
        countlinePoint = 0;// WeldingLine[CurrentLine].transform.childCount;
    }
    public void Start()
    {

        /* Color tc = fireRedFilerMesh.GetComponent<Renderer>().material.color;
           tc.a = 0;
           fireRedFilerMesh.GetComponent<Renderer>().material.SetColor("_BaseColor", tc);

        */   /* loat counter = 0;
            RedBolt.GetComponent<Renderer>().material.shader = Shader.Find("HDRP/Lit";
            while (counter < 1)
            {
                counter += Time.deltaTime / 3;
                RedBolt.GetComponent<Renderer>().material.Lerp(materialToChange, materialToChange2, counter);
                yield return 0;
            }
    */
      //  if (jointTools == JointTools.tPlate)
        {
          ///  StartCoroutine(RedPlateFadeIn(3f));
        }
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Electrode")
        {
            isFiller = true;
        }

        if (other.transform.tag == "CleanPoint" && isFiller)//Add By GP
        {
            starParticle.Play();
            starParticle.GetComponent<AudioSource>().Play();
            sparkLight.SetActive(true);
        }

        if (other.transform.tag == "DotPoint" && isFiller)
        {
            countdotpoint++;
            other.transform.GetComponent<MeshRenderer>().material = blackMaterial;
            other.transform.GetComponent<SphereCollider>().enabled = false;
            starParticle.Play();
            starParticle.GetComponent<AudioSource>().Play();
            sparkLight.SetActive(true);
            Color tc = fireRedFilerMesh.GetComponent<Renderer>().material.color;
            tc.a += 0.02f;
            fireRedFilerMesh.GetComponent<Renderer>().material.SetColor("_BaseColor", tc);
            if (countdotpoint == 3)
            {
                GasJointweldingManager.instance.CheckTackPoint();
            }
        }

        if (isWelding && other.transform.tag == "Slag" && isFiller)
        {
            if (WeldingLine[CurrentLine].transform.GetChild(countlinePoint).name == other.transform.name)
            {
                countlinePoint++;
                if (jointTools==JointTools.tPlate)
                {
                    if (countlinePoint == 1)
                    {
                       // StartCoroutine(RedPlateFadeIn(3f));
                    }
                    other.transform.GetComponent<MeshRenderer>().enabled = false;
                    other.transform.GetComponent<BoxCollider>().enabled = false;
                    other.transform.GetChild(1).gameObject.SetActive(true);
                    OnCallFlameCollider();
                }
                else
                {
                    other.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                    chippingHummer.PlayClip(0);
                    GasJointweldingManager.instance.hummerhighlight.SetActive(false);
                    StartCoroutine(HammerBreakpoint(other.gameObject));
                    OnCallHammerMethodCollider();
                }
            }
        }
    }
    //Fade in Coroutine
    public IEnumerator RedPlateFadeIn(float fadeSpeed)
    {
        Renderer rend = plateRedMat.transform.GetComponent<Renderer>();
        Color matColor = rend.material.color;
        float alphaValue = rend.material.color.a;


        //while loop to deincrement Alpha value until object is invisible
        while (rend.material.color.a < 1f)
        {
            alphaValue += Time.deltaTime / fadeSpeed;
            rend.material.color = new Color(matColor.r, matColor.g, matColor.b, alphaValue);
            yield return null;
        }
        //    rend.material.color = new Color(matColor.r, matColor.g, matColor.b, 0f);
     //   StartCoroutine( RedPlateFadeout(fadeSpeed));
       
    }
    //Fade out Coroutine
    public IEnumerator RedPlateFadeout(float fadeSpeed)
    {
        Renderer rend = plateRedMat.transform.GetComponent<Renderer>();
        Color matColor = rend.material.color;
        float alphaValue = rend.material.color.a;


        //while loop to deincrement Alpha value until object is invisible
        while (rend.material.color.a > 0f)
        {
            alphaValue -= Time.deltaTime / fadeSpeed;
            rend.material.color = new Color(matColor.r, matColor.g, matColor.b, alphaValue);
            yield return null;
        }
        //    rend.material.color = new Color(matColor.r, matColor.g, matColor.b, 0f);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Electrode")
        {
            isFiller = false;
            sparkLight.SetActive(false);
          //  starParticle.GetComponent<AudioSource>().Stop();
        //    starParticle.Stop();
        }
    }
    private void OnTriggerStay(Collider other)//Add By GP
    {
      if (other.transform.tag == "CleanPoint" && isFiller)//Add By GP
        {
            starParticle.Play();
            starParticle.GetComponent<AudioSource>().Play();
            sparkLight.SetActive(true);
        }
    }
    public IEnumerator HammerBreakpoint(GameObject other)
    {
        other.transform.GetComponent<MeshRenderer>().enabled = false;
        other.transform.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        other.transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        other.gameObject.SetActive(false);
    }
    public void OnCallHammerMethodCollider()
    {
        if (countlinePoint + 1 <= WeldingLine[CurrentLine].transform.childCount)
        {
            WeldingLine[CurrentLine].transform.GetChild(countlinePoint).gameObject.GetComponent<BoxCollider>().enabled = true;
            WeldingLine[CurrentLine].transform.GetChild(countlinePoint).gameObject.GetComponent<MeshRenderer>().enabled = true;

        }
        else// (countlinePoint <= 0)
        {
            CurrentLine++;
            if (CurrentLine == 2)
            {
                tackPoint.SetActive(false);
                GasJointweldingManager.instance.checkChappingHummer();
            }
            else
            {
                //   WeldingLine[CurrentLine].SetActive(true);
                countlinePoint = 0;
                //  isWelding = false;
                //  WeldingLine[CurrentLine].transform.GetChild(0).gameObject.SetActive(true);
                WeldingLine[CurrentLine].transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
                WeldingLine[CurrentLine].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;

            }
        }
    }
    public void OnCallFlameCollider()
    {
        if (countlinePoint + 1 <= WeldingLine[CurrentLine].transform.childCount)
        {
            
            WeldingLine[CurrentLine].transform.GetChild(countlinePoint).gameObject.SetActive(true);
            starParticle.Play();
            starParticle.GetComponent<AudioSource>().Play();
            sparkLight.SetActive(true);
            Color tc = fireRedFilerMesh.GetComponent<Renderer>().material.color;
            tc.a += 0.02f;
            fireRedFilerMesh.GetComponent<Renderer>().material.SetColor("_BaseColor", tc);
        }
        else// (countlinePoint <= 0)
        {
            CurrentLine++;
            if (CurrentLine == 2)
            {
                tackPoint.SetActive(false);
                weldingModel.SetActive(true);
                //   FreezeRotation.instance.isFreeze = false;
                isFiller = true;
             //   StartCoroutine(RedPlateFadeout(3f));
                GasJointweldingManager.instance.weldingComplete();
            }
            else
            {
                WeldingLine[CurrentLine].SetActive(true);
                countlinePoint = 0;
                isWelding = false;
                //    FreezeRotation.instance.isFreeze = false;
                //  FreezeRotation.instance.Freezeangle = GasJointweldingManager.instance.torch_m_35d.transform;
                GasJointweldingManager.instance.torch_m_35d.SetActive(true);
                GasJointweldingManager.instance.SecondTourchPlateRotate();
                WeldingLine[CurrentLine].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
using UnityEngine;

public class animDebug : MonoBehaviour
{

    private Animator dbAC;

    private float interactLength;
    private bool loopInteract1;

   

    [SerializeField]
    private bool interact1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dbAC = GetComponent<Animator>();
        
      

    }

    // Update is called once per frame
    void Update()
    {
    // add another if with interact 2 if you wanna add more interaction animtions.
        if(interact1)
        {
            dbAC.SetTrigger("trInteract1");
           
        }



        /* 
         // was tryna get the time of the anim playing and tell it to play it again after it's done so the anim doen's tneed to be set to loop but can still loop. however the first time it's triggered it kept grabbing the aniamiton that was playing as the button was pressed. not the new one.
        // so i instead set it up in the aniamtor to transistion from default swimming to things instead of from any. this way it'l just play a little ibto f swimming realize it should be interacting and flip backl
        loopInteract1 = interact1;

    
        if (loopInteract1 && interactLength <= 0)
        {
            dbAC.SetTrigger("trInteract1");
            loopInteract1 = false;

        

                //thank you so much for helping me with getting the animation time anca
                AnimatorStateInfo stateInfo = dbAC.GetCurrentAnimatorStateInfo(0);
                interactLength = stateInfo.length;
                Debug.Log("Lenght of animation = " + interactLength);
 
        }

        if (interactLength > 0)
        {
            interactLength -= Time.deltaTime;
            //Debug.Log("Anim play cooldown = " + interactLength);
        }
        
       

        if (dbAC == null)
        {
            Debug.LogWarning("no animation controller assighned on " + this.gameObject.name);
            return;
        
        }
        */
    }
}

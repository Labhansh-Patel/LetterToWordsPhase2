using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance = null;



	private AudioSource Audio;
	private bool is_music =true;
	public Sprite[] On_Off_sprite; 
	public Toggle Sound;
	public AudioClip[] GameInSound;

    public AudioClip BtnClick;



    void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy (gameObject);



		
		DontDestroyOnLoad (gameObject);
		
	}

	// Use this for initialization
	void Start ()
	{
		//AudioSource Source = GetComponent <AudioSource> ();
		//Audio = Source;
	}
	
	// Update is called once per frame
	void Update ()
 	{
		
		MusicStatus ();

	}





	public void muteSound (){

		is_music = false;
		BgMusicOnOff();
	}

	public void unmuteSound (){

		is_music = true;
		BgMusicOnOff();
	}

	public void BgMusicOnOff()
	{

		if (is_music == true)
			PlayBgMusic ();
		else
			PauseBgMusic ();
	}


	public  void PlayBgMusic()
	{
		//Audio.Play ();
		//Sound.sprite = On_Off_sprite[0];
		AudioListener.pause = false;

	}
	public void PauseBgMusic ()
	{
		//Audio.Pause();
		//Sound.sprite = On_Off_sprite[1]; 
		AudioListener.pause = true;
	 
	}

	public bool MusicStatus()
	{
		
		return is_music;
	}


   public void ToggleBgSound()
   {
	   if(is_music==true)
	   {
		   muteSound();
	   }
	   else{
		   unmuteSound();
	   }
   }




	public bool ToggleSound (){

		is_music = !is_music;
		BgMusicOnOff();
		return is_music;
	}



//    public void OffMusic()
//    {
// 	   Audio.Pause();
//    }
	  public void PlayClip(int clipNum)
     { // used to play a specific clip from another script
       // GetComponent<MoreAudioClips>().PlayClip[2] would play the 3rd clip (index 2) that you set in the Inspector list for MoreAudioCLips
         
         //GetComponent<AudioSource>().PlayOneShot(GameInSound[clipNum]); //uses the AudioSource on the current gameObject
		 AudioSource Source = GetComponent <AudioSource> ();
		
		Source.GetComponent <AudioSource> ().clip=GameInSound[clipNum];
		Audio=Source;
		Audio.Play();
     }



    public void PlayBtnSoud()
    {

  
        AudioSource Source = GetComponent<AudioSource>();
        Source.PlayOneShot(BtnClick, 1);
       // Source.GetComponent<AudioSource>().clip = BtnClick;
        //Audio = Source;
        //Audio.Play();
    }


}

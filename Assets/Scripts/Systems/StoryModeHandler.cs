using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StoryModeHandler : MonoBehaviour
{
    [SerializeField] private GameObject player; // Reference to the player GameObject

    private void Awake()
    {
        // Try to find player if not assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("Player not found! Please assign player reference or tag your player with 'Player' tag.");
            }
        }
    }

    private Vector3 GetNarratorPosition()
    {
        return player != null ? player.transform.position : Vector3.zero;
    }

    public void StartStoryMode()
    {
        StartCoroutine(StorySequence());
    }

    private IEnumerator StorySequence()
    {
        // Intro sequence
        AudioManager.Play("1_narration_intro", GetNarratorPosition(), true, isNarration:true);
        yield return new WaitForSeconds(0.2f);

        // Hood sequence
        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Open hood").completed);
        AudioManager.Play("2_wait_hood_open", GetNarratorPosition(), true, isNarration:true);
        GameStateManager.Instance.ActivateTask("Find coolant");
        yield return new WaitForSeconds(3f);
        
        // Coolant sequence
        AudioManager.Play("3_find_coolant_lost", GetNarratorPosition(), true, isNarration:true);
        yield return new WaitForSeconds(0.2f);

      
        
        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Find coolant").completed);
        AudioManager.Play("4_wait_found_coolant", GetNarratorPosition(), true, isNarration:true);
        yield return new WaitForSeconds(0.2f);

        GameStateManager.Instance.ActivateTask("Fill coolant");

        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Fill coolant").completed);
        AudioManager.Play("5_wait_success_coolant", GetNarratorPosition(), true, isNarration:true);
       

        // start find oil
        GameStateManager.Instance.ActivateTask("Find oil");

        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Find oil").completed);
        AudioManager.Play("6_wait_found_oil", GetNarratorPosition(), true, isNarration:true);

        GameStateManager.Instance.ActivateTask("Fill oil");
        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Fill oil").completed);
        AudioManager.Play("7_wait_oil_success", GetNarratorPosition(), true, isNarration:true);

        GameStateManager.Instance.ActivateTask("Find windshield fluid");

        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Find windshield fluid").completed);

        AudioManager.Play("8_windshield_fluid_found", GetNarratorPosition(), true, isNarration:true);

        GameStateManager.Instance.ActivateTask("Fill windshield fluid");

        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Fill windshield fluid").completed);
        AudioManager.Play("9_windshield_fluid_filled", GetNarratorPosition(), true, isNarration:true);

         GameStateManager.Instance.ActivateTask("Find jumpstarter");

        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Find jumpstarter").completed);
        AudioManager.Play("10_jumpstarter_found", GetNarratorPosition(), true, isNarration:true);

        GameStateManager.Instance.ActivateTask("Jumpstart car");

        AudioManager.Play("10_jumpstarter_found", GetNarratorPosition(), true, isNarration:true);


        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Jumpstart car").completed);
        AudioManager.Play("car_start_effect", GetNarratorPosition(), true, isNarration:false);
        yield return new WaitForSeconds(2f);
        AudioManager.Play("11_car_started", GetNarratorPosition(), true, isNarration:true);

        yield return new WaitForSeconds(15f); // wait for entire narration +2.5s
        AudioManager.Play("12_wait_find_car_jack", GetNarratorPosition(), true, isNarration:true);

        GameStateManager.Instance.ActivateTask("Lift car");
        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Lift car").completed);
        AudioManager.Play("13_wait_lifted_car", GetNarratorPosition(), true, isNarration:true);

        yield return new WaitForSeconds(5.5f); 
        AudioManager.Play("14_find_cross_key", GetNarratorPosition(), true, isNarration:true);

        GameStateManager.Instance.ActivateTask("Find cross key");
        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Find cross key").completed);
        AudioManager.Play("15_wait_found_key", GetNarratorPosition(), true, isNarration:true);

        GameStateManager.Instance.ActivateTask("Attach wheel");
        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Attach wheel").completed);
        AudioManager.Play("16_wait_lower_car", GetNarratorPosition(), true, isNarration:true);

        GameStateManager.Instance.ActivateTask("Remove car jack");
        yield return new WaitUntil(() => GameStateManager.Instance.GetTask("Remove car jack").completed);
        AudioManager.Play("17_final_tasks_done", GetNarratorPosition(), true, isNarration:true);
        yield return new WaitForSeconds(10f);
        
        // Clean up all singletons and managers before loading new scene
        CleanupGame();
        SceneManager.LoadScene("Leaderboard Starting Scene 1");
    }

    // Add this new method to handle cleanup
    private void CleanupGame()
    {
        // Clean up audio first
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.CleanupAudio();
            Destroy(AudioManager.Instance.gameObject);
        }
        
        // Clean up game state
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.CleanupManager();
            Destroy(GameStateManager.Instance.gameObject);
        }
        
        // Optional: Resources cleanup
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    public void PlaySoundOnce(string soundName){
        AudioManager.Play(soundName, GetNarratorPosition(), true, isNarration:false);
    }

    private void Start()
    {
        StartStoryMode();
    }
}


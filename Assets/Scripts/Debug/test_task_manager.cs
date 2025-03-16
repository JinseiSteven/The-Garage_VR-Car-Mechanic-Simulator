using UnityEngine;
using TMPro;

public class test_task_manager : MonoBehaviour
{
    private TextMeshProUGUI textDisplay;
    private GameStateManager gameStateManager;
    
    void Start()
    {
        textDisplay = GetComponent<TextMeshProUGUI>();
        gameStateManager = GameStateManager.Instance;
        
        // Initial text update
        UpdateTaskDisplay();

        // Subscribe to task events to update the display when tasks change
        if (gameStateManager != null)
        {
            gameStateManager.OnTaskCompleted += OnTaskStatusChanged;
            gameStateManager.OnTaskActivated += OnTaskStatusChanged;
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events when the object is destroyed
        if (gameStateManager != null)
        {
            gameStateManager.OnTaskCompleted -= OnTaskStatusChanged;
            gameStateManager.OnTaskActivated -= OnTaskStatusChanged;
        }
    }

    void Update(){
       // UpdateTaskDisplay();
    }
    private void OnTaskStatusChanged(string taskName)
    {
        Debug.Log("Task status changed: " + taskName);
        UpdateTaskDisplay();
    }

    private void UpdateTaskDisplay()
    {
        if (textDisplay != null && GameStateManager.Instance != null)  // Use Instance directly here to ensure we have latest
        {
            string currentTask = "Open hood";
            if (string.IsNullOrEmpty(currentTask))
            {
                textDisplay.text = "No task selected";
                return;
            }

            Task task = GameStateManager.Instance.GetTask(currentTask);  // Use Instance directly here
            
            if (task != null)
            {
                string status = task.completed ? "Completed" : "In Progress";
                string activeStatus = (GameStateManager.Instance.GetActiveTask() == task) ? " (Active)" : "";  // Use Instance directly
                textDisplay.text = $"Task: {task.TaskName}\nStatus: {status}{activeStatus}, activated: {task.activated}, last updated: {System.DateTime.Now}";
            }
            else
            {
                textDisplay.text = $"Task '{currentTask}' not found";
            } 
        }
        else
        {
            Debug.LogWarning("TextDisplay or GameStateManager is null");
        }
    }
}

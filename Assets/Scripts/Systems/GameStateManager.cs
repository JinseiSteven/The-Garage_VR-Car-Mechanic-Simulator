using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;


[Serializable]
public class Task
{
    [NonSerialized] public int id;
    [NonSerialized] public string TaskName;
    [NonSerialized] public bool activated = false;
    [NonSerialized] public bool completed = false;

    [NonSerialized] public float completionTime;

    [SerializeField] public List<GameObject> TaskObjects = new List<GameObject>();
    [SerializeField] public Image Guide;

    public Task(int id, string name)
    {
        this.id = id;
        this.TaskName = name;
    }

    public void CheckOutline()
    {
        foreach (GameObject obj in this.TaskObjects)
        {
            if (obj == null)
            {
                Debug.LogWarning($"Object in the task called \"{this.TaskName}\" (id={this.id}) doesn't exist!");
                continue;
            }
            Outline outline = obj.GetComponent<Outline>();
            if (outline == null)
            {
                outline = obj.AddComponent<Outline>();
            }
            outline.enabled = false;
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 2f;
        }
    }

    public void SetOutline(bool enable)
    {
        foreach (GameObject obj in TaskObjects)
        {
            if (obj != null)
            {
                Outline outline = obj.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = enable;
                }
            }
        }
    }
}


public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;
    public static GameStateManager Instance
    {
        get { return _instance; }
    }

    [Header("Tasks:")]

    [Header("Open hood")]
    public Task OpenHoodTask = new Task(0, "Open hood");

    [Header("Coolant Tasks")]
    public Task FindCoolantTask = new Task(1, "Find coolant");
    public Task FillCoolantTask = new Task(2, "Fill coolant");

    [Header("Oil Tasks")]
    public Task FindOilTask = new Task(3, "Find oil");
    public Task FillOilTask = new Task(4, "Fill oil");

    [Header("Windshield Tasks")]
    public Task FindWindshieldFluidTask = new Task(5, "Find windshield fluid");
    public Task FillWindshieldFluidTask = new Task(6, "Fill windshield fluid");

    [Header("Jumpstart Tasks")]
    public Task FindJumpstarterTask = new Task(7, "Find jumpstarter");
    public Task JumpstartCarTask = new Task(8, "Jumpstart car");

    [Header("Attach Tire Tasks")]

    public Task LiftCarTask = new Task(9, "Lift car");

    public Task FindCrossKeyTask = new Task(10, "Find cross key");

    public Task AttachWheelTask = new Task(11, "Attach wheel");


    public Task RemoveCarJackTask = new Task(11, "Remove car jack");

    [Header("All tasks")]
    public Task AllTasks = new Task(4, "All tasks");

    private List<Task> Tasks = new List<Task>();
    private Task activeTask = null;

    public event Action<string> OnTaskCompleted;
    public event Action<string> OnTaskActivated;

    private InputAction aButtonAction;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManager();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void InitializeManager()
    {
        // Reset all tasks
        Tasks.Clear();
        Tasks.Add(OpenHoodTask);
        Tasks.Add(FindCoolantTask);
        Tasks.Add(FillCoolantTask);
        Tasks.Add(FindOilTask);
        Tasks.Add(FillOilTask);
        Tasks.Add(FindWindshieldFluidTask);
        Tasks.Add(FillWindshieldFluidTask);
        Tasks.Add(FindJumpstarterTask);
        Tasks.Add(JumpstartCarTask);
        Tasks.Add(LiftCarTask);
        Tasks.Add(FindCrossKeyTask);
        Tasks.Add(AttachWheelTask);
        Tasks.Add(RemoveCarJackTask);

        // Reset all task states
        foreach (Task task in Tasks)
        {
            task.activated = false;
            task.completed = false;
            task.completionTime = 0;
            task.CheckOutline();
            task.Guide.enabled = task.TaskName == "Open hood";
        }

        activeTask = null;
    }

    public void CleanupManager()
    {
        // Clear all tasks and reset state
        foreach (Task task in Tasks)
        {
            task.activated = false;
            task.completed = false;
            task.SetOutline(false);
            if (task.Guide != null)
                task.Guide.enabled = false;
        }
        Tasks.Clear();
        activeTask = null;
        
        // Clear event subscribers
        OnTaskCompleted = null;
        OnTaskActivated = null;
    }

    private void Start()
    {
        // Initialize the input action
        var inputActionAsset = GetComponent<PlayerInput>()?.actions;
        if (inputActionAsset != null)
        {
            aButtonAction = inputActionAsset.FindAction("AButton");
        }

        // Then activate initial task
        string currentTask = MenuConfig.taskToDo;
        ActivateTask(currentTask);
    }

    private void Update()
    {
        // Outline all task objects while "a" is being pressed
        if (activeTask != null && aButtonAction != null)
        {
            if (aButtonAction.ReadValue<float>() > 0)
            {
                activeTask.SetOutline(true);
            }
            else
            {
                activeTask.SetOutline(false);
            }
        }
    }

    /// <summary>
    /// Completes the task with the given taskId, this means:
    /// - Setting the task as activeTask
    /// - Enabling the guide on the whiteboard
    /// </summary>
    ///
    /// <param name="taskId">
    /// The unique ID of the task being completed.
    /// </param>
    public void ActivateTask(string taskName)
    {
        Task task = Tasks.Find(t => t.TaskName == taskName);

        if (task == null)
        {
            Debug.LogWarning($"Task {taskName} not found.");
            return;
        }

        if (task.completed)
        {
            Debug.Log($"Task {taskName} is already completed!");
        }

        task.activated = true;
        activeTask = task;
        task.Guide.enabled = true;
        task.completionTime = Time.time;

        Debug.Log($"Task {taskName} is activated!");
    }


    /// <summary>
    /// Completes the task with the given taskId, this means:
    /// - Removing the task as activeTask
    /// - Marking the task as completed
    /// - Disabling the guide on the whiteboard
    /// </summary>
    ///
    /// <param name="taskName">
    /// The unique ID of the task being completed.
    /// </param>
    public void CompleteTask(string taskName)
    {
        Debug.Log("COPMPLETING AAAAA");
        Task task = Tasks.Find(t => t.TaskName == taskName);

        if (task == null)
        {
            Debug.LogWarning($"Task {taskName} not found.");
            return;
        }

        if (task.completed)
        {
            Debug.Log($"Task {taskName} is already completed!");
            return;
        }

        task.completed = true;
        task.activated = false;
        activeTask = null;
        task.Guide.enabled = false;
        task.completionTime = Time.time - task.completionTime;

        Debug.Log($"Task {taskName} is completed!");
    }

    /// <summary>
    /// Notifies that a task has been completed and triggers the <see cref="OnTaskActivated"/>
    /// event, to which <see cref="ActivateTask(int)"/> is subscribed.
    /// </summary>
    ///
    /// <param name="taskName">
    /// The unique ID of the task being activated.
    /// </param>
    public static void NotifyTaskActivated(string taskName)
    {
        if (Instance != null)
        {
            Instance.OnTaskActivated?.Invoke(taskName);
        }
    }

    /// <summary>
    /// Notifies that a task has been completed and triggers the <see cref="OnTaskCompleted"/>
    /// event, to which <see cref="CompleteTask(int)"/> is subscribed.
    /// </summary>
    ///
    /// <param name="taskName">
    /// The unique ID of the task being completed.
    /// </param>
    public void NotifyTaskCompleted(string taskName)
    {
        
        if (Instance != null)
        {
            CompleteTask(taskName);
            OnTaskCompleted?.Invoke(taskName);
        }
    }

    // Public get task method
    public Task GetTask(string taskName)
    {
        return Tasks.Find(t => t.TaskName == taskName);
    }

    // Add this public getter for the active task
    public Task GetActiveTask()
    {
        return activeTask;
    }
}

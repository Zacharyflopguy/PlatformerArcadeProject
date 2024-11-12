using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleMainMenu : MonoBehaviour
{
    public List<ButtonHandle> buttons; // List of all button handles in the menu
    public InputAction upAction;       // Action for moving selection up
    public InputAction downAction;     // Action for moving selection down
    public InputAction selectAction;   // Action for selecting the current button
    private float cooldown;
    
    private int currentIndex = 0;      // Index of the currently selected button

    // Start is called before the first frame update
    void Start()
    {
        if (buttons.Count > 0)
        {
            UpdateButtonSelection(); // Initialize button selection at the start
        }

        cooldown = Time.time + 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < cooldown)
        {
            return;
        }
        
        // Check if the upAction or downAction has been pressed
        if (upAction.WasPerformedThisFrame())
        {
            MoveSelection(-1); // Move up
        }
        else if (downAction.WasPerformedThisFrame())
        {
            MoveSelection(1); // Move down
        }
        
        // Check if the selectAction has been performed
        if (selectAction.WasPerformedThisFrame())
        {
            OnSelect();
        }
    }

    private void MoveSelection(int direction)
    {
        // Unselect the current button
        buttons[currentIndex].Unselect();

        // Update the current index and wrap around if necessary
        currentIndex += direction;
        if (currentIndex < 0)
        {
            currentIndex = buttons.Count - 1;
        }
        else if (currentIndex >= buttons.Count)
        {
            currentIndex = 0;
        }

        // Select the new button
        UpdateButtonSelection();
    }

    private void UpdateButtonSelection()
    {
        // Set the selected sprite for the current button
        buttons[currentIndex].Select();
    }

    private void OnSelect()
    {
        if(buttons[currentIndex].action.Equals("Start"))
        {
            // Load the game scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Slime game");
        }
        else if(buttons[currentIndex].action.Equals("Quit"))
        {
            // Quit the application
            Application.Quit();
        }
        else if(buttons[currentIndex].action.Equals("HowToPlay"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("HowToPlay");
        }
        else if(buttons[currentIndex].action.Equals("ToMainMenu"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnEnable()
    {
        // Enable input actions
        upAction.Enable();
        downAction.Enable();
        selectAction.Enable();
    }

    private void OnDisable()
    {
        // Disable input actions
        upAction.Disable();
        downAction.Disable();
        selectAction.Disable();
    }
}

using UnityEngine;
using System.Collections.Generic;
using System;

public class Player : Character
{
    #region States
    public enum State
    {
        Idle,
        Running,
        Dodging,
        RifleAimedShot,
        ExecutionerSwordAttack,
        GameStart
    }//End States

    [Header("State Options")]
    [SerializeField]
    private State currentState = State.Idle;

    [SerializeField]
    private PlayerState currentStateScript;

    [SerializeField]
    [Tooltip("If you want to enable and disable certain abilities, add or remove the state within this list accordingly.\nA script ensures that only the relevant state scripts are available during play, adding those needed and removing those not needed.")]
    private List<State> states;
    #endregion

    #region Items
    [Header("Item Options")]
    [SerializeField]
    [Tooltip("This is filled on game start with all the item scripts that are held on a child object of the player.\nThey can be added before game start - the code will account for it.")]
    private List<Item> items;
    #endregion

    protected override void Start()
    {
        //Sets the health and stamina to max
        base.Start();

        //Make sure all player states in the state list are initialized on game start
        InitializePlayerStates();

        //Set current state script to active state script
        currentStateScript = (PlayerState) GetComponent(PlayerState.stateDictionary[currentState]);

        //Set up items based on what's on the player
        InitializePlayerItems();
    }//End Start

    protected override void Update()
    {
        base.Update();

        currentStateScript.UpdateState();
    }//End Update

    #region State Managemenet
    private void InitializePlayerStates()
    {
        //Get list of states/state scripts already attached to player on game start
        PlayerState[] playerStates = GetComponentsInChildren<PlayerState>();
        List<State> statesWantedOnPlayer = new List<State>();
        for(int i = 0; i < playerStates.Length; i++)
        {
            statesWantedOnPlayer.Add(playerStates[i].GetStateID());
        }//End for

        //Loop through every state in the list of states we want to have
        foreach(State state in states)
        {
            //If the state we've gotten to in the loop is not on the player
            if (!statesWantedOnPlayer.Contains(state))
            {
                Type type = PlayerState.stateDictionary[state];
                gameObject.AddComponent(type);
            }//End if
        }//End foreach

        //Go through the list of states we already had at the start
        foreach(State state in statesWantedOnPlayer)
        {
            //If the state isn't in the list of states we want
            if(!states.Contains(state))
            {
                //Remove the state from the player
                Type type = PlayerState.stateDictionary[state];
                Destroy(GetComponentInChildren(type));
            }//End if
        }//End foreach
    }//End InitializePlayerStates

    //Returns true if state is added fully, false if it isn't
    private bool AddState(State state)
    {
        //Get the type of state object we're referring to and check if it's already on the player
        Type type = PlayerState.stateDictionary[state];
        bool scriptAlreadyOnPlayer = GetComponent(type);

        //Use an integer to check conditions for adding state
        int check = 0;
        if (scriptAlreadyOnPlayer) check += 1;
        if (states.Contains(state)) check += 10;

        //Check all variations of the conditions
        switch(check)
        {
            //Neither state nor script are already on the player
            case 00:
                Debug.Log(state.ToString() + " added to player successfully.");
                states.Add(state);
                gameObject.AddComponent(type);
                return true;

            //Script is already on the player, but not in the list
            case 01:
                Debug.LogWarning(state.ToString() + " was not in list, but script was found on the player object.\nShouldn't happen, debug this, Jordan.");
                states.Add(state);
                return true;

            //State is already in the list, but the script is not on the player
            case 10:
                Debug.LogWarning(state.ToString() + " was found in list, but the script was not on the player object.\nShouldn't happen, debug this, Jordan.");
                gameObject.AddComponent(type);
                return true;

            //State is already fully implemented on the player
            case 11:
                Debug.LogWarning(state.ToString() + " was both found in state list and script was found on player.\nNo extra data has been added to the player.");
                return false;
        }//End switch

        //This should never be reached - if it is, I've messed up somewhere
        Debug.LogError("AddState call fell through the switch statement when it shouldn't have. Debug this.");
        return false;
    }//End AddState

    //Returns true if state is removed fully, false if it isn't
    private bool RemoveState(State state)
    {
        //Get the type of state object we're referring to and check if it's on the player
        Type type = PlayerState.stateDictionary[state];
        bool scriptIsOnPlayer = GetComponent(type);

        //Use an integer to check conditions for removing state
        int check = 0;
        if (!scriptIsOnPlayer) check += 1;
        if (!states.Contains(state)) check += 10;

        //Check all variations of the conditions
        switch (check)
        {
            //State is fully implemented on the player
            case 00:
                Debug.Log(state.ToString() + " removed from player successfully.");
                states.Remove(state);
                Destroy(GetComponent(type));
                return true;

            //State is in the list, but the script is not on the player
            case 01:
                Debug.LogWarning(state.ToString() + " was in list, but script wasn't found on the player object.\nShouldn't happen, debug this, Jordan.");
                states.Remove(state);
                return true;

            //Script is on the player, but the state is not in the list
            case 10:
                Debug.LogWarning(state.ToString() + " was not in list, but script was found on the player object.\nShouldn't happen, debug this, Jordan.");
                Destroy(GetComponent(type));
                return true;

            //State is fully absent on player already
            case 11:
                Debug.LogWarning(state.ToString() + " was not found on player at all.\nNo data has been removed from the player.");
                return false;
        }//End switch

        //This should never be reached - if it is, I've messed up somewhere
        Debug.LogError("RemoveState call fell through the switch statement when it shouldn't have. Debug this.");
        return false;
    }//End RemoveState

    //Returns true if state is present, false if it isn't
    public bool HasState(State state)
    {
        return states.Contains(state);
    }//End HasState

    //Returns state if present in states list
    public PlayerState GetState(State state)
    {
        if(states.Contains(state))
        {
            return (PlayerState) GetComponentInChildren(PlayerState.stateDictionary[state]);
        }//End if

        Debug.LogError("Tried to return state of type " + state.ToString() + " but it was not found on the player.");
        return null;
    }//Ebd GetState

    public bool ChangeState(State stateToChangeTo)
    {
        if(states.Contains(stateToChangeTo))
        {
            //Can add checks to see if exiting or entering states fail, not doing that for now while the methods are empty and always return true
            currentStateScript.ExitState();
            currentState = stateToChangeTo;
            currentStateScript = (PlayerState)GetComponent(PlayerState.stateDictionary[currentState]);
            currentStateScript.EnterState();
            Debug.Log("Changed to " + currentState.ToString() + " successfully.");
            return true;
        }//End if
        else
        {
            Debug.LogError("Tried to change state to " + stateToChangeTo.ToString() + ", but it is not on the player.");
            return false;
        }//End else
    }//End ChangeState

    //A workaround I despise needing for the sake of start button on-click events hating non-void return types
    public void StartGame()
    {
        ChangeState(State.Idle);
    }//End ChangeState
    #endregion

    #region Item Management
    private void InitializePlayerItems()
    {
        Item[] itemsCurrentlyOnPlayer = GetComponentsInChildren<Item>();
        //If the actual items list isn't created already, create it using the items currently on the player
        if (items == null)
        {
            items = new List<Item>(itemsCurrentlyOnPlayer);
        }//End if
        //Otherwise, remove items not actually there, and fill the list with the items that aren't yet on the list
        else
        {
            foreach(Item item in items)
            {
                //Remove every item in the items list that doesn't actually have an object attached to the player in some way
                if(!GetComponentInChildren(item.GetType()))
                {
                    items.Remove(item);
                }//End if
            }//End foreach
            foreach(Item item in itemsCurrentlyOnPlayer)
            {
                //Add every item wanted on the player to the list if it isn't already there
                if(!items.Contains(item))
                {
                    items.Add(item);
                }//End if
            }//End foreach
        }//End else
    }//End InitializePlayerItems

    //Returns true if item is on the player, false if it isn't
    public bool HasItem(Type item)
    {
        return items.Contains((Item) GetComponentInChildren(item));
    }//End HasItem

    //Returns the item if it is on the player
    public Item GetItem(Type item)
    {
        if (HasItem(item))
        {
            for(int i = 0; i < items.Count; i++)
            {
                if(items[i].GetType() == item)
                {
                    return items[i];
                }//End if
            }//End for
            return null;
        }//End if
        else
        {
            Debug.LogWarning("Tried to get item of type " + item.Name + " but none was found on the player.");
            return null;
        }//End else
    }//End GetItem
    #endregion
}
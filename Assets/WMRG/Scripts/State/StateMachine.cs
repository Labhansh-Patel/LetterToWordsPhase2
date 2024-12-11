using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
    private IState _CurrentState;
    private IState _previousState;
    public void  ChangeState(IState _newstate)
    {

        if (_CurrentState != null)
        {
            _CurrentState.Exit();
        }

        _previousState = _CurrentState;

        _CurrentState = _newstate;

        _CurrentState.Enter();
    }

    public void ExecuteUpdateState()
    {
        var _RunningState = this._CurrentState;
        if (_RunningState != null)
        {
            _RunningState.Execute();
        }



    }

    public void SwitchToPreviousState()
    {
        if (_previousState != null)
        {

            _CurrentState.Exit();
            //IState MyLocalPreviousStates = _CurrentState;

            _CurrentState = this._previousState;
            //this._previousState = MyLocalPreviousStates;

            _CurrentState.Enter();
        }
    }

   


}

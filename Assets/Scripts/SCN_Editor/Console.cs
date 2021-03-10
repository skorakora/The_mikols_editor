using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Console
{
    
    List<string> commands = new List<string>();
    int command_counter = 0;




    void ExecuteCommand(string command)//commands list
    {

        if (command.StartsWith("spawn"))
        {
            commands.Add(command);
            command_counter++;
            spawn(command);
        }

        if (command.StartsWith("move"))
        {
            commands.Add(command);
            command_counter++;
            move(command);
        }

        if (command.StartsWith("delete"))
        {
            commands.Add(command);
            command_counter++;
            delete(command);
        }

        else
        {
            //todo Wrong command message
            return;
        }
    }

    void undo()
    {
        if (command_counter == 0)//nothing to undo
        {
            return;
        }
        else //undo commands
        {
            command_counter--;
            if (commands[command_counter].StartsWith("spawn"))
            {

            }

            if (commands[command_counter].StartsWith("move"))
            {

            }

            if (commands[command_counter].StartsWith("delete"))
            {

            }
        }


    }

    //----------------------------------------------------------commands---------------------------------------------------

    private void spawn(string command)
    {

    }

    private void move(string command)
    {

    }

    private void delete(string command)
    {

    }


}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Parser : MonoBehaviour
{

    public string GetToken(StreamReader file)
    {
        List<byte> buf = new List<byte>(); //bufor z wczytywanym słowem


        while (true)//pomija wszystko co nie jest literami
        {
            int _char = file.Read();
            if (_char == -1) return null;
            if (IsLetter(file, _char))
            {
                buf.Add(Convert.ToByte(_char));
                break;
            }

        }

        while (true)//czyta wyraz 
        {
            int _char = file.Read();
            if (_char == -1) return null;
            if (IsLetter(file, _char))//sprawdza czy jest litera
            {
                buf.Add(Convert.ToByte(_char));
            }
            else
            {
                break;
            }
        }


        return System.Text.Encoding.UTF8.GetString(buf.ToArray());
    }

    private bool IsLetter(StreamReader file, int _char)
    {

        if (_char == -1)//sprawdza czy doszło do końca pliku
        {
            return false;
        }
        else if (_char == 13)//sprawdza czy nie ma znaku końca linii
        {
            _char = file.Read();
            if (_char == 10)
            {
                return false;
            }
            return false;
        }
        else if (_char == 32 || _char == 9)// sprawdza czy znak to nie tabulator lub spacja
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

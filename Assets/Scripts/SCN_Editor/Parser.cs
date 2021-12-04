using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Parser : MonoBehaviour
{

    public string GetToken(FileStream file)
    {
        List<byte> buf = new List<byte>(); //bufor z wczytywanym słowem
        int _char;

        while (true)//pomija wszystko co nie jest literami
        {
            _char = file.ReadByte();
            if (_char == -1) return null;
            if (IsLetter(file, _char))
            {
                buf.Add(Convert.ToByte(_char));
                break;
            }

        }



        if (_char == 47)//pomija komentarze
        {
            _char = file.ReadByte();
            if (_char == 47)
            {
                SkipComment(file, _char);
                buf.RemoveAt(buf.Count - 1);
                goto readWord;
            }
            else if (_char == 42)
            {
                SkipBlockComment(file, _char);
                buf.RemoveAt(buf.Count - 1);
                goto readWord;
            }
            buf.Add(Convert.ToByte(_char));
            
        }
        
        readWord:

        while (true)//czyta wyraz 
        {
            _char = file.ReadByte();
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


    private void SkipBlockComment(FileStream file, int _char)
    {
        while (true)
        {
            _char = file.ReadByte();
            if (_char == -1)
            {
                Debug.LogError("ERROR: wrong block comment, reached end of file!");
                return;
            }
            if (_char == 42)
            {
                _char = file.ReadByte();
                if (_char == 47)
                {
                    return;
                }
            }
        }
    }

    private void SkipComment(FileStream file, int _char)
    {
        while (true)
        {
            _char = file.ReadByte();
            if (_char == -1)
            {
                return;
            }
            if (_char == 13)
            {
                _char = file.ReadByte();
                if (_char == 10)
                {
                    return;
                }
            }
        }
    }

    private bool IsLetter(FileStream file, int _char)
    {

        if (_char == -1)//sprawdza czy doszło do końca pliku
        {
            return false;
        }
        else if (_char == 13)//sprawdza czy nie ma znaku końca linii
        {
            _char = file.ReadByte();
            if (_char == 10)
            {
                return false;
            }
            Debug.Log(_char + "Debbuger, pominięto.");
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


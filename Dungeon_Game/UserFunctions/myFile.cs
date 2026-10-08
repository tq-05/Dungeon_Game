using Microsoft.Xna.Framework; 
using Microsoft.Xna.Framework.Graphics; 
using Microsoft.Xna.Framework.Input; 
using System.Xml.Linq; 
using System.IO; 
using System.Runtime.CompilerServices; 
using System.Runtime.InteropServices; 
using System; 
using System.Linq; 
using System.Collections.Generic; 
using System.Net.Mime; 

public class myFile 
{    
    private string file1Path, file2Path, file3Path, weapon, highScore; 
    private string[] fileData; 
    public myFile() 
    { 
        file1Path = @"C:\Users\marti\Downloads\file1.txt"; 
        file2Path = @"C:\Users\marti\Downloads\file1.txt"; 
        file3Path = @"C:\Users\marti\Downloads\file1.txt"; 
        fileData = new string[2]; 
    } 
 
    public void createFile() 
    { 
        if (File.Exists(file1Path)) 
        { 
            Console.WriteLine("file1 exist"); 
        } 
        else 
        { 
            File.Create(file1Path); 
        } 
 
        if (File.Exists(file2Path)) 
        { 
            Console.WriteLine("file2 exist"); 
        } 
        else 
        { 
            File.Create(file2Path); 
        } 
 
        if (File.Exists(file3Path)) 
        { 
            Console.WriteLine("file3 exist"); 
        } 
        else 
        { 
            File.Create(file3Path); 
        } 
 
         
    } 
     private void retrieveFileData(int fileNumber) 
    { 
        switch (fileNumber) 
        { 
            // file 1 
            case 1: 
            if (File.ReadAllLines(file1Path).Length == 0) 
            { 
                this.weapon = "-"; 
                this.highScore = "-"; 
                Globals.isLoaded = false; 
            } 
            else 
            { 
                fileData = File.ReadAllLines(file1Path); 
                this.weapon = fileData[0]; 
                this.highScore = fileData[1]; 
            } 
            break; 
 
            // file 2 
            case 2: 
            if (File.ReadAllLines(file2Path).Length == 0) 
            { 
                this.weapon = "-"; 
                this.highScore = "-"; 
                Globals.isLoaded = false; 
            } 
            else 
            { 
                fileData = File.ReadAllLines(file2Path); 
                this.weapon = fileData[0]; 
                this.highScore = fileData[1]; 
            } 
            break; 
 
            // file 3 
            case 3: 
            if (File.ReadAllLines(file3Path).Length == 0) 
            { 
                this.weapon = "-"; 
                this.highScore = "-"; 
                Globals.isLoaded = false; 
            } 
            else 
            { 
                 
                fileData = File.ReadAllLines(file3Path); 
                this.weapon = fileData[0]; 
                this.highScore = fileData[1]; 
            } 
            break; 
 
        } 
         
    } 
    public (string, string) getFileData(int fileNumber) 
    { 
        retrieveFileData(fileNumber); 
        return (weapon, highScore); 
    } 
    public void writeToFile(string weapon, string highScore) 
    { 
        string[] temp = new string[2]; 
 
        if(weapon == "u") 
        { 
            switch (Globals.selectedFileNumber) 
            { 
                // file 1 
                case 1: 
                temp = File.ReadAllLines(file1Path); 
                int previousHighScore = Convert.ToInt32(temp[1]); 
                if(Convert.ToInt32(highScore) > previousHighScore) 
                { 
                    File.WriteAllText(file1Path, temp[0] + "\n" + highScore); 
                } 
                break; 
 
                // file 2 
                case 2: 
                temp = File.ReadAllLines(file2Path); 
                int previousHighScore2 = Convert.ToInt32(temp[1]); 
                if(Convert.ToInt32(highScore) > previousHighScore2) 
                { 
                    File.WriteAllText(file2Path, temp[0] + "\n" + highScore); 
                } 
                break; 
 
                // file 3
                 case 3: 
                temp = File.ReadAllLines(file3Path); 
                int previousHighScore3 = Convert.ToInt32(temp[1]); 
                if(Convert.ToInt32(highScore) > previousHighScore3) 
                { 
                    File.WriteAllText(file3Path, temp[0] + "\n" + highScore); 
                } 
                break; 
            } 
        } 
        else 
        { 
            switch (Globals.selectedFileNumber) 
            { 
                // file 1 
                case 1: 
                File.WriteAllText(file1Path, weapon + "\n" + highScore); 
                break; 
 
                // file 2 
                case 2: 
                File.WriteAllText(file2Path, weapon + "\n" + highScore); 
                break; 
 
                // file 3 
                case 3: 
                File.WriteAllText(file3Path, weapon + "\n" + highScore); 
                break; 
            } 
             
        } 
         
    } 
 
    public void deleteFile(int fileNumber) 
    { 
 
         switch (fileNumber) 
        { 
            // file 1 
            case 1: 
            File.WriteAllText(file1Path, ""); 
            break; 
 
            // file 2
            case 2: 
            File.WriteAllText(file2Path, ""); 
            break; 
            // file 3 
            case 3: 
            File.WriteAllText(file3Path, ""); 
            break; 
        } 
    } 
}

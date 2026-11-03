using Microsoft.Xna.Framework; 
using Microsoft.Xna.Framework.Graphics; 
using Microsoft.Xna.Framework.Input; 
using System.Xml.Linq; 
using System.IO; 
using System.Runtime.CompilerServices; 
using System.Runtime.InteropServices; 
using System; 
using System.Net.Mime; 
using System.Collections.Generic; 
using System.Xml; 
 
public class fileMenu 
{    
    private int numberOfloadButtons; 
    private string dHS1,dHS2,dHS3,dW1,dW2,dW3 ; 
    public float drawcd; 
    
    private Vector2 fileTexture = new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.12), (float)(Globals._graphics.PreferredBackBufferHeight*0.27));
    private Sprite2d fileUI; 
    private List<Font2d> text; 
    private Font2d font; 
    private myMouse mouse; 
    private Buttons loadButton, deleteButton; 
    private myFile file; 
    private List<Sprite2d> fileInterfaces; 
    private List<Buttons> loadButtons, deleteButtons; 

    public bool isDone; 
     
 
    public fileMenu(myMouse MOUSE) 
    { 
        text = new List<Font2d>(); 
        drawcd =0; 
        numberOfloadButtons = 0; 
        file = new myFile(); 
        fileInterfaces = new List<Sprite2d>(); 
        loadButtons = new List<Buttons>(); 
        deleteButtons = new List<Buttons>(); 
        mouse = MOUSE; 
        displayText(); 
        #region User Interface stuff 
        for (numberOfloadButtons = 0; numberOfloadButtons < 3; numberOfloadButtons++ ) 
        { 
            switch(numberOfloadButtons) 
            { 
                case 0: 
                    loadButton = new Buttons("LOAD",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.2), Globals._graphics.PreferredBackBufferHeight / 2), new Vector2 (200,50)); 
                    loadButtons.Add(loadButton); 

                    deleteButton = new Buttons("DELETE",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.16), (float)(Globals._graphics.PreferredBackBufferHeight *0.385)), new Vector2 (50,25)); 
                    deleteButtons.Add(deleteButton); 

                    fileUI = new Files("file",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.2), Globals._graphics.PreferredBackBufferHeight / 2), fileTexture);
                    fileInterfaces.Add(fileUI); 

                    Console.WriteLine("loadButton one has been added to the list"); 
                    break; 


                case 1: 
                    loadButton = new Buttons("LOAD",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.5), Globals._graphics.PreferredBackBufferHeight / 2), new Vector2 (200,50)); 
                    loadButtons.Add(loadButton); 
                    
                    deleteButton = new Buttons("DELETE",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.46), (float)(Globals._graphics.PreferredBackBufferHeight *0.385)), new Vector2 (50,25)); 
                    deleteButtons.Add(deleteButton); 

                    fileUI = new Files("file",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.5), Globals._graphics.PreferredBackBufferHeight / 2), fileTexture); 
                    fileInterfaces.Add(fileUI); 

                    Console.WriteLine("loadButton two has been added to the list"); 
                    break; 


                case 2: 
                    loadButton = new Buttons("LOAD",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.8), Globals._graphics.PreferredBackBufferHeight / 2), new Vector2 (200,50)); 
                    loadButtons.Add(loadButton); 

                    deleteButton = new Buttons("DELETE",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.76), (float)(Globals._graphics.PreferredBackBufferHeight *0.385)), new Vector2 (50,25)); 
                    deleteButtons.Add(deleteButton);

                    fileUI = new Files("file",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.8), Globals._graphics.PreferredBackBufferHeight / 2), fileTexture);
                    fileInterfaces.Add(fileUI); 

                    Console.WriteLine("loadButton three has been added to the list"); 
                    break; 
            } 
 
        } 
        #endregion 
 
    } 
    public virtual void Update() 
    { 
        drawcd+= (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
        // so it doesnt overlap with the last user input 
        (dW1,dHS1) = file.getFileData(1); 
        (dW2,dHS2) = file.getFileData(2); 
        (dW3,dHS3) = file.getFileData(3); 
        displayText(); 
        if(drawcd < 0.5) 
        { 
            drawcd+= (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
        } 
        else 
        { 
             
            // for file selection 
            if( mouse != null && loadButtons.Count >= 3 && deleteButtons.Count >= 3) 
            { 
                #region FILE SELECTION 
                if(loadButtons[0].isClicked(mouse.mousePos , mouse.IsLeftMouseButtonPressed)) 
                { 
                    Console.WriteLine("file1 has been clicked"); 
                    isDone = true; 
                    Globals.selectedFileNumber = 1; 
                    drawcd= 0; 
                    return; 
                } 
 
                if(loadButtons[1].isClicked(mouse.mousePos , mouse.IsLeftMouseButtonPressed)) 
                {
                     Console.WriteLine("file2 has been clicked"); 
                    isDone = true; 
                    Globals.selectedFileNumber = 2; 
                    drawcd= 0; 
                    return; 
                } 
                 
                if(loadButtons[2].isClicked(mouse.mousePos , mouse.IsLeftMouseButtonPressed)) 
                { 
                    Console.WriteLine("file3 has been clicked"); 
                    isDone = true;  
                    Globals.selectedFileNumber = 3; 
                    drawcd= 0; 
                    return; 
                } 
                #endregion 
                #region DELETE SYSTEM 
                if(deleteButtons[0].isClicked(mouse.mousePos , mouse.IsLeftMouseButtonPressed)) 
                { 
                    file.deleteFile(1); 
                } 
 
                if(deleteButtons[1].isClicked(mouse.mousePos , mouse.IsLeftMouseButtonPressed)) 
                { 
                    file.deleteFile(2); 
                } 
                 
                if(deleteButtons[2].isClicked(mouse.mousePos , mouse.IsLeftMouseButtonPressed)) 
                { 
                    file.deleteFile(3); 
                } 
                #endregion 
            } 
             
 
        } 
    } 
    public bool fileSelected() 
    { 
        return isDone; 
    } 
    public void displayText() 
    { 
        text = new List<Font2d>(); 
        for (int texts = 0; texts < 3; texts++ ) 
            { 
                switch(texts) 
                { 
                    case 0: 
                    font = new Font2d("font",new Vector2 ((float)(Globals._graphics.PreferredBackBufferWidth*0.16), (float)(Globals._graphics.PreferredBackBufferHeight*0.43)), "Weapon: " + dW1 + "\nHighScore: " + dHS1 ); 
                    text.Add(font); 
                    break; 
 
                    case 1: 
                    font = new Font2d("font",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.46), (float)(Globals._graphics.PreferredBackBufferHeight*0.43)), "Weapon: " + dW2 + "\nHighScore: " + dHS2 ); 
                    text.Add(font); 
                    break; 
 
                    case 2: 
                    font = new Font2d("font",new Vector2 ((float)(Globals._graphics.PreferredBackBufferWidth*0.76), (float)(Globals._graphics.PreferredBackBufferHeight*0.43)), "Weapon: " + dW3 + "\nHighScore: " + dHS3 ); 
                    text.Add(font); 
                    break; 
                } 
 
            } 
    } 
 
    public virtual void Draw() 
    { 
        foreach(var fileInterface in fileInterfaces) 
        { 
            fileInterface.Draw(); 
        } 
         
        foreach(var loadButton in loadButtons) 
        { 
            loadButton.Draw(); 
        } 
 
        foreach (var font in text) 
        { 
            font.Draw(); 
        } 
  
        foreach(var deleteButton in deleteButtons) 
        { 
            deleteButton.Draw(); 
        } 
    } 
 
} 
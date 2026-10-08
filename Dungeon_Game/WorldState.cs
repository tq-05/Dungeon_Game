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
public class WorldState{ 
    #region  variables 
    public Player player; 
    public mainMenu menu; 
    private SpriteFont font; 
    myMouse mouse; 
    public bool isGameOver, exitKeyPressed, exit; 
    private float keyPressCd,finishCD; 
    public fileMenu fileSelection; 
    public weaponMenu weaponSelection; 
    public myFile file = new myFile(); 
    public dungeonGenerator dungeon; 
    public gameOverMenu gameOverScreen; 
    bool generate = false; 
    #endregion 
    public WorldState(myMouse MOUSE) 
    { 
        file.createFile(); 
        mouse = MOUSE; 
        isGameOver = false; 
        dungeon = new dungeonGenerator(); 
        font = Globals.content.Load<SpriteFont>("font"); 
        menu = new mainMenu(mouse); 
        fileSelection = new fileMenu(mouse); 
        weaponSelection = new weaponMenu(mouse); 
    } 
    public virtual void Update() 
    { 
        completeFloor(); 
        getExit(); 
        getMenus(); 
    } 
    public void getMenus() // gets the functionality of each menu 
    { 
        if(!isGameOver) 
        { 
             if(!menu.menuOn()) 
        { 
            menu.Update(); 
        } 
        else 
        { 
 if (!fileSelection.fileSelected()) 
            { 
                fileSelection.Update(); 
            } 
            else 
            { 
                if (file.getFileData(Globals.selectedFileNumber) != ("-","-")) 
                { 
                    if(!generate) 
                    { 
                        player = new Player("Player",new Vector2(Globals._graphics.PreferredBackBufferWidth / 2, Globals._graphics.PreferredBackBufferHeight / 2), new Vector2 (75,75), mouse, dungeon); 
                        generate = true; 
                    } 
                    else 
                    { 
                        player.Update(); 
                    } 
                } 
                else 
                { 
                    weaponSelection.Update(); 
                } 
                 
            } 
        } 
        } 
        else 
        { 
            gameOverScreen = new gameOverMenu(mouse); 
            gameOverScreen.Update(); 
            finishCD += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
            if(gameOverScreen.isFinishButtonPressed() && finishCD > 2) 
            { 
                isGameOver = false; 
                Globals.totalKills = 0; 
                Globals.totalTime = 0; 
            } 
        } 
    } 
    public void getExit() // gets the functionality of the settings 
    { 
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) 
 { 
            exitKeyPressed = true; 
        } 
        if(exitKeyPressed) 
        { 
            keyPressCd += (float)(Globals.gameTime.ElapsedGameTime.TotalSeconds); 
            if (keyPressCd > 0.5) 
            { 
                if (Keyboard.GetState().IsKeyDown(Keys.Escape)) 
                { 
                    exit = true;  
                } 
                if (Keyboard.GetState().IsKeyDown(Keys.Space)) 
                { 
                    exitKeyPressed = false; 
                    keyPressCd = 0; 
                } 
                if (Keyboard.GetState().IsKeyDown(Keys.M)) 
                { 
                    dungeon = new dungeonGenerator(); 
                    generate = false; 
                    exitKeyPressed = false; 
                    menu.isMenu = false; 
                    fileSelection.isDone = false; 
                    fileSelection.drawcd = 0.0f; 
                    Globals.totalTime = 0; 
                    Globals.totalKills = 0; 
                    keyPressCd = 0; 
                } 
                if (Keyboard.GetState().IsKeyDown(Keys.L) && menu.menuOn()) 
                { 
                    dungeon = new dungeonGenerator(); 
                    generate = false; 
                    exitKeyPressed = false; 
                    fileSelection.isDone = false; 
                    Globals.totalTime = 0; 
                    Globals.totalKills = 0; 
                    keyPressCd = 0; 
                } 
            } 
             
        } 
    } 
    public void displayMenus() // displays the menus 
    {
         if (!isGameOver) 
        { 
            if(!menu.menuOn()) 
            { 
                menu.Draw(); 
            } 
            else 
            { 
                if (!fileSelection.fileSelected()) 
                { 
                    fileSelection.Draw(); 
                } 
                else 
                { 
                    if (file.getFileData(Globals.selectedFileNumber) != ("-","-")) 
                    { 
                        if (generate) 
                        { 
                            player.Draw(); 
                            dungeon.Draw(); 
                        } 
                    } 
                    else 
                    { 
                        weaponSelection.Draw(); 
                    } 
                } 
            } 
        } 
        else 
        { 
            gameOverScreen = new gameOverMenu(mouse); 
            gameOverScreen.Draw(); 
        } 
    } 
    public void displayExit() // displayes the settings option 
    { 
        if (exitKeyPressed) 
        { 
            Globals._spriteBatch.DrawString(font, "Press ESC to exit \nPress SPACE to return \nPress M to return to Main Menu \nPress L to return to Save Files ",new Vector2(0,0), Color.White); 
        } 
    } 
    public virtual void Draw() 
    { 
        displayMenus(); 
        displayExit(); 
    } 
    public void completeFloor() // checks if the user has found the dungeon room and entered the portal 
    { 
        if (dungeon != null && player != null) 
        { 
            if(dungeon.isFloorFinished()) 
            { 
                generate = false; 
                exitKeyPressed = false; 
                menu.isMenu = false; 
                fileSelection.isDone = false; 
                fileSelection.drawcd = 0.0f; 
                keyPressCd = 0; 
                isGameOver = true; 
                dungeonGenerator.isBossRoom = false; 
                dungeon = new dungeonGenerator(); 
            } 
            if(!Player.checkIfPlayerIsAlive()) 
            { 
                Globals.totalTime = 999; 
                generate = false; 
                exitKeyPressed = false; 
                menu.isMenu = false; 
                fileSelection.isDone = false; 
                fileSelection.drawcd = 0.0f; 
                keyPressCd = 0; 
                isGameOver = true; 
                dungeonGenerator.isBossRoom = false; 
                dungeon = new dungeonGenerator(); 
                Player.playerHP = 1; 
            } 
        } 
         
         
    } 
} 

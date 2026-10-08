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
public class dungeonGenerator 
{ 
 
    #region  
    public string[,] dungeon; 
    Font2d font; 
    public Sprite2d portal; 
    public List<Sprite2d> storePortal; 
    float bossRoomNotification; 
    public List<(int,int)> playerSpawns; 
    public static bool isBossRoom; 
    #endregion 
    public dungeonGenerator() 
    { 
        font = new Font2d("mediumFont",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.1), (float)(Globals._graphics.PreferredBackBufferHeight*0.2)), "YOU HAVE ENTERED THE BOSS ROOM"); 
        bossRoomNotification = 0; 
        storePortal = new List<Sprite2d>(); 
        playerSpawns = new List<(int, int)>(); 
        dungeon = new string[4,4]; 
    } 
 
    public void createDungeon() // calls different methods to create the dungeon, stores the starting rooms generated 
    { 
        isBossRoom = false; 
        List<(int,int)> spawnedRooms = new List<(int, int)>(); 
        List<(int,int)> connectUnpairedRooms = new List<(int, int)>(); 
        initializeDungeon(spawnedRooms); 
        playerSpawns = new List<(int, int)>(spawnedRooms); 
        showDungeon(); 
        createCorridors(spawnedRooms,connectUnpairedRooms); 
        showDungeon(); 
        createBossRoom(playerSpawns); 
        Console.WriteLine("dungeon has been created"); 
        showDungeon(); 
         
 
         
    } 
 private void initializeDungeon(List<(int,int)> spawnedRooms) // fills the dungeon with empty spaces represented as ! and spawns 4 random starting rooms which cannot have the same (0,x) value represented as # 
    { 
        List<int> columnsLeft = new List<int> {0,1,2,3}; 
        int createRoom = Globals.random.Next(0,4); 
 
        for (int row = 0; row < 4; row++) 
        { 
            while (!columnsLeft.Contains(createRoom) && columnsLeft.Count !=0) 
            { 
                createRoom = Globals.random.Next(0,4); 
            } 
 
            for(int column = 0; column < 4; column++) 
            { 
                dungeon[row,column] = "!"; 
            } 
            dungeon[row,createRoom] = "#"; 
            spawnedRooms.Add((row,createRoom)); 
            columnsLeft.Remove(createRoom); 
             
        } 
        Console.WriteLine($"SpawnedRooms Count: {spawnedRooms.Count}"); 
        foreach (var room in spawnedRooms) 
        { 
            Console.WriteLine($"Room: ({room.Item1}, {room.Item2})"); 
        } 
 
    } 
    private void createCorridors(List<(int,int)> spawnedRooms,List<(int,int)> connectUnpairedRooms) // calls different methods to create corridors 
    { 
        bool finished = false; 
        int firstRoomY,firstRoomX,secondRoomY,secondRoomX; 
        List<(int,int)> room1CastedLines ,room2CastedLines; 
        bool connected = false; 
        while (!finished) 
        { 
            connected = false; 
            getRooms(spawnedRooms,out firstRoomX,out firstRoomY,out secondRoomX,out secondRoomY); 
            getCastedLines(firstRoomX,firstRoomY,secondRoomX,secondRoomY, out room1CastedLines, out room2CastedLines); 
            foreach((int,int) castedCoordinate in room1CastedLines) 
            { 
                if (room2CastedLines.Count > 0 && !connected) 
                { 
                    foreach((int,int) castedCoordinate2 in room2CastedLines) 
                    { 
                        if (castedCoordinate == castedCoordinate2) 
                        { 
                            Console.WriteLine("the two coords re being checked"); 
                            if(!connected) 
                            { 
                                Console.WriteLine("COORDS HAVE BEEN FOUND"); 
                                connectRooms(castedCoordinate,castedCoordinate2,firstRoomX,firstRoomY,secondRoomX,secondRoomY); 
                                showDungeon(); 
                                connectUnpairedRooms.Add((firstRoomY,firstRoomX))
; 
                                spawnedRooms.Remove((firstRoomY,firstRoomX)); 
                                spawnedRooms.Remove((secondRoomY,secondRoomX)); 
                                Console.WriteLine($"SpawnedRooms Count: {spawnedRooms.Count}"); 
                                foreach (var room in spawnedRooms) 
                                { 
                                    Console.WriteLine($"Room: ({room.Item1}, {room.Item2})"); 
                                } 
 
                                connected = true; 
                            } 
                            break; 
                        } 
                    } 
                } 
 
                if (connected) 
                { 
                    break; 
                }    
            } 
 
            if (spawnedRooms.Count == 0) 
            { 
                connected = false; 
                getRooms(connectUnpairedRooms,out firstRoomX,out firstRoomY,out secondRoomX,out secondRoomY); 
                getCastedLines(firstRoomX,firstRoomY,secondRoomX,secondRoomY, out room1CastedLines, out room2CastedLines); 
                foreach((int,int) castedCoordinate in room1CastedLines) 
                { 
                    if (room2CastedLines.Count > 0 && !connected) 
                    { 
                        foreach((int,int) castedCoordinate2 in room2CastedLines) 
                        { 
                            if (castedCoordinate == castedCoordinate2) 
                            { 
                                if(!connected) 
                                { 
                                    Console.WriteLine("LAST CHECK"); 
                                    connectRooms(castedCoordinate,castedCoordinate2,firstRoomX,firstRoomY,secondRoomX,secondRoomY); 
                                    showDungeon(); 
                                    connectUnpairedRooms.Remove((firstRoomY,firstRoomX)); 
                                    connectUnpairedRooms.Remove((secondRoomY,secondRoomX)); 
                                    Console.WriteLine($"SpawnedRooms Count: {connectUnpairedRooms.Count}"); 
                                    foreach (var room in connectUnpairedRooms) 
                                    { 
                                        Console.WriteLine($"Room: ({room.Item1}, {room.Item2})"); 
                                    } 
 
                                    connected = true; 
                                } 
                                break; 
                            } 
                        } 
                    } 
                finished = true; 
            } 
 
        } 
         
 
         
        } 
    } 
private void getRooms(List<(int,int)> spawnedRooms,out int firstRoomX, out int firstRoomY, out int secondRoomX, out int secondRoomY)// gets two random starting rooms  
    { 
            // Console.WriteLine("Getting rooms"); 
            int chooseRooms = Globals.random.Next(0,spawnedRooms.Count); 
            (firstRoomY,firstRoomX) = spawnedRooms[chooseRooms]; 
            int temp = Globals.random.Next(0,spawnedRooms.Count); 
            if (spawnedRooms.Count>1) 
            { 
                while (temp == chooseRooms) 
                { 
                    temp = Globals.random.Next(0,spawnedRooms.Count); 
                } 
                chooseRooms = temp; 
                (secondRoomY,secondRoomX) = spawnedRooms[chooseRooms]; 
            } 
            else 
            { 
                secondRoomX = -1; 
                secondRoomY = -1; 
            } 
    } 
    private void getCastedLines(int firstRoomX, int firstRoomY, int secondRoomX, int secondRoomY,out List<(int,int)> room1CastedLines,out List<(int,int)> room2CastedLines) // casts 2 lins from 2 rooms to get coordinates of where they meet 
    { 
        // Console.WriteLine("casting lines"); 
        room1CastedLines = new List<(int, int)>(); 
        room2CastedLines= new List<(int, int)>(); 
        for (int castLine = 1; castLine < 4; castLine++) 
            { 
                if(firstRoomX+castLine < 4) 
                { 
                    room1CastedLines.Add((firstRoomY,firstRoomX+castLine)); 
                } 
                if(firstRoomX-castLine >= 0) 
                { 
                    room1CastedLines.Add((firstRoomY,firstRoomX-castLine)); 
                } 
                if(firstRoomY+castLine < 4) 
                { 
                    room1CastedLines.Add((firstRoomY+castLine,firstRoomX)); 
                } 
if(firstRoomY-castLine >= 0) 
                { 
                    room1CastedLines.Add((firstRoomY-castLine,firstRoomX)); 
                } 
 
                if(secondRoomX+castLine < 4) 
                { 
                    room2CastedLines.Add((secondRoomY,secondRoomX+castLine)); 
                } 
                if(secondRoomX-castLine >= 0) 
                { 
                    room2CastedLines.Add((secondRoomY,secondRoomX-castLine)); 
                } 
                if(firstRoomY+castLine < 4) 
                { 
                    room2CastedLines.Add((secondRoomY+castLine,secondRoomX)); 
                } 
                if(firstRoomY-castLine >= 0) 
                { 
                    room2CastedLines.Add((secondRoomY-castLine,secondRoomX)); 
                } 
 
                 
            } 
    } 
    private void connectRooms((int,int) castedCoordinate , (int,int) castedCoordinate2, int firstRoomX, int firstRoomY, int secondRoomX, int secondRoomY) // places rooms between two rooms to create a "corridor" like structure until it meets the coordinates recieved from getCasedLines 
    { 
        Console.WriteLine("connecting rooms"); 
        int temp1X,temp1Y,temp2X,temp2Y; 
        (temp1Y,temp1X) = castedCoordinate; 
        (temp2Y,temp2X) = castedCoordinate2; 
        while (firstRoomX < temp1X) 
        { 
            firstRoomX++; 
            dungeon[firstRoomY,firstRoomX] = "#"; 
        } 
        while (firstRoomX > temp1X) 
        { 
            firstRoomX--; 
            dungeon[firstRoomY,firstRoomX] = "#"; 
        }
         while (firstRoomY < temp1Y) 
        { 
            firstRoomY++; 
            dungeon[firstRoomY,firstRoomX] = "#"; 
        } 
        while (firstRoomY > temp1Y) 
        { 
            firstRoomY--; 
            dungeon[firstRoomY,firstRoomX] = "#"; 
        } 
 
         
        while (secondRoomX < temp2X) 
        { 
            secondRoomX++; 
            dungeon[secondRoomY,secondRoomX] = "#"; 
        } 
        while (secondRoomX > temp2X) 
        { 
            secondRoomX--; 
            dungeon[secondRoomY,secondRoomX] = "#"; 
        } 
 
        while (secondRoomY < temp2Y) 
        { 
            secondRoomY++; 
            dungeon[secondRoomY,secondRoomX] = "#"; 
        } 
        while (secondRoomY > temp2Y) 
        { 
            secondRoomY--; 
            dungeon[secondRoomY,secondRoomX] = "#"; 
        } 
         
 
    } 
    public void showDungeon() // made by AI remember to delete later its just for debugging 
    { 
        for (int row = 0; row < dungeon.GetLength(0); row++) 
        { 
            for (int column = 0; column < dungeon.GetLength(1); column++) 
            { 
                Console.Write(dungeon[row, column] + "        "); 
            }
            Console.WriteLine(); 
            Console.Write(row + ",0      " + row + ",1      " + row + ",2      " + row + ",3      "); 
            Console.WriteLine(); 
        } 
    } 
    public bool isNextAreaARoom(int playerDungeonPosY,int playerDungeonPosX) // checks if the next room the user is trying enter is a room returning true/false 
    { 
        Console.WriteLine("PLAYER POS Y : " + playerDungeonPosY + "\nPLAYER POS X: " +playerDungeonPosX); 
        if(dungeon[playerDungeonPosY,playerDungeonPosX] == "#" ||dungeon[playerDungeonPosY,playerDungeonPosX] == "C") 
        { 
            Console.WriteLine("IT IS RETURNING TRUE"); 
            isBossRoom = false; 
            return true; 
        } 
        if(dungeon[playerDungeonPosY,playerDungeonPosX] == "B" || dungeon[playerDungeonPosY,playerDungeonPosX] == "CB") 
        { 
            Console.WriteLine("YOU ARE IN THE BOSS ROOM"); 
            createPortal(); 
            isBossRoom = true; 
            return true; 
        } 
        else 
        { 
            Console.WriteLine("IT IS RETURNING FALSE"); 
            return false; 
        } 
         
    } 
    public (int,int) getPlayerStartingPos() // gets a random starting location which is one of the first points which are placed down 
    { 
 
        return playerSpawns[Globals.random.Next(0,playerSpawns.Count)]; 
    } 
    private void createBossRoom(List<(int, int)> playerSpawns) // replaces a random room which was placed first as a Boss room 
    { 
        int bossLocationX,bossLocationY;
         (bossLocationY,bossLocationX) = playerSpawns[Globals.random.Next(0,playerSpawns.Count)]; 
        dungeon[bossLocationY,bossLocationX] = "B"; 
        Console.WriteLine("Location of boss room is " + bossLocationY + " " + bossLocationX); 
        playerSpawns.Remove((bossLocationY,bossLocationX)); 
    } 
    public bool isRoomCleared(int playerDungeonPosY,int playerDungeonPosX) // checks if the current has been cleared returning true/false 
    { 
        string roomState = dungeon[playerDungeonPosY,playerDungeonPosX]; 
        if (roomState == "#") 
        { 
            dungeon[playerDungeonPosY,playerDungeonPosX] = "C"; 
            return false; 
        } 
        if(roomState == "B") 
        { 
            dungeon[playerDungeonPosY,playerDungeonPosX] = "CB"; 
            return false; 
        } 
        if (roomState == "C") 
        { 
            return true; 
        } 
        return false; 
    } 
    public Rectangle portalBounds() // gets the hitbox for the portal 
    { 
        return new Rectangle(Globals._graphics.PreferredBackBufferWidth/2 - 25,Globals._graphics.PreferredBackBufferHeight/2 + 25,50,50); 
    } 
    public bool isFloorFinished() // checks if the user has touched the portal returning true/false 
    { 
        if (storePortal.Count >0 && Player.enemies.Count == 0) 
        { 
            if (portalBounds().Intersects(Player.playerBounds()) || portalBounds().Contains(Player.playerBounds())) 
            { 
                storePortal.RemoveAt(0); 
                return true; 
            } 
        }
         return false; 
    } 
    public void createPortal() // creates a portal and places it inside a list 
    { 
        if (storePortal != null) 
        { 
            if (storePortal.Count == 0 && Player.enemies.Count == 0) 
            { 
                portal = new Sprite2d("portalExit",new Vector2(Globals._graphics.PreferredBackBufferWidth/2,Globals._graphics.PreferredBackBufferHeight/2), new Vector2(50,50)); 
                storePortal.Add(portal); 
            } 
            else 
            { 
                Console.WriteLine("this cde shouldnt be running"); 
            } 
        } 
    } 
    public virtual void Draw() 
    { 
        if (isBossRoom && bossRoomNotification < 1.1) 
        { 
            Console.WriteLine("PLAYER HAS BEEN NOTFIED OF BOSS ROOM"); 
            font.Draw(); 
            bossRoomNotification +=  (float)(Globals.gameTime.ElapsedGameTime.TotalSeconds); 
        } 
        if (isBossRoom && Player.enemies.Count == 0) 
        { 
            foreach (var portal in storePortal) 
            { 
                portal.Draw(); 
            } 
        } 
    }  
} 
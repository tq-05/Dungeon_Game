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

public class Player : Characters 
{ 
    #region  variables 
    private float finalSpeed, dashCD, swingCD, shootCD, attackCD,dashLength,playerSpeed, dashBuff, speedBuff, diagonalMovement,vulnerableDuration,vulnerableTime,restingTime,restCounter; 
    public static Vector2 playerDim, playerPos, attackSize; 
    private int totalEnemies,damage ,numberOfAttacksLeft, dungeonPosY, dungeonPosX; 
    public static int playerHP; 
    private myFile file; 
    private myMouse mouse; 
    private Enemy enemy; 
    private Font2d font; 
    public static List<Enemy> enemies; 
    public static List<Slice> playerSlices; 
    public static List<Bullet> playerAmmo; 
    private List<Font2d> text; 
    private bool isDashOn; 
    private string weapon, x; 
    private bool isFirstRoom, canLeaveRoom; 
    public static bool isResting,isRanged,isMelee; 
    private dungeonGenerator dungeon; 
    // x is a placeholder so i can retrieve the file data  
    #endregion 
 
     
    public Player(string Model, Vector2 POS , Vector2 DIM, myMouse MOUSE, dungeonGenerator DUNGEON) : base("Player",new Vector2(Globals._graphics.PreferredBackBufferWidth / 2, Globals._graphics.PreferredBackBufferHeight / 2), new Vector2 (200,200)) 
    { 
        loadModel(Model);
        Console.WriteLine(Globals.selectedFileNumber + " - this is the filenumber"); 
        file = new myFile(); 
        dungeon = DUNGEON; 
        playerDim = DIM; 
        mouse = MOUSE; 
        playerHP = healthPoints; 
        playerPos = pos; 
        playerSpeed = speed; 
        playerAmmo = new List<Bullet>(); 
        playerSlices = new List<Slice>(); 
        enemies = new List<Enemy>(); 
        isFirstRoom = true;
         isResting = false; 
        canLeaveRoom = true; 
        totalEnemies = 0; 
        dungeon.createDungeon(); 
        (dungeonPosY,dungeonPosX) = dungeon.getPlayerStartingPos(); 
        Console.WriteLine("The starting position for the user is " + dungeonPosY +" " + dungeonPosX); 
    } 
    public override void Update() 
    { 
        if(playerHP > 0) 
        { 
            KeyboardState kstate = Keyboard.GetState(); 
            if (!isResting) 
            { 
                checkWeapon(); 
            } 
            else if (numberOfAttacksLeft == 0) 
            { 
                Console.WriteLine("YOU ARE CURRENTLY VULNERABLE TO ATTACKS"); 
                vulnerableDuration += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
                if(vulnerableDuration > vulnerableTime) 
                { 
                    vulnerableDuration = 0; 
                    isResting = false; 
                } 
            } 
            movement(kstate); 
            dungeonNavigation(); 
            if (isRanged) 
            { 
            shootBullet(kstate); 
            } 
            if (isMelee) 
            { 
                swingSlice(kstate); 
            } 
            if(enemies.Count > 0) 
            { 
                for(int enemyIndex=enemies.Count-1;enemyIndex>= 0;enemyIndex--) 
                { 
                    enemies[enemyIndex].Update(); 
                    if(checkIfEnemyHasDied()) 
                    { 
                        continue; 
                    } 
                } 
            } 
            if (enemies.Count == 0) 
            { 
                canLeaveRoom = true; 
            } 
        } 
        else 
        { 
            enemies = new List<Enemy>(); 
            playerAmmo = new List<Bullet>(); 
            playerSlices = new List<Slice>(); 
        } 
         
    } 
     
    public override void Draw() 
    { 
        // draws player 
        if (texture != null) 
        {  
            Globals._spriteBatch.Draw(texture, new Rectangle( (int)(playerPos.X), (int)(playerPos.Y),(int)(playerDim.X),(int)(playerDim.Y)),null,Color.White, 0.0f, new Vector2(texture.Bounds.Width/2,texture.Bounds.Height/2), new SpriteEffects() , 0); 
        } 
        drawAttack(); 
        drawEnemies(); 
        showAttackStatus(); 
    } 
    public void checkWeapon() // checks what kind of weapon the user has 
    { 
        (weapon,x) = file.getFileData(Globals.selectedFileNumber); 
        switch (weapon) 
        { 
            case "Sword": 
                isMelee = true; 
                isRanged = false; 
                damage = 1; 
                vulnerableTime = 4; 
                numberOfAttacksLeft = 10; 
                attackCD = 0.4f; 
                restingTime = 3; 
                attackSize =  new Vector2(300,100); 
                break; 
            case "Gsword": 
                isMelee = true; 
                isRanged = false; 
                damage = 3; 
                vulnerableTime = 12; 
                restingTime = 2; 
                numberOfAttacksLeft = 6; 
                attackSize =  new Vector2(600,200); 
                attackCD = 0.65f; 
                break; 
            case "Rifle": 
                isRanged = true; 
                isMelee = false; 
                attackCD = 0.15f; 
                vulnerableTime = 10; 
                restingTime = 5; 
                damage = 2; 
                numberOfAttacksLeft = 30; 
                attackSize = new Vector2(25,50); 
                break; 
            case "Gun": 
                isRanged = true; 
                isMelee = false; 
                attackCD = 0.25f; 
                vulnerableTime = 6; 
                restingTime = 4; 
                damage = 4; 
                numberOfAttacksLeft = 12; 
                attackSize = new Vector2(50,75); 
                break; 
             
            default: 
            file.writeToFile("-","-"); 
            break; 
        } 
        isResting = true; 
    } 
    public void showAttackStatus() 
    { 
        text = new List<Font2d>(); 
        if ((restingTime - restCounter) > 0 && numberOfAttacksLeft != 0) 
        {
            font = new Font2d("statusFont", new Vector2((float)(playerPos.X - playerDim.X/2),(float)(playerPos.Y - playerDim.Y*1.25)), "You are currently resting \nTime unti rest completed: " + (restingTime - restCounter)); 
            text.Add(font); 
        } 
        else if ((restingTime - restCounter) <= 0 && numberOfAttacksLeft > 0) 
        { 
            font = new Font2d("statusFont", new Vector2((float)(playerPos.X - playerDim.X/2),(float)(playerPos.Y - playerDim.Y*1.25)), "You are in perfect shape! \n(number of attacks is MAX)"); 
            text.Add(font); 
        } 
        else if (numberOfAttacksLeft == 0) 
        { 
            font = new Font2d("statusFont", new Vector2((float)(playerPos.X - playerDim.X/2),(float)(playerPos.Y - playerDim.Y*1.25)), "You are currently vulnerable \nYou have " + (vulnerableTime - vulnerableDuration) + " seconds left!"); 
            text.Add(font); 
        } 
        font = new Font2d("statusFont", new Vector2((float)(playerPos.X - playerDim.X/2),(float)(playerPos.Y - playerDim.Y*1.4)), "You have " + numberOfAttacksLeft + " number of attack(s) left"); 
        text.Add(font); 
        foreach(var sentence in text) 
        { 
            sentence.Draw(); 
        } 
 
    } 
    public void movement(KeyboardState kstate) // checks input for the player to move up/down/left/right 
    { 
        speedBuff = 1f; 
        diagonalMovement = 1f; 
        float realTimePlayerSpeed = playerSpeed * (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
        // ensures the player moves smoothly, instead of choppy so it isnt based off framerate which is different for everyone 
         
        // dash cd for the dash 
        if (isDashOn == true) 
        { 
            dashLength += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
            if (dashLength > 0.1)
             { 
                isDashOn = false; 
                dashBuff = 1f; 
                dashLength = 0; 
            } 
        } 
        else 
        { 
            dashBuff = 1f; 
        } 
        // speed buffs are applied 
 
        if (isDashOn == false) 
        { 
            dashCD += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
        } 
         
         
        if (kstate.IsKeyDown(Keys.Q) && dashCD>2) 
        { 
            dashBuff= 4f; 
            dashCD = 0; 
            isDashOn = true; 
        } 
        if (kstate.IsKeyDown(Keys.W) && kstate.IsKeyDown(Keys.A)) 
        { 
            diagonalMovement = 0.59f; 
        } 
        if (kstate.IsKeyDown(Keys.W) && kstate.IsKeyDown(Keys.D)) 
        { 
            diagonalMovement = 0.59f; 
        } 
        if (kstate.IsKeyDown(Keys.S) && kstate.IsKeyDown(Keys.D)) 
        { 
            diagonalMovement = 0.59f; 
        } 
        if (kstate.IsKeyDown(Keys.S) && kstate.IsKeyDown(Keys.A)) 
        { 
            diagonalMovement = 0.59f; 
        } 
 
        finalSpeed = playerSpeed * realTimePlayerSpeed * dashBuff * speedBuff; 
        finalSpeed = finalSpeed * diagonalMovement; 
 
        if (kstate.IsKeyDown(Keys.W))
         { 
            playerPos =  new Vector2(playerPos.X , playerPos.Y - finalSpeed); 
        } 
        if (kstate.IsKeyDown(Keys.S)) 
        { 
            playerPos =  new Vector2(playerPos.X , playerPos.Y + finalSpeed); 
        } 
        if (kstate.IsKeyDown(Keys.A)) 
        { 
            playerPos =  new Vector2(playerPos.X - finalSpeed, playerPos.Y); 
        } 
        if (kstate.IsKeyDown(Keys.D)) 
        { 
            playerPos =  new Vector2(playerPos.X + finalSpeed, playerPos.Y); 
        } 
         
        // MOVEMENT SYSTEM 
    } 
    public void shootBullet(KeyboardState kstate) // checks input for shoot activation 
    { 
            shootCD += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
            if (kstate.IsKeyDown(Keys.Space) && shootCD > attackCD && numberOfAttacksLeft > 0 ) 
            { 
                Bullet bullet = Bullet.shoot("bullet", playerPos, attackSize, mouse.mousePos); 
                playerAmmo.Add(bullet); 
                numberOfAttacksLeft--; 
                shootCD=0; 
                restCounter = 0; 
                Console.WriteLine("You have used a Bullet \nYou have " +  numberOfAttacksLeft + " playerAmmo left!"); 
                //creates a list of "bullets" (objects which belong to the bullet class) and allows them to be drawn. 
            } 
            else if (numberOfAttacksLeft!=0) 
            { 
                //Console.WriteLine("you are currently resting"); 
                restCounter += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
                //Console.WriteLine("YOU currently have " + (restingTime restCounter) + " seconds left"); 
                if (restCounter > restingTime) 
                {
                     //Console.WriteLine("You have successfully rested!"); 
                    isResting = false; 
                } 
            } 
 
            foreach (var bullet in  playerAmmo) 
            { 
                bullet.Update(); 
                if(bullet.done) 
                { 
                    playerAmmo.Remove(bullet); 
                    return; 
                } 
            } 
    } 
    public void swingSlice(KeyboardState kstate) // checks input for swing activation 
    { 
        swingCD += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
        if (kstate.IsKeyDown(Keys.E) && swingCD > attackCD && numberOfAttacksLeft > 0 ) 
        { 
            Slice slice = Slice.swing("Slice", playerPos, attackSize, mouse.mousePos); 
            playerSlices.Add(slice); 
            numberOfAttacksLeft--; 
            swingCD=0; 
            restCounter = 0; 
            Console.WriteLine("You have used a Slice \nYou have " + numberOfAttacksLeft + " playerSlices left!"); 
            //creates a list of "playerSlices" (objects which belong to the Slice class) and allows them to be drawn. 
        } 
        else if (numberOfAttacksLeft!=0) 
        { 
            //Console.WriteLine("you are currently resting"); 
            restCounter += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
            //Console.WriteLine("YOU currently have " + (restingTime-restCounter) + " seconds left"); 
            if (restCounter > restingTime) 
            { 
                //Console.WriteLine("You have successfully rested!"); 
                isResting = false; 
            } 
        } 
        foreach (var slice in  playerSlices) 
        { 
            slice.Update(); 
            if(slice.done) 
            { 
                playerSlices.Remove(slice); 
                return; 
            } 
        } 
    } 
    public void drawAttack() // draws the attack sprites 
    { 
        if (playerAmmo.Count > 0 ) 
        { 
            foreach (var bullet in  playerAmmo) 
            { 
                bullet.Draw(); 
            } 
        } 
         
        // draws the player, and also connects the variables playerPos and playerDim with the sprite which is drawn. 
        if (playerSlices.Count >0) 
        { 
            foreach (var slice in  playerSlices) 
            { 
                slice.Draw(); 
            } 
        } 
    } 
    public void spawnEnemies() // spawns in enemies in the current room  
    { 
        enemies = new List<Enemy>(); 
        int numberOfEnemiesToBeSpawned = getNumberOfEnemies(); 
        Vector2 enemyDim = new Vector2 (100,100); 
        float posX,posY; 
        string enemyType; 
        for (int numberSpawned=0; numberSpawned != numberOfEnemiesToBeSpawned; numberSpawned++) 
        { 
            (posX,posY) = getRandomLocationOfEnemies(enemyDim); 
            enemyType = getTypesOfEnemies(); 
            enemy = new Enemy(enemyType,new Vector2(posX,posY), new Vector2(enemyDim.X,enemyDim.Y), totalEnemies); 
            enemies.Add(enemy); 
            totalEnemies++; 
        } 
    } 
    public int getNumberOfEnemies() // generates a random number from 3 to 9 to determine the amount of enemies to be spawned 
    { 
        return Globals.random.Next(3,10);  
    } 
    public string getTypesOfEnemies() // generates a random number to determine the chances of the enmy being a ranged or melee enemy 
    { 
        int chance = Globals.random.Next(0,2); 
        if (chance == 1) 
        { 
            return "rangedEnemy"; 
        } 
        if (chance == 0) 
        { 
            return "meleeEnemy"; 
        } 
        return "enemy"; 
    } 
    public (float,float) getRandomLocationOfEnemies(Vector2 enemyDim) // gets random vector2 coordinates to spawn enemies 
    { 
        int offSetX,offSetY,offSetX2,offSetY2; 
        offSetX = (int)(Globals._graphics.PreferredBackBufferWidth*0.8); 
        offSetY = (int)(Globals._graphics.PreferredBackBufferHeight*0.8); 
        offSetX2 = (int)(Globals._graphics.PreferredBackBufferWidth*0.2); 
        offSetY2= (int)(Globals._graphics.PreferredBackBufferHeight*0.2); 
        float enemyPosX = (float)Globals.random.Next(offSetX2,offSetX); 
        float enemyPosY = (float)Globals.random.Next(offSetY2,offSetY); 
        Vector2 enemyPos = new Vector2(enemyPosX,enemyPosY); 
        Rectangle enemyHitBox = new Rectangle((int)(enemyPosX - enemyDim.X / 2), (int)(enemyPosY - enemyDim.Y / 2),(int)(enemyDim.X),(int)(enemyDim.Y)); 
        if (enemies.Count> 0) 
        { 
            foreach(var enemy in enemies) 
            { 
                while (enemy.enemyBounds().Intersects(enemyHitBox) || enemy.enemyBounds().Contains(enemyHitBox)) 
                { 
                    enemyPosX = (float)Globals.random.Next(offSetX2,offSetX); 
                    enemyPosY = (float)Globals.random.Next(offSetY2,offSetY);
                     enemyHitBox = new Rectangle((int)(enemyPosX - enemyDim.X / 2), (int)(enemyPosY - enemyDim.Y / 2),(int)(enemyDim.X),(int)(enemyDim.Y)); 
                } 
            } 
        } 
        return (enemyPosX,enemyPosY); 
    } 
    public void drawEnemies() // draws the enemy sprites 
    { 
        if (enemies.Count>0) 
        { 
            foreach(var enemy in enemies) 
            { 
                enemy.Draw(); 
            } 
        } 
         
    } 
    public bool checkIfEnemyHasDied() // hitbox for the enemies, checks if they have been damaged 
    { 
        if(enemies.Count>0) 
        { 
            foreach(var enemy in enemies) 
            { 
                if (playerAmmo.Count > 0) 
                { 
                    foreach(var bullet in playerAmmo) 
                    { 
                        if (enemy.enemyBounds().Intersects(bullet.bulletBounds()) || enemy.enemyBounds().Contains(bullet.bulletBounds())) 
                        { 
                            enemy.healthPoints -= damage; 
                            if (enemy.healthPoints <= 0) 
                            { 
                                playerAmmo.Remove(bullet); 
                                enemies.Remove(enemy); 
                                Globals.totalKills++; 
                                return true; 
                            } 
                            playerAmmo.Remove(bullet); 
                            return false; 
                        } 
                         
                    }
                    } 
                else if (playerSlices.Count > 0) 
                { 
                    foreach(var slice in playerSlices) 
                    { 
                        if (enemy.enemyBounds().Intersects(slice.sliceBounds()) || enemy.enemyBounds().Contains(slice.sliceBounds())) 
                        { 
                            enemy.healthPoints -= damage; 
                            if (enemy.healthPoints <= 0) 
                            { 
                                enemies.Remove(enemy); 
                                Globals.totalKills++; 
                                return true; 
                            } 
                        } 
                         
                    } 
                } 
            } 
        } 
        return true; 
    } 
    public void dungeonNavigation() // calls different functions to navigate the dungeon with each room being the size of the player's screen 
    { 
        if(!canLeaveRoom) 
        { 
            lockedIn(); 
            Globals.totalTime += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
        } 
        else if (!isFirstRoom) 
        { 
            Globals.totalTime += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
            navigateRight(); 
            navigateLeft(); 
            navigateDown(); 
            navigateUp(); 
        } 
        else 
        { 
            firstRoom(); 
        }
        } 
    public void navigateLeft() // checks if the user has touched the left side of the screen and checks if there is an available room in the dungeon array 
    { 
         if (playerPos.X <  playerDim.X/ 2) // left of screen 
            { 
                Console.WriteLine("attempted going left"); 
                if (dungeonPosX - 1 >= 0) 
                { 
                    if (dungeon.isNextAreaARoom(dungeonPosY,dungeonPosX - 1)) 
                    { 
                        Console.WriteLine("player has entered a left room"); 
                        playerPos.X = Globals._graphics.PreferredBackBufferWidth - playerDim.X/2; 
                        dungeonPosX -= 1; 
                        playerAmmo = new List<Bullet>(); 
                        playerSlices = new List<Slice>(); 
                        if(!dungeon.isRoomCleared(dungeonPosY,dungeonPosX)) 
                        { 
                            Console.WriteLine("SPAWNING ENEMIES"); 
                            spawnEnemies(); 
                            canLeaveRoom = false; 
                        } 
                    } 
                     
                    else 
                    {    
                        playerPos.X = playerDim.X / 2; 
                    } 
                } 
                else 
                {    
                    playerPos.X = playerDim.X / 2; 
                } 
            } 
    } 
    public void navigateRight()// checks if the user has touched the right side of the screen and checks if there is an available room in the dungeon array 
    { 
        if (playerPos.X > Globals._graphics.PreferredBackBufferWidth - playerDim.X / 2) // right of screen 
            { 
                if (dungeonPosX + 1 <= 3)
                 { 
                    Console.WriteLine("attempted going right"); 
                    if (dungeon.isNextAreaARoom(dungeonPosY,dungeonPosX + 1)) 
                    { 
                        Console.WriteLine("player has entered a right room"); 
                        playerPos.X = playerDim.X/2; 
                        dungeonPosX+= 1; 
                        playerAmmo = new List<Bullet>(); 
                        playerSlices = new List<Slice>(); 
                        if(!dungeon.isRoomCleared(dungeonPosY,dungeonPosX)) 
                        { 
                            Console.WriteLine("SPAWNING ENEMIES"); 
                            spawnEnemies(); 
                            canLeaveRoom = false; 
                        } 
                         
                    } 
                    else 
                    { 
                        playerPos.X = Globals._graphics.PreferredBackBufferWidth - playerDim.X / 2; 
                    } 
                } 
                else 
                { 
                    playerPos.X = Globals._graphics.PreferredBackBufferWidth - playerDim.X / 2; 
                } 
            } 
    } 
    public void navigateDown()// checks if the user has touched the bottom of the screen and checks if there is an available room in the dungeon array 
    { 
        if (playerPos.Y> Globals._graphics.PreferredBackBufferHeight - playerDim.Y / 2)// BOTTOM of screen 
            { 
                Console.WriteLine("atttempted going down"); 
                 if (dungeonPosY + 1 <= 3) 
                { 
                    if (dungeon.isNextAreaARoom(dungeonPosY + 1,dungeonPosX)) 
                    { 
                        playerPos.Y = playerDim.Y / 2; 
                        dungeonPosY+= 1; 
                        playerAmmo = new List<Bullet>(); 
                        playerSlices = new List<Slice>();
                        if(!dungeon.isRoomCleared(dungeonPosY,dungeonPosX)) 
                        { 
                            Console.WriteLine("SPAWNING ENEMIES"); 
                            spawnEnemies(); 
                            canLeaveRoom = false; 
                        } 
                    } 
                    else 
                    { 
                        playerPos.Y = Globals._graphics.PreferredBackBufferHeight - playerDim.Y / 2; 
                    }   
                } 
                else 
                { 
                    playerPos.Y = Globals._graphics.PreferredBackBufferHeight - playerDim.Y / 2; 
                }    
                 
            } 
    } 
    public void navigateUp()// checks if the user has touched the top of the screen and checks if there is an available room in the dungeon array 
    { 
        if (playerPos.Y < playerDim.Y / 2)// TOP of screen 
            { 
                Console.WriteLine("attempted going up"); 
                if (dungeonPosY - 1 >= 0) 
                { 
                    if (dungeon.isNextAreaARoom(dungeonPosY - 1,dungeonPosX)) 
                    { 
                        playerPos.Y = Globals._graphics.PreferredBackBufferHeight - playerDim.Y / 2; 
                        dungeonPosY-= 1; 
                        playerAmmo = new List<Bullet>(); 
                        playerSlices = new List<Slice>(); 
                        if(!dungeon.isRoomCleared(dungeonPosY,dungeonPosX)) 
                        { 
                            Console.WriteLine("SPAWNING ENEMIES"); 
                            spawnEnemies(); 
                            canLeaveRoom = false; 
                        } 
                    } 
                    else 
                    {   
                         playerPos.Y = playerDim.Y / 2; 
                    } 
                } 
                else 
                {    
                    playerPos.Y = playerDim.Y / 2; 
                } 
            } 
    } 
    public void firstRoom() // checks if the user touches anyside of the screen and puts the player in a random starting location which is not the boss room in the dungeon 
    { 
        if (playerPos.X > Globals._graphics.PreferredBackBufferWidth - playerDim.X / 2) // right of screen 
            { 
                playerPos.X = Globals._graphics.PreferredBackBufferWidth/2; 
                playerPos.Y = Globals._graphics.PreferredBackBufferHeight/2; 
                spawnEnemies();  
                canLeaveRoom = false; 
                isFirstRoom = false; 
                dungeon.dungeon[dungeonPosY,dungeonPosX] = "C"; 
            } 
            else if (playerPos.X <  playerDim.X/ 2) // left of screen 
            { 
                playerPos.X = Globals._graphics.PreferredBackBufferWidth/2; 
                playerPos.Y = Globals._graphics.PreferredBackBufferHeight/2; 
                spawnEnemies(); 
                canLeaveRoom = false; 
                isFirstRoom = false; 
                dungeon.dungeon[dungeonPosY,dungeonPosX] = "C"; 
            } 
 
            if (playerPos.Y> Globals._graphics.PreferredBackBufferHeight - playerDim.Y / 2) // top of screen 
            { 
                playerPos.X = Globals._graphics.PreferredBackBufferWidth/2; 
                playerPos.Y = Globals._graphics.PreferredBackBufferHeight/2; 
                spawnEnemies(); 
                canLeaveRoom = false; 
                isFirstRoom = false; 
                dungeon.dungeon[dungeonPosY,dungeonPosX] = "C"; 
            } 
            else if (playerPos.Y < playerDim.Y / 2) //bottom of screen 
            {
                 playerPos.X = Globals._graphics.PreferredBackBufferWidth/2; 
                playerPos.Y = Globals._graphics.PreferredBackBufferHeight/2; 
                spawnEnemies(); 
                canLeaveRoom = false; 
                isFirstRoom = false; 
                dungeon.dungeon[dungeonPosY,dungeonPosX] = "C"; 
            }   
    } 
    public void lockedIn() // prevents the player from "navigating" through the dungeon if there are enemies in the current room 
    { 
         if (playerPos.X > Globals._graphics.PreferredBackBufferWidth - playerDim.X / 2) 
         { 
            playerPos.X = Globals._graphics.PreferredBackBufferWidth - playerDim.X / 2; 
         } 
         else if (playerPos.X <  playerDim.X/ 2) 
         { 
            playerPos.X = playerDim.X / 2; 
         } 
         if (playerPos.Y> Globals._graphics.PreferredBackBufferHeight - playerDim.Y / 2) 
         { 
            playerPos.Y = Globals._graphics.PreferredBackBufferHeight - playerDim.Y / 2; 
         } 
         else if (playerPos.Y < playerDim.Y / 2) 
         { 
            playerPos.Y = playerDim.Y / 2; 
         } 
    }     
    public static Rectangle playerBounds() // gets the player's hitbox 
    { 
        return new Rectangle((int)(playerPos.X - playerDim.X/2),(int)(playerPos.Y+playerDim.Y/2),(int)playerDim.X,(int)playerDim.Y); 
    } 
    public static bool isPlayerVulnerable() 
    { 
        if(!isResting) 
        { 
            return true; 
        } 
        else
         { 
            return true; 
        }  
    } 
    public static bool checkIfPlayerIsAlive() 
    { 
        if (playerHP <= 0) 
        { 
            return false; 
        } 
        return true; 
    } 
} 
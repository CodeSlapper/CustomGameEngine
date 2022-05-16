using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.Drawing;
using System;

// NEW for Audio
using System.IO;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class GameScene : Scene
    {
        public static float dt = 0;
        EntityManager entityManager;
        SystemManager systemManager;
        //added collision manager
        MazeCollisionManager mazeCollsion;
        
        int score1;


        public Camera camera;

        public static GameScene gameInstance;

        bool[] keysPressed = new bool[255];
       
        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            mazeCollsion = new MazeCollisionManager();
            //this reffereing to gamescene since we are inside of this class 
            mazeCollsion.MazeReference(this);
            mazeCollsion.Entity_GameScene_Ref(entityManager);

            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate += Keyboard_KeyUp;
            // Enable Depth Testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Set Camera
            camera = new Camera(new Vector3(0, 1, -5), new Vector3(0 ,1, 0), (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);
            
            CreateEntities();
            CreateSystems();

            // TODO: Add your initialization logic here

        }
        /*Creat entities contains an insane amount of for loops to add all pieces of the map, 
         * points and anything extra. an attemp was made to create nested loops of adding entites, 
         * the project never loads or outright crashes*/
        private void CreateEntities()
        {
            Entity newEntity;


            //-------------------------------------------END OF POINT AND WALL ENTITIES------------------------------
            newEntity = new Entity("PacManStage");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 20.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Walls/PacmanStage.obj"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Floor");
            newEntity.AddComponent(new ComponentPosition(0.0f, -0.1f, 0.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Walls/Floor.obj"));
            newEntity.AddComponent(new ComponentAudio("Audio/buzz.wav"));
            entityManager.AddEntity(newEntity);

            for(int i=0; i<3; i++)
            {
                newEntity = new Entity("T_Wall"+i);
                newEntity.AddComponent(new ComponentPosition(-30.0f+(30*i), 1.0f, -20.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Walls/T_wall.obj"));
                entityManager.AddEntity(newEntity);
            }
            for (int i = 0; i < 2; i++)
            {
                newEntity = new Entity("Reverse_T_Wall"+i);
                newEntity.AddComponent(new ComponentPosition(-15.0f+(30*i), 1.0f, 0.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Walls/Reverse_T_Wall.obj"));
                entityManager.AddEntity(newEntity);
                
            }
            //-------------other side of the stage-------------
            for (int j = 3; j < 6; j++)
            {
                newEntity = new Entity("Reverse_T_Wall" + j);
                newEntity.AddComponent(new ComponentPosition(120.0f+(-30*j), 1.0f, 65.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Walls/Reverse_T_Wall.obj"));
                entityManager.AddEntity(newEntity);
            }
            for (int i = 3; i < 5; i++)
            {
                newEntity = new Entity("T_Wall" + i);
                newEntity.AddComponent(new ComponentPosition(-105 + (30 * i), 1.0f, 45.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Walls/T_wall.obj"));
                entityManager.AddEntity(newEntity);
            }
            //--------------------------------------------
            newEntity = new Entity("Ghost_Room");
            newEntity.AddComponent(new ComponentPosition(0.0f, 1.0f, 20.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Walls/GhostRoom.obj"));
            entityManager.AddEntity(newEntity);
            for (int i=0; i<2; i++)
            {
                newEntity = new Entity("LongWall"+i);
                newEntity.AddComponent(new ComponentPosition(38.0f+(-75*i), 1.0f, 20.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Walls/LongWall.obj"));
                entityManager.AddEntity(newEntity);
            }
            for (int i = 0; i < 2; i++)
            {
                newEntity = new Entity("ShortWall" + i);
                newEntity.AddComponent(new ComponentPosition(30.0f + (-60 * i), 1.0f, 20.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Walls/ShortWall.obj"));
                entityManager.AddEntity(newEntity);
            }
            //------------point spheres----------------
            for (int i = 0; i < 18; i++)
            {
                newEntity = new Entity("Point"+i);
                newEntity.AddComponent(new ComponentPosition(27.0f, 1.0f, -19.0f+(5*i)));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
            }
            for (int i = 18; i < 36; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(-27.0f, 1.0f, -110.0f + (5 * i)));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
            }
            int mult = 0;
            for (int i = 36; i < 46; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(-22.0f+(5* mult), 1.0f, 0.0f ));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult++;
            }
            int mult2 = 0;
            for (int i = 46; i < 56; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(-22.0f + (5 * mult2), 1.0f, 40.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult2++;
            }
            int mult3 = 0;
            for (int i = 56; i < 76; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(-43.0f , 1.0f, -27.0f + (5 * mult3)));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult3++;
            }
            int mult4 = 0;
            for (int i = 76; i < 94; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(-35.0f, 1.0f, -27.0f + (5 * mult4)));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult4++;
            }
            int mult5 = 0;
            for (int i = 94; i < 114; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(42.0f, 1.0f, -27.0f + (5 * mult5)));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult5++;
            }
            int mult6 = 0;
            for (int i = 114; i < 132; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(35.0f, 1.0f, -27.0f + (5 * mult6)));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult6++;
            }
            int mult7 = 0;
            for (int i = 132; i < 145; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(-30.0f + (5 * mult7), 1.0f, -27.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult7++;
            }
            int mult8 = 0;
            for (int i = 145; i < 162; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(-38.0f + (5 * mult8), 1.0f, 67.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult8++;
            }
            int mult9 = 0;
            for (int i = 162; i < 167; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(4.0f + (5 * mult9), 1.0f, -20.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult9++;
            }
            int mult10 = 0;
            for (int i = 167; i < 172; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(-20.0f + (5 * mult10), 1.0f, -20.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult10++;
            }
            int mult11 = 0;
            for (int i = 172; i < 176; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(-10.0f , 1.0f, -20.0f + (5 * mult11)));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult11++;
            }
            int mult12 = 0;
            for (int i = 176; i < 179; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(10.0f, 1.0f, -17.0f + (5 * mult12)));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult12++;
            }
            int mult13 = 0;
            for (int i = 179; i < 182; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(10.0f, 1.0f, 50.0f + (5 * mult13)));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult13++;
            }
            int mult14 = 0;
            for (int i = 182; i < 185; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(-10.0f, 1.0f, 50.0f + (5 * mult14)));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult14++;
            }
            int mult15 = 0;
            for (int i = 185; i < 190; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(-9.0f + (5 * mult15), 1.0f, 47.0f ));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult15++;
            }
            int mult16 = 0;
            for (int i = 190; i < 193; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(-20.0f + (5 * mult16), 1.0f, 60.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult16++;
            }
            int mult17 = 0;
            for (int i = 193; i < 196; i++)
            {
                newEntity = new Entity("Point" + i);
                newEntity.AddComponent(new ComponentPosition(10.0f + (5 * mult17), 1.0f, 60.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/PointSphere.obj"));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.8f));
                entityManager.AddEntity(newEntity);
                mult17++;
            }
            //-------------------------------------------END OF POINT AND WALL ENTITIES------------------------------
            //due to collisionBox not working I have filled the entities with small and numerous collision spheres
            //--------------------------------SPHERICAL COLLISION FOR WALLS--------------------------------------
            int WalMult1 = 0;
            for (int i = 0; i < 28; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(30.0f , 1.0f, 0.0f + ((1.4f) * WalMult1)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult1++;
            }
            int WalMult2 = 0;
            for (int i = 28; i < 56; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-30.0f, 1.0f, 0.0f + ((1.4f) * WalMult2)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult2++;
            }
            int WalMult3 = 0;
            for (int i = 56; i < 94; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-37.5f, 1.0f, -16.0f + ((1.9f) * WalMult3)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult3++;
            }
            int WalMult4 = 0;
            for (int i = 94; i < 132; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(38.5f, 1.0f, -14.0f + ((1.8f) * WalMult4)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult4++;
            }
            //outer left wall
            int WalMult5 = 0;
            for (int i = 132; i < 182; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(45.5f, 1.0f, -28.0f + ((2.0f) * WalMult5)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult5++;
            }
            //outer right wall
            int WalMult6 = 0;
            for (int i = 182; i < 232; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-45.5f, 1.0f, -28.0f + ((2.0f) * WalMult6)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult6++;
            }
            int WalMult7 = 0;
            for (int i = 232; i < 262; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-45.5f + ((3.1f) * WalMult7), 1.0f, -30.0f ));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult7++;
            }
            int WalMult8 = 0;
            for (int i = 262; i < 292; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-45.5f + ((3.1f) * WalMult8), 1.0f, 71.0f));

                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult8++;
            }
            //T_shape Walls collisions (wide section)
            int WalMult9 = 0;
            for (int i = 292; i < 302; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-9.0f + ((2.0f) * WalMult9), 1.0f, -22.0f));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult9++;
            }
            int WalMult10 = 0;
            for (int i = 302; i < 312; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-60.0f + ((2.0f) * WalMult9), 1.0f, -22.0f));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult9++;
            }
            int WalMult11 = 0;
            for (int i = 312; i < 322; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(22.0f + ((2.0f) * WalMult11), 1.0f, -22.0f));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult11++;
            }
            int WalMult12 = 0;
            for (int i = 322; i < 332; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(6.5f + ((2.0f) * WalMult12), 1.0f, -2.0f));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult12++;
            }
            int WalMult13 = 0;
            for (int i = 332; i < 342; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-25.0f + ((2.0f) * WalMult13), 1.0f, -2.0f));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult13++;
            }
            int WalMult14 = 0;
            for (int i = 342; i < 352; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-25.0f + ((2.0f) * WalMult14), 1.0f, 43.0f));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult14++;
            }
            int WalMult15 = 0;
            for (int i = 352; i < 362; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(6.5f + ((2.0f) * WalMult15), 1.0f, 43.0f));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult15++;
            }
            int WalMult16 = 0;
            for (int i = 362; i < 372; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-9.5f + ((2.0f) * WalMult16), 1.0f, 63.5f));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult16++;
            }
            int WalMult17 = 0;
            for (int i = 372; i < 382; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-39.5f + ((2.0f) * WalMult17), 1.0f, 63.5f));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult17++;
            }
            int WalMult18 = 0;
            for (int i = 382; i < 392; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(22.5f + ((2.0f) * WalMult18), 1.0f, 63.5f));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult18++;
            }
            //"noses of T walls"
            int WalMult19 = 0;
            for (int i = 392; i < 399; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(30.0f , 1.0f, 50.5f + ((2.0f) * WalMult19)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult19++;
            }
            int WalMult20 = 0;
            for (int i = 399; i < 406; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(0.0f, 1.0f, 50.5f + ((2.0f) * WalMult20)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult20++;
            }
            int WalMult21 = 0;
            for (int i = 406; i < 413; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-30.0f, 1.0f, 50.5f + ((2.0f) * WalMult21)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult21++;
            }
            int WalMult22 = 0;
            for (int i = 413; i < 419; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-15.0f, 1.0f, 45.5f + ((2.0f) * WalMult22)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult22++;
            }
            int WalMult23 = 0;
            for (int i = 419; i < 425; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(15.0f, 1.0f, 45.5f + ((2.0f) * WalMult23)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult23++;
            }
            int WalMult24 = 0;
            for (int i = 425; i < 431; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(15.0f, 1.0f, -13.0f + ((2.0f) * WalMult24)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult24++;
            }
            int WalMult25 = 0;
            for (int i = 431; i < 437; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(30.0f, 1.0f, -20.0f + ((2.0f) * WalMult25)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult25++;
            }
            int WalMult26 = 0;
            for (int i = 437; i < 443; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-15.0f, 1.0f, -15.0f + ((2.0f) * WalMult26)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult26++;
            }
            int WalMult27 = 0;
            for (int i = 443; i < 449; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(0.0f, 1.0f, -19.0f + ((2.0f) * WalMult27)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult27++;
            }
            int WalMult28 = 0;
            for (int i = 449; i < 455; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-30.0f, 1.0f, -19.0f + ((2.0f) * WalMult28)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult28++;
            }
            int WalMult29 = 0;
            for (int i = 455; i < 471; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-18.0f, 1.0f, 5.0f + ((2.0f) * WalMult29)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult29++;
            }
            int WalMult30 = 0;
            for (int i = 471; i < 487; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(20.0f, 1.0f, 5.0f + ((2.0f) * WalMult30)));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult30++;
            }
            int WalMult31 = 0;
            for (int i = 487; i < 505; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-16.0f+ ((2.0f) * WalMult31), 1.0f, 5.0f ));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult31++;
            }
            int WalMult32 = 0;
            for (int i = 505; i < 523; i++)
            {
                newEntity = new Entity("Collision" + i);
                newEntity.AddComponent(new ComponentPosition(-16.0f + ((2.0f) * WalMult32), 1.0f, 35.0f));
                //added a radius component for the objet to collide with something
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult32++;
            }
            int WalMult33 = 0;
            for (int i =0; i<4; i++)
            {
                newEntity = new Entity("Ghost"+i);
                newEntity.AddComponent(new ComponentPosition(5.0f, 2.0f, 0.0f));
                newEntity.AddComponent(new ComponentGeometry("Geometry/Sphericals/Ghost"+i+".obj"));
                newEntity.AddComponent(new ComponentVelocity(-0.5f*(WalMult33), 0.0f, 0.0f));
                newEntity.AddComponent(new ComponentCollisionSphere(0.5f));
                entityManager.AddEntity(newEntity);
                WalMult33++;
            }

        }

        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);
            //new system added for physics on lab 4.2
            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);
            /*using addig the systemsphere like this cause of the same reason we did the close method
             on Audio it's a template, the other ISystem do not have this camRefmethod*/
            SystemCollisionSphere ColSystem = new SystemCollisionSphere();
            ColSystem.CamRef(camera);
            ColSystem.CollisionManRef(mazeCollsion);
            systemManager.AddSystem(ColSystem);

            SystemCollisionBox BoxSystem = new SystemCollisionBox();
            BoxSystem.CamBoxRef(camera);
            BoxSystem.CollisionManRef(mazeCollsion);
            systemManager.AddSystem(BoxSystem);

            newSystem = new SystemAudio();
            systemManager.AddSystem(newSystem);

        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Update(FrameEventArgs e)
        {
            dt = (float)e.Time;
            System.Console.WriteLine("fps=" + (int)(1.0 / dt));

            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed)
                sceneManager.Exit();

            // TODO: Add your update logic here
            mazeCollsion.ProcessCollisions();
            if (keysPressed[(char)Key.Up])
            {
                camera.MoveForward(0.1f);
            }
            if (keysPressed[(char)Key.Down])
            {
                camera.MoveForward(-0.1f);
            }
            if (keysPressed[(char)Key.Right])
            {
                camera.RotateY(0.05f);
            }
            if (keysPressed[(char)Key.Left])
            {
                camera.RotateY(-0.05f);
            }
            if(keysPressed[(char)Key.M])
            {
                sceneManager.ChangeScene(SceneType.GAME_OVER);
            }
            //called each frame to execute code when a collision is happening

            // cameraPos.Update(e);
            // NEW for Audio
            // Update OpenAL Listener Position and Orientation based on the camera
            AL.Listener(ALListener3f.Position, ref camera.cameraPosition);
            AL.Listener(ALListenerfv.Orientation, ref camera.cameraDirection, ref camera.cameraUp);
        }
        //increasing the score when an entity with a specific name is within collision range (see MazecoliisionmanageR)
       public void scoreUp(int pScore)
       {
            score1 = pScore;
       }
        //function that can be called to set a new position for the camera  (See camera.cs )
       public void Camera_Set_Position()
       {
            
            camera.CameraSetPosition(new Vector3 (0, 1, -5));
       }
        //stops the camera from passing through entities , Previous position is a vector is the the last position before the collision
        public void Stop_no_clip()
        {
            camera.CameraSetPosition(camera.Previous_position);
        }
       
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Action ALL systems
            systemManager.ActionSystems(entityManager);

            // Render score
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.clearColour = Color.Transparent;
            GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 2f)), "Score: "+score1, 18, StringAlignment.Near, Color.White);
            GUI.Render();

        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate -= Keyboard_KeyUp;

            //clearing the memory off objects when creating a new scene
            ResourceManager.RemoveAllAssets();
        }

        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = true;
        }
        public void Keyboard_KeyUp(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = false;
        }
    }
}

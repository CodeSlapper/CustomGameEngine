using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game.Scenes;
using OpenTK;

namespace OpenGL_Game.Managers
{
    class MazeCollisionManager : CollisionManager
    {

        int score;
        GameScene reference;
        EntityManager Entity_Reference;
        //setting the camera position back to it's original position and calling the function from GameScene when a collision occurs
        // Camera CamPOS = new Camera(new Vector3(0, 4, 7), new Vector3(0, 0, 0), 800f / 450f, 0.1f, 100f);
        //process collisions is the main function that any code executed within this body is called when a collision happens
        public override void ProcessCollisions()
        {
            foreach (Collision collision in collisionManifold)
            {
                //if the camera collides with the entity called point increase the score and remove the entity
                for(int i = 0; i<196; i++)
                {
                    if (collision.entity.Name == "Point"+i)
                    {
                        score++;
                        //increases score when collision happens
                        reference.scoreUp(score);
                        //resets the camera back to a designated place when collidiing with an entity
                        //reference.Camera_Set_Position();
                        //removes an entity on collision
                        Entity_Reference.Remove_Entity(i);
                        //reference.Stop_no_clip();
                        
                    }
                }
                for (int i =0; i < 523; i++)
                {
                    if (collision.entity.Name == "Collision"+i)
                    {
                        reference.Stop_no_clip();
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if (collision.entity.Name == "Ghost" + i)
                    {
                        reference.Camera_Set_Position();
                    }
                }



            }
            ClearManifold();
        }
        public void Entity_GameScene_Ref(EntityManager pReference)
        {
            Entity_Reference = pReference;
        }
        // we need the reference of mazecollision in GameScene not a refference of gameScene since Collision is INSIDE of gamescene
        public void MazeReference(GameScene pScene)
        {
            
            reference = pScene;
        }
    }
}

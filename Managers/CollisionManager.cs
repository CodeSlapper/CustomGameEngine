using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
   enum COLLISIONTYPE
    {
        SPHERE_SPHERE,
        BOX_SPHERE
    }
    struct Collision
    {
        public Entity entity;
        public COLLISIONTYPE collisionType;
    }

    abstract class CollisionManager
    {
        protected List<Collision> collisionManifold = new List<Collision>();
        public CollisionManager() {}
        //clear all previous collisions that happened in the last frame
        public void ClearManifold() { collisionManifold.Clear();}
        //this method is called on SystemCollisionSphere
        public void CollisionBetweenCamera(Entity entity , COLLISIONTYPE collisionType)
        {
            //if we are already  have this collision in the manifold then do not add it
            foreach (Collision coll in collisionManifold)
            {
                if (coll.entity == entity) return;
            }
            Collision collision;
            collision.entity = entity;
            collision.collisionType = collisionType;
            collisionManifold.Add(collision);
        }
        //any child class has to imnplement this method
        public abstract void ProcessCollisions();
    }

}

using OpenGL_Game.Systems;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    class ComponentCollisionBox : IComponent
    {
        double top;
        double bottom;
        double left;
        double right;
        public Box2d Box;
        public ComponentCollisionBox(double left, double top, double right, double bottom)
        {
            this.Box = new Box2d(left, top, right, bottom);
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }
        public double Top
        {
            get { return top; }
            set { top = value; }
        }
        public double Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }
        public double Left
        {
            get { return left; }
            set { left = value; }
        }
        public double Right
        {
            get { return right; }
            set { right = value; }
        }
        
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_BOX; }
        }
    }
}


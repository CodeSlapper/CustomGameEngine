using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game
{
    class Camera
    {
        public Matrix4 view, projection;
        public Vector3 cameraPosition, cameraDirection, cameraUp;
        private Vector3 targetPosition;

        //Camera radius
       public float Radius = 1.2f;

        public Vector3 Previous_position;


        public Camera()
        {
            cameraPosition = new Vector3(0.0f, 0.0f, 0.0f);
            cameraDirection = new Vector3(0.0f, 0.0f, -1.0f);
            cameraUp = new Vector3(0.0f, 1.0f, 0.0f);
            UpdateView();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), 1.0f, 0.1f, 100f);
        }

        public Camera(Vector3 cameraPos, Vector3 targetPos, float ratio, float near, float far)
        {
            cameraUp = new Vector3(0.0f, 1.0f, 0.0f);
            cameraPosition = cameraPos;
            cameraDirection = targetPos-cameraPos;
            cameraDirection.Normalize();
            UpdateView();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), ratio, near, far);
        }

        public void MoveForward(float move)
        {
            Previous_position = cameraPosition;
            cameraPosition += move*cameraDirection;
            UpdateView();
        }

        public void Translate(Vector3 move)
        {
            Previous_position = cameraPosition;
            cameraPosition += move;
            UpdateView();
        }


        public void RotateY(float angle)
        {
            cameraDirection = Matrix3.CreateRotationY(angle) * cameraDirection;
            UpdateView();
        }

        public void UpdateView()
        {
            targetPosition = cameraPosition + cameraDirection;
            view = Matrix4.LookAt(cameraPosition, targetPosition, cameraUp);
        }
        /* this function serves two purposes, one for the the no clip camera and one for the reset of the camera position to a desired position 
         the parameter is used to create a new position  and the Previous position is used to fix the xamera in place and stopping it from going through entities*/
        public void CameraSetPosition(Vector3 CamPos)
        {
            cameraPosition = CamPos;
            Previous_position = cameraPosition;
            UpdateView();
        }
    }
}

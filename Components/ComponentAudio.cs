using OpenGL_Game.Managers;
using OpenGL_Game.OBJLoader;
using System.IO;
using OpenTK.Audio.OpenAL;
using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentAudio : IComponent
    {
        int audioSource;
        public ComponentAudio (string audioname)
        {
            int audioBuffer;
            
            // NEW for Audio
            // Setup Audio Source from the Audio Buffer
            audioBuffer = ResourceManager.LoadAudio(audioname);
            audioSource = AL.GenSource();
            AL.Source(audioSource, ALSourcei.Buffer, audioBuffer); // attach the buffer to a source
            AL.Source(audioSource, ALSourceb.Looping, true); // source loops infinitely
           // sourcePosition = new Vector3(10.0f, 0.0f, 0.0f); // give the source a position
           
            AL.SourcePlay(audioSource); // play the ausio source
        }
        //need public that sets position for the audio (method)  called for system
        public void SetPosition(Vector3 emitterPosition)
        {

            AL.Source(audioSource, ALSource3f.Position, ref emitterPosition);
        }
        

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }
        public void Close()
        {
            AL.SourceStop(audioSource);
            AL.DeleteSource(audioSource);
        }

    }
}

using Audio.Core;
using Microsoft.Phone.BackgroundAudio;

namespace Audio.StorageStreamingAgent
{
    /// <summary>
    /// A background agent that performs per-track streaming for playback
    /// </summary>
    public class AudioStreamer : AudioStreamingAgent
    {
        /// <summary>
        /// Called when a new track requires audio decoding
        /// (typically because it is about to start playing)
        /// </summary>
        /// <param name="track">
        /// The track that needs audio streaming
        /// </param>
        /// <param name="streamer">
        /// The AudioStreamer object to which a MediaStreamSource should be
        /// attached to commence playback
        /// </param>
        /// <remarks>
        /// To invoke this method for a track set the Source parameter of the AudioTrack to null
        /// before setting  into the Track property of the BackgroundAudioPlayer instance
        /// property set to true;
        /// otherwise it is assumed that the system will perform all streaming
        /// and decoding
        /// </remarks>
        protected override void OnBeginStreaming(AudioTrack track, Microsoft.Phone.BackgroundAudio.AudioStreamer streamer)
        {
            streamer.SetSource(new Mp3MediaStreamSource(null));

            NotifyComplete();
        }

        /// <summary>
        /// Called when the agent request is getting cancelled
        /// The call to base.OnCancel() is necessary to release the background streaming resources
        /// </summary>
        protected override void OnCancel()
        {
            base.OnCancel();
        }
    }
}

// Specs: http://www.guthspot.se/video/deshaker.htm

namespace VrPlayer.Stabilizers.Deshaker
{
    public class DeshakerFrame
    {
        // Frame number of the source video. For interlaced video, the frame number will have an 'A' or 'B' appended, depending on the field.
        public int FrameNumber { get; set; }

        // The number of horizontal panning pixels between (the middle line of) the previous frame and current frame.
        public double PanX { get; set; }

        // The number of vertical panning pixels between (the middle line of) the previous frame and current frame.
        public double PanY { get; set; }

        // The number of degrees of rotation between (the middle line of) the previous frame and current frame.
        public double Rotation { get; set; }

        // The zoom factor between (the middle line of) the previous frame and current frame.
        public double Zoom { get; set; }

        // If rolling shutter is enabled, the number of horizontal panning pixels between (the first line of) the previous frame and current frame.
        public double PanXRS { get; set; }

        // If rolling shutter is enabled, the number of vertical panning pixels between (the first line of) the previous frame and current frame.
        public double PanYRS { get; set; }

        // If rolling shutter is enabled, the number of degrees of rotation between (the first line of) the previous frame and current frame.
        public double RotatioRS { get; set; }

        // If rolling shutter is enabled, the zoom factor between (the first line of) the previous frame and current frame.
        public double ZoomRS { get; set; }

        // Reserved for the keyword "skipped" for skipped frames.
        public bool Skipped { get; set; }

        // Reserved for the keyword "n_scene" for the first frame of a new scene.
        public bool NewScene { get; set; }

        // A comment start character ('#') will be added here by pass 1, because the remaining columns are not used by pass 2. They are only for your information.
        public string CommentStart { get; set; }

        // The calculated scene detection value for this frame.
        public double SceneDetectionValue { get; set; }

        // How many blocks (in percent) that were ok in this frame.
        public double PercentOkBlocks { get; set; }

        // Reserved for the keywork "blank", which indicates a blank frame.
        public bool Blank { get; set; }
    }
}

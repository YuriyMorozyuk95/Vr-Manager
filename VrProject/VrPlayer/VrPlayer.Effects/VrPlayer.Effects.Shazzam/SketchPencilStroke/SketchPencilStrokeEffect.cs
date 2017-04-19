using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.Shazzam.SketchPencilStroke
{
    [DataContract]
    public class SketchPencilStrokeEffect : EffectBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("inputSampler", typeof(SketchPencilStrokeEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty BrushSizeProperty =
            DependencyProperty.Register("brushSize", typeof(double), typeof(SketchPencilStrokeEffect), new UIPropertyMetadata(0.005D, PixelShaderConstantCallback(0)));
        [DataMember]
        public double BrushSize
        {
            get { return ((double)(GetValue(BrushSizeProperty))); }
            set { SetValue(BrushSizeProperty, value); }
        }

        public SketchPencilStrokeEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.Shazzam",
                "SketchPencilStroke/SketchPencilStrokeEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(BrushSizeProperty);
        }
    }
}

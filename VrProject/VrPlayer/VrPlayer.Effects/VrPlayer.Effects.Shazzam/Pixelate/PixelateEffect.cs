using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.Shazzam.Pixelate
{
    [DataContract]
    public class PixelateEffect : EffectBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("inputSampler", typeof(PixelateEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty PixelCountsProperty = 
            DependencyProperty.Register("PixelCounts", typeof(Size), typeof(PixelateEffect), new UIPropertyMetadata(new Size(60D, 40D), PixelShaderConstantCallback(0)));
        public Size PixelCounts
        {
            get { return ((Size)(GetValue(PixelCountsProperty))); }
            set { SetValue(PixelCountsProperty, value); }
        }

        public static readonly DependencyProperty BrickOffsetProperty =
            DependencyProperty.Register("BrickOffset", typeof(double), typeof(PixelateEffect), new UIPropertyMetadata(0D, PixelShaderConstantCallback(1)));
        [DataMember]
        public double BrickOffset
        {
            get { return ((double)(GetValue(BrickOffsetProperty))); }
            set { SetValue(BrickOffsetProperty, value); }
        }

        public PixelateEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.Shazzam",
                "Pixelate/PixelateEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(PixelCountsProperty);
            UpdateShaderValue(BrickOffsetProperty);
        }
    }
}

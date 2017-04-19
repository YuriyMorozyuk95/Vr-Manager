using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.Anaglyph
{
    [DataContract]
    public class AnaglyphEffect : EffectBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(AnaglyphEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty StereoModeProperty =
            DependencyProperty.Register("StereoMode", typeof(double), typeof(AnaglyphEffect), new UIPropertyMetadata(2.0D, PixelShaderConstantCallback(0)));
        [DataMember]
        public double StereoMode
        {
            get { return ((double)(GetValue(StereoModeProperty))); }
            set { SetValue(StereoModeProperty, value); }
        }

        public static readonly DependencyProperty AnaglyphModeProperty =
            DependencyProperty.Register("AnaglyphMode", typeof(double), typeof(AnaglyphEffect), new UIPropertyMetadata(0.0D, PixelShaderConstantCallback(1)));
        [DataMember]
        public double AnaglyphMode
        {
            get { return ((double)(GetValue(AnaglyphModeProperty))); }
            set { SetValue(AnaglyphModeProperty, value); }
        }

        public AnaglyphEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.Anaglyph",
                "AnaglyphEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(StereoModeProperty);
        }
	}
}

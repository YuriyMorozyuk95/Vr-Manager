using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.DepthMapOverUnder
{
    [DataContract]
    public class DepthMapOverUnderEffect : EffectBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(DepthMapOverUnderEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }    
        
        public static readonly DependencyProperty MaxOffsetProperty =
            DependencyProperty.Register("MaxOffset", typeof(double), typeof(DepthMapOverUnderEffect), new UIPropertyMetadata(0D, PixelShaderConstantCallback(0)));
        [DataMember]
        public double MaxOffset
        {
            get { return ((double)(GetValue(MaxOffsetProperty))); }
            set { SetValue(MaxOffsetProperty, value); }
        }

        public DepthMapOverUnderEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.DepthMapOverUnder",
                "DepthMapOverUnderEffect.ps"), UriKind.RelativeOrAbsolute);
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(MaxOffsetProperty);
        }
    }
}

using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

using VrPlayer.Contracts.Effects;

namespace VrPlayer.Effects.DepthMapSbs
{
    [DataContract]
    public class DepthMapSbsEffect : EffectBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(DepthMapSbsEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }
    
        public static readonly DependencyProperty MaxOffsetProperty =
            DependencyProperty.Register("MaxOffset", typeof(double), typeof(DepthMapSbsEffect), new UIPropertyMetadata(0D, PixelShaderConstantCallback(0)));
        [DataMember]
        public double MaxOffset
        {
            get { return ((double)(GetValue(MaxOffsetProperty))); }
            set { SetValue(MaxOffsetProperty, value); }
        }

        public DepthMapSbsEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Effects.DepthMapSbs",
                "DepthMapSbsEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(MaxOffsetProperty);
        }
    }
}

using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

using VrPlayer.Contracts.Distortions;

namespace VrPlayer.Distortions.Pincushion
{
    [DataContract]
    public class PincushionEffect : DistortionBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(PincushionEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }
        
        public static readonly DependencyProperty FactorProperty =
            DependencyProperty.Register("Factor", typeof(double), typeof(PincushionEffect), new UIPropertyMetadata(5D, PixelShaderConstantCallback(0)));       
        [DataMember]
        public double Factor
        {
            get { return ((double)(GetValue(FactorProperty))); }
            set { SetValue(FactorProperty, value); }
        }

        public PincushionEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Distortions.Pincushion",
                "PincushionEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(FactorProperty);
        }
    }
}
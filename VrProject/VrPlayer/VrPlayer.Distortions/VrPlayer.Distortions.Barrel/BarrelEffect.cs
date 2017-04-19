using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

using VrPlayer.Contracts.Distortions;

namespace VrPlayer.Distortions.Barrel
{
    [DataContract]
    public class BarrelEffect : DistortionBase
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(BarrelEffect), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty FactorProperty =
            DependencyProperty.Register("Factor", typeof(double), typeof(BarrelEffect), new UIPropertyMetadata(1.8D, PixelShaderConstantCallback(0)));
        [DataMember]
        public double Factor
        {
            get { return ((double)(GetValue(FactorProperty))); }
            set { SetValue(FactorProperty, value); }
        }

        public static readonly DependencyProperty XCenterProperty =
            DependencyProperty.Register("XCenter", typeof(double), typeof(BarrelEffect), new UIPropertyMetadata(0.5D, PixelShaderConstantCallback(1)));
        [DataMember]
        public double XCenter
        {
            get { return ((double)(GetValue(XCenterProperty))); }
            set { SetValue(XCenterProperty, value); }
        }

        public static readonly DependencyProperty YCenterProperty =
            DependencyProperty.Register("YCenter", typeof(double), typeof(BarrelEffect), new UIPropertyMetadata(0.5D, PixelShaderConstantCallback(2)));
        [DataMember]
        public double YCenter
        {
            get { return ((double)(GetValue(YCenterProperty))); }
            set { SetValue(YCenterProperty, value); }
        }

        public static readonly DependencyProperty BlueOffsetProperty =
            DependencyProperty.Register("BlueOffset", typeof(double), typeof(BarrelEffect), new UIPropertyMetadata(0D, PixelShaderConstantCallback(3)));
        [DataMember]
        public double BlueOffset
        {
            get { return ((double)(GetValue(BlueOffsetProperty))); }
            set { SetValue(BlueOffsetProperty, value); }
        }

        public static readonly DependencyProperty RedOffsetProperty =
            DependencyProperty.Register("RedOffset", typeof(double), typeof(BarrelEffect), new UIPropertyMetadata(0D, PixelShaderConstantCallback(4)));
        [DataMember]
        public double RedOffset
        {
            get { return ((double)(GetValue(RedOffsetProperty))); }
            set { SetValue(RedOffsetProperty, value); }
        }

        public BarrelEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Distortions.Barrel",
                "BarrelEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(FactorProperty);
            UpdateShaderValue(XCenterProperty);
            UpdateShaderValue(YCenterProperty);
            UpdateShaderValue(BlueOffsetProperty);
            UpdateShaderValue(RedOffsetProperty);
         }        
    }
}
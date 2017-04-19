/// <class>InvertColorEffect</class>
/// <description>An effect that inverts all colors.</description>

sampler2D inputSampler : register(S0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float4 color = tex2D( inputSampler, uv );
   float4 invertedColor = float4(color.a - color.rgb, color.a);
   return invertedColor;
}
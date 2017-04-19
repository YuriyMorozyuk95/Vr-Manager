//Tutorial: http://www.codeproject.com/Articles/32638/Anaglyph-ShaderEffect-in-WPF

sampler2D input : register(s0);

float StereoMode : register(C0);
float AnaglyphMode : register(C1);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float2 Uv1;
	float2 Uv2;
	
	if(StereoMode == 1)
  {
  	Uv1 = float2(uv.x,uv.y/2);
    Uv2 = float2(uv.x,uv.y/2 + 0.5);
  }
  else if(StereoMode == 2)
  {
  	Uv1 = float2(uv.x/2,uv.y);
    Uv2 = float2(uv.x/2 + 0.5,uv.y);
  }
  else
  {
  	Uv1 = float2(uv.x,uv.y);
    Uv2 = float2(uv.x,uv.y);
  }  

	float4 Color1; 
  float4 Color2; 

	if(AnaglyphMode == 1)
 	{
  	Color1 = tex2D( input , Uv1); 
  	Color2 = tex2D( input , Uv2); 
  }
  else
  {
  	Color2 = tex2D( input , Uv1); 
  	Color1 = tex2D( input , Uv2); 
  }
  
  Color1.r = Color2.r; 
  Color1.g = Color1.g; 
  Color1.b = Color1.b; 
  Color1.a = max(Color1.a,Color2.a);

  return Color1;
}
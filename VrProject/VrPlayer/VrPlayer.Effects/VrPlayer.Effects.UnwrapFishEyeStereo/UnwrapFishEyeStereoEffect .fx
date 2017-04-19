sampler2D input : register(s0);

// new HLSL shader
// modify the comment parameters to reflect your shader parameters

/// <summary>Explain the purpose of this variable.</summary>
/// <minValue>05/minValue>
/// <maxValue>10</maxValue>
/// <defaultValue>3.5</defaultValue>
float SampleInputParam : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	static const float PI = 3.14159265;
	float fsin = sin(uv.y*PI);
	
	float2 coord;
	
	if(uv.x < 0.5)
	{
  	coord = float2(fsin * (uv.x - 0.25) + 0.25, uv.y);
	}
  else
  {
  	coord = float2(fsin * (uv.x - 0.75) + 0.75, uv.y);
	}  
	
	float4 color; 
	color= tex2D( input, coord); 

	return color; 
}
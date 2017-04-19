sampler2D input : register(s0);

float MaxOffset : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR 
{
  if(uv.x < MaxOffset / 2 || uv.x > 1 - MaxOffset / 2)
 	{
    return float4(0,0,0,1);
  }
 
  
  if( uv.y < 0.5)
  {
  	uv = float2(uv.x - MaxOffset / 2, uv.y);

  	//Depth
		float2 depthMapUV = float2(uv.x,uv.y + 0.5);
  	float4 depthMap = tex2D(input, depthMapUV);
  	float depth = depthMap.r;  
	
		float2 colorMapUV = float2(uv.x + MaxOffset * depth, uv.y);
  	float4 colorMap = tex2D(input, colorMapUV);
  	return colorMap;
	}
	else
  {
  	uv = float2(uv.x + MaxOffset / 2, uv.y);
  
    //Depth
		float2 depthMapUV = float2(uv.x,uv.y);
  	float4 depthMap = tex2D(input, depthMapUV);
  	float depth = depthMap.r;
		
		float2 colorMapUV = float2(uv.x - MaxOffset * depth, uv.y - 0.5);
  	float4 colorMap = tex2D(input, colorMapUV);
  	return colorMap;
  }  
}
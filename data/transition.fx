sampler s0 : register(s0);
float TransitionDelta : register(c0);

float4 mainPlain(float4 position : SV_Position, float4 col : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
	return float4(0, 0, 0, (TransitionDelta < tex2D(s0, uv).r));
}

technique Plain
{
    pass Pass1
    {
        PixelShader = compile ps_4_0 mainPlain();
    }
}

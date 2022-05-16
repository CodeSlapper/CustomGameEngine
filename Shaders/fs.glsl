#version 330
 
in vec2 v_TexCoord;
in vec3 v_Normal;
in vec3 v_FragPos;
uniform sampler2D s_texture;
uniform vec3 v_diffuse;	// OBJ NEW

out vec4 Color;

struct SpotLight
{
	vec3 lightPos;
	vec4 lightAmbient;
	vec3 lightColor;
	vec4 Light_Direction;
	vec3 Diffuse_Light;
	float cutOff;
};
uniform SpotLight S_Light [4];

struct BasicLight
{
	vec3 lightPos;
	vec4 lightAmbient;
	vec3 lightColor;
	vec4 Light_Direction;
	vec3 Diffuse_Light;
	float cutOff;
};
uniform BasicLight  B_Light [2];

//NOTES: removed the attenuation from the spotlight to make them stand out from the basic light

void main()
{
	for (int i=0; i<4; i++)
	{
		vec3 norm = normalize(v_Normal);
		vec3 lightDir = normalize(S_Light[i].lightPos - v_FragPos); 
				float  Distance = length(B_Light[i].lightPos -v_FragPos);
		float Attenuation= 1.0f / (1.0f + 0.005f * Distance * 0.0075f *(Distance * Distance));
		float theta = acos(dot(lightDir.xyz , normalize(-S_Light[i].Light_Direction.xyz)));

		if (theta < 0.2f)
		{
			float diff = max(dot(norm, lightDir), 0.0);
			vec3 diffuse = diff * S_Light[i].lightColor;
			Color =Color+ vec4((S_Light[i].lightAmbient) + (vec4(v_diffuse, 1) * texture2D(s_texture, v_TexCoord) * vec4(diffuse, 0)));  // OBJ CHANGED
		}
		else
		{
			Color =Color+ vec4(S_Light[i].lightAmbient.xyz * vec3(S_Light[i].Diffuse_Light.xyz), 1.0);
		}
	}

	for (int i=0; i<2; i++)
	{
		float  Distance = length(B_Light[i].lightPos -v_FragPos);
		//original attenuation calculation was multiplied by 0.0075 but changed to  0.000005f to increase the alltitude of the the light position to illuminate the entire stage
		float Attenuation= 1.0f / (1.0f + 0.05f * Distance * 0.000005f *(Distance * Distance));
		
		vec3 norm = normalize(v_Normal);
		vec3 lightDir = normalize(B_Light[i].lightPos - v_FragPos); 
		float diff = max(dot(norm, lightDir), 0.0);
		vec3 diffuse = diff * B_Light[i].lightColor;
		Color = Color+ vec4( (B_Light[i].lightAmbient*Attenuation) + (vec4(v_diffuse*Attenuation, 1) * texture2D(s_texture, v_TexCoord) * vec4(diffuse*Attenuation, 0)));  // OBJ CHANGED
	}


}
#version 330 core

in vec3 vs_out_normal;
in vec3 vs_out_world_pos;

out vec4 frag_color;

uniform vec4 color;

uniform vec3 light_pos;
uniform vec4 light_color;
uniform vec3 view_pos;

float ambientStrength = 0.1;
float specularStrength = 0.5;

void main()
{
	vec3 normal = normalize(vs_out_normal);
	vec3 light_dir = normalize(light_pos - vs_out_world_pos);

	// Ambient component
	vec3 ambient = ambientStrength * light_color.xyz;

	// Diffuse component
	float diff = max(dot(normal, light_dir), 0.0);
	vec3 diffuse = diff * light_color.xyz;

	// Specular component
	vec3 view_dir = normalize(view_pos - vs_out_world_pos);
	vec3 reflect_dir = reflect(-light_dir, normal);
	float spec = pow(max(dot(view_dir, reflect_dir), 0.0), 32);
	vec3 specular = specularStrength * spec * light_color.xyz;

	// Final result
	vec3 result = (ambient + diffuse + specular) * color.xyz;
	frag_color = vec4(result, 1.0);
}
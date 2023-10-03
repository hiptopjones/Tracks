#version 330 core

in vec2 vTexCoord;
out vec4 fragColor;

uniform sampler2DArray ourTexture;
uniform int layer;
uniform vec4 color;

void main()
{
    fragColor = color * texture(ourTexture, vec3(vTexCoord, layer));
}
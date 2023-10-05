#version 330 core

in vec2 vTexCoord;
out vec4 fragColor;

uniform sampler2D ourTexture;
uniform vec4 color;

void main()
{
    fragColor = color * texture(ourTexture, vTexCoord);
}
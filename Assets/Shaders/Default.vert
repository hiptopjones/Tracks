﻿#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;
layout (location = 2) in vec2 aTexCoord;

out vec3 vColor;
out vec2 vTexCoord;

uniform mat4 transform;

void main()
{
    gl_Position = vec4(aPosition, 1.0) * transform;
    vColor = aColor;
    vTexCoord = aTexCoord;
}
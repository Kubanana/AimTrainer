#version 330 core

in vec2 vUv;
out vec4 FragColor;

uniform sampler2D uTexture;

void main()
{
    vec4 tex = texture(uTexture, vUv);
    FragColor = tex;
}
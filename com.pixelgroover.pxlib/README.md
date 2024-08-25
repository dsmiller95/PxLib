# Pixelgroover PxLib 

A suite of tools useful when kickstarting a new unity project. Of particular note:

- PxAnimator + Anim
	- a handy way to set up and then play frame-based animations
- PxAudioPlayer
	- a simple audio player set up to handle background music and sound effects.
	- includes background music blending
- PxSpriteShader, PxShaderController, PxShaderEffect, PxShaderEffectCollector
	- an image shader with many useful coloring options
	- attaching a PxShaderEffectCollector and multiple PxShaderEffects allows granular control over specific shader effects by toggling individual PxShaderEffect objects on and off
	- In-progress compatibility with unity's UI via PxImageShader
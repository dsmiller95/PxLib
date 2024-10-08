# [Pixelgroover PxLib](com.pixelgroover.pxlib/README.md)

# Summary

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



# Installation


## Install via Git Url
Requires a version of unity that supports path query parameter for git packages (Unity >= 2019.3.4f1, Unity >= 2020.1a21).
Add `https://github.com/dsmiller95/PxLib.git?path=com.pixelgroover.pxlib#0.1.0` to Package Manager. replace "0.1.0" with the
current version, or omit to pin to the latest version at time of install.

or add `"com.pixelgroover.pxlib": "https://github.com/dsmiller95/PxLib.git?path=com.pixelgroover.pxlib#0.1.0"` to Packages/manifest.json.


## Install via Openupm

install the openupm cli: https://openupm.com/

run `openupm add com.pixelgroover.pxlib` at your project root to install the latest version.


## Install for Local Development

To install while allowing changes to the source code, clone the repository and add it as a filepath reference.
Clone the repository as a sibling to your unity project's folder. If the unity project is at `C:\source\Game-Dev\PixelPlatformer`, then clone the PxLib repository into `C:\source\Game-Dev\PxLib` . For example:

```sh
git clone PxLib git@github.com:dsmiller95/PxLib.git
```

Then add `"com.pixelgroover.pxlib": "file:../../PxLib/com.pixelgroover.pxlib"` to your unity project's package manifest at `Packages/manifest.json`.
This will reference the cloned repository via a relative filepath.

Alternatively, use the unity package manager window. Add a new package, and select "Add package from disk..." . Navigate to `\PxLib\com.pixelgroover.pxlib\` and select the `package.json` file in that folder.

At this point, configure your preferences under External Tools to generate .csproj files for this Local package. This is so that your preferred code editor includes the PxLib source files in the project when you open it, making them easier to edit.

Make any changes as required to the PxLib library, then optionally submit your changes up to this repository via a fork.

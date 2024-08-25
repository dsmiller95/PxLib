# [README](com.pixelgroover.pxlib/README.md)


# Installation

## Install via git url
Requires a version of unity that supports path query parameter for git packages (Unity >= 2019.3.4f1, Unity >= 2020.1a21). You can add https://github.com/dsmiller95/PxLib.git?path=com.pixelgroover.pxlib to Package Manager

or add "com.pixelgroover.pxlib": "https://github.com/dsmiller95/PxLib.git?path=com.pixelgroover.pxlib" to Packages/manifest.json.

## Install via openupm

install the openupm cli: https://openupm.com/

run `openupm add com.pixelgroover.pxlib` at your project root


## Installation for local development

Clone the repository as a sibling to your unity project folder. If your unity project is at `C:\source\Game-Dev\PixelPlatformer`, then clone PxLib into `C:\source\Game-Dev\PxLib` . For example:

```sh
git clone PxLib TODOGitRef
```

Then add "com.pixelgroover.pxlib": "../../PxLib/com.pixelgroover.pxlib" to your unity project's Packages/manifest.json.

Make any changes as required to the pxLib library, then optionally submit your changes up to this repository via a fork.
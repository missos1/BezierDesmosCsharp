# BezierDesmosCsharp

A C# backend pipeline that traces images into Bézier curves so you can render them in Desmos. 

Inspired by junferno's [DesmosBezierRenderer](https://github.com/kevinjycui/DesmosBezierRenderer).

## How it works

1. Takes an image input.
2. Parse it through **Potrace** to trace the shapes.
(Optional) Uses **OpenCV** canny edge detection to find the edges and trace the shapes on non BW images.
3. Passes that data through [**BitmapToVector**](https://github.com/daltonks/BitmapToVector) to generate Bézier curves.
4. Converts those curves into a list of LaTeX strings.
5. Sends the LaTeX strings to the frontend via HTTP POST so Desmos can draw it.

## How to run

### Option 1: The easy way
Download the latest release directly from the [Releases](../../releases) page, extract and run the `.exe`.

### Option 2: Build from source
Make sure you have the .NET SDK installed and your OpenCV, BitmapToVector wrappers set up.

```bash
# Clone it
git clone https://github.com/missos1/BezierDesmosCsharp
cd BezierDesmosCsharp

# Install dependencies
dotnet restore

# Run the backend
dotnet run

﻿// --------------------------------------------------------------------------------------
// Builds the documentation from `.fsx` and `.md` files in the 'docsrc/content' directory
// (the generated documentation is stored in the 'docs' directory)
// --------------------------------------------------------------------------------------

#r "paket: groupref generate //"
#load "./.fake/generate.ja.fsx/intellisense.fsx"

// Binaries that have XML documentation (in a corresponding generated XML file)
let referenceBinaries = [ "../../../artifacts/bin/fcs/Release/net461/FSharp.Compiler.Service.dll" ]

// Specify more information about your project
let info =
  [ "project-name", "F# Compiler Services"
    "project-author", "Microsoft Corporation, Dave Thomas, Anh-Dung Phan, Tomas Petricek"
    "project-summary", "F# compiler services for creating IDE tools, language extensions and for F# embedding"
    "project-github", "https://github.com/fsharp/FSharp.Compiler.Service"
    "project-nuget", "https://www.nuget.org/packages/FSharp.Compiler.Service" ]

// --------------------------------------------------------------------------------------
// For typical project, no changes are needed below
// --------------------------------------------------------------------------------------

open Fake.IO.FileSystemOperators
open Fake.IO
open Fake.Core
open FSharp.Formatting.Razor

// When called from 'build.fsx', use the public project URL as <root>
// otherwise, use the current 'output' directory.
let root = "."

// Paths with template/source/output locations
let bin         = __SOURCE_DIRECTORY__ @@ "../../../release/fcs/netcoreapp3.0"
let content     = __SOURCE_DIRECTORY__ @@ "../content/ja"
let outputJa    = __SOURCE_DIRECTORY__ @@ "../../../docs/ja"
let files       = __SOURCE_DIRECTORY__ @@ "../files"
let templates   = __SOURCE_DIRECTORY__ @@ "templates/ja"
let formatting  = __SOURCE_DIRECTORY__ @@ "../../packages/generate/FSharp.Formatting"
let docTemplate = formatting @@ "templates/docpage.cshtml"

// Where to look for *.csproj templates (in this order)
let layoutRoots =
  [ templates
    formatting @@ "templates"
    formatting @@ "templates/reference"]

// Copy static files and CSS + JS from F# Formatting
// Build documentation from `fsx` and `md` files in `docsrc/content`
let buildDocumentation () =
  for dir in [content] do
    let sub = if dir.Length > content.Length then dir.Substring(content.Length + 1) else "."
    RazorLiterate.ProcessDirectory
      ( dir, docTemplate, outputJa @@ sub, replacements = ("root", root)::info,
        layoutRoots = layoutRoots, generateAnchors = true, processRecursive=false )

// Generate
buildDocumentation()

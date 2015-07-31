using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly:Addin (
    "ClassDiagramAddin",
    Namespace = "ClassDiagramAddin",
	Version = "1.0"
)]

[assembly:AddinName ("ClassDiagramAddin")]
[assembly:AddinCategory ("ClassDiagramAddin")]
[assembly:AddinDescription ("ClassDiagramAddin")]
[assembly:AddinAuthor ("Pranav Ramarao")]

[assembly:AddinDependency ("::MonoDevelop.Core", MonoDevelop.BuildInfo.Version)]
[assembly:AddinDependency ("::MonoDevelop.Ide", MonoDevelop.BuildInfo.Version)]
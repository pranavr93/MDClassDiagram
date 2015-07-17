using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly:Addin (
	"FirstAddin",
	Namespace = "FirstAddin",
	Version = "1.0"
)]

[assembly:AddinName ("FirstAddin")]
[assembly:AddinCategory ("FirstAddin")]
[assembly:AddinDescription ("FirstAddin")]
[assembly:AddinAuthor ("Michael Hutchinson")]

[assembly:AddinDependency ("::MonoDevelop.Core", MonoDevelop.BuildInfo.Version)]
[assembly:AddinDependency ("::MonoDevelop.Ide", MonoDevelop.BuildInfo.Version)]
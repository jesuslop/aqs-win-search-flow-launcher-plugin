# AQS-win-search-flow-launcher-plugin
Alpha version for a [Flow Launcher](https://www.flowlauncher.com/) plugin for searching in Windows Search using [Advanced Query Syntax](https://learn.microsoft.com/en-us/windows/win32/lwef/-search-2x-wds-aqsreference). This syntax is already understood by Windows Explorer (search field). This plugin makes these queries work also in Flow Launcher.

# AQS examples

Examples of AQS queries:

`"social security" `(exact match), Flow Launcher is not supporting this to this day.

`author:(john OR joanne) foldername:mydocuments`

`kind:music date:past month`

# AQS Localization

While the AQS examples use English search tokens (`author`, or `kind`) in English-localized Windows setups, Windows targeting other languages use localized versions of the tokens. For instance for Spanish langauge Windows, the tokens can be (`autores`, `carpeta`, etc.) Same with date specifications and so on.

# Status

This is a proof of concept, alfa version of the plugin, presently not being applied to Flow Laucher plugin store. I wrote this for personal use, and include a local deployment scrip. I am donating hereby the code for anyone wanting to run the extra mile and put the extra work needed to make this publishable in the store, and/or invite core plugin developers to incorporate any (or all) the code they may see fit into the current File Explorer plugin. 

# Code specifics

Querying Windows Search index is well supported by OS-provided SQL-type DB query interfaces. The plugin is .net based and has a COM reference to ADO-DB components for that. This should be similar to funtionality already existing in core File Explorer pluging.

Dealing with windows search AQS (abstract query syntax) is not that 100% easy. One can find in `nuget` package repositories a package called `tlbimp-Microsoft.Search.Interop` that is what's needed to eventually instantiate `GenerateSQLFromUserQuery` that crucially transforms a user-given query in AQS into a formal SQL query to run against Windows Search indexer (an step that must be already happening in File Explorer plugin). 

So basically File Explorer plugin could support AQS syntax simply by front-running the user query, making in pass through the `GenerateSQLFromUserQuery` and use the result as the windows-search DB query crunching that sould be already in place.

The `tlbimp-Microsoft.Search.Interop` is warned to be old.

# Scope

The plugin is tested in Windows 10. Windows 11 support is untested.










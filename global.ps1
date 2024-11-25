Import-Module -Name "$PSScriptRoot\..\common\common.psm1"

$plugin_name = "WindowsSearchNext"
$flowlauncher_exe_path="${env:USERPROFILE}\AppData\Local\FlowLauncher\Flow.Launcher.exe"
$flowlauncher_plugin_path="${env:USERPROFILE}\AppData\Roaming\FlowLauncher\Plugins"
$plugin_path="$flowlauncher_plugin_path\$plugin_name"


function install() {
	$fl_procs=Get-Process | Where-Object { $_.ProcessName -like "*Flow.Launcher*" }
	
	# if running, stop the flow launcher process (assumes at most one istance)
	if ($fl_procs) {
		Stop-Process -Id $fl_procs[0].Id
		write-output "Flow launcher stopped"
	}
	if (test-path $plugin_path) {
		remove-item $plugin_path -recurse -force 
	}
	copy-item "$PSScriptRoot\bin\Debug" -recurse -destination $plugin_path
	write-output "Plugin files deployed to Flow Launcher plugin dir"
	
	# if stopped, then relaunch
	if ($fl_procs) {
		Start-Process -NoNewWindow $flowlauncher_exe_path
		write-output "Flow Launcher restarted"
	}
}

function uninstall() {
	$fl_procs=Get-Process | Where-Object { $_.ProcessName -like "*Flow.Launcher*" }

	# if running, stop the flow launcher process (assumes at most one istance)
	if ($fl_procs) {
		Stop-Process -Id $fl_procs[0].Id
		write-output "Flow launcher stopped"
	}
	if (test-path $plugin_path) {
		remove-item $plugin_path -recurse -force
		write-output "Plugin removed from Flow Launcher plugins dir"
	}
	# if stopped, then relaunch
	if ($fl_procs) {
		Start-Process -NoNewWindow $flowlauncher_exe_path
		write-output "Flow Launcher restarted"
	}
}
 
if ($args.count -ne 1) {
	write-error "Bad arguments. syntax: global [install|uninstall]"
	exit 1
}

switch ($args[0].ToLower())
{
	"install" { install }
	"uninstall" { uninstall }
	default { write-error "Bad arguments. syntax: global [install|uninstall]" }
}


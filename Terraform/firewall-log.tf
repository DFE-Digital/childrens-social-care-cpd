resource "azurerm_monitor_diagnostic_setting" "firewall-diagnostics" {
  name                       = var.fw_diag_name[terraform.workspace]
  target_resource_id         = azurerm_application_gateway.appgw.id
  log_analytics_workspace_id = data.azurerm_log_analytics_workspace.log-analytics-ws.id

  count = terraform.workspace == "Prod" || terraform.workspace == "Load-Test" ? 1 : 0

  enabled_log {
    category = "ApplicationGatewayAccessLog"
  }

  enabled_log {
    category = "ApplicationGatewayFirewallLog"
  }

  enabled_log {
    category = "ApplicationGatewayPerformanceLog"
  }
}

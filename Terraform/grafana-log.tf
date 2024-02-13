resource "azurerm_monitor_diagnostic_setting" "grafana-diagnostics" {
  name                       = var.gf_diag_name[terraform.workspace]
  target_resource_id         = azurerm_linux_web_app.grafana-web-app.id
  log_analytics_workspace_id = data.azurerm_log_analytics_workspace.log-analytics-ws.id

  metric {
    category = "AllMetrics"
    enabled  = true
  }

  enabled_log {
    category = "HTTPLogs"
  }

  enabled_log {
    category = "AppServiceConsoleLogs"
  }

  enabled_log {
    category = "AppServiceAppLogs"
  }
}

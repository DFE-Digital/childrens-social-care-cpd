resource "azurerm_monitor_diagnostic_setting" "firewall-diagnostics" {
  name                       = "firewall-diagnostics"
  target_resource_id         = azurerm_application_gateway.appgw.id
  log_analytics_workspace_id = data.azurerm_log_analytics_workspace.log-analytics-ws.id

  enabled_log {
    category = "ApplicationGatewayAccessLog"
    retention_policy {
      enabled = true
      days    = 7
    }
  }

  enabled_log {
    category = "ApplicationGatewayPerformanceLog"
    retention_policy {
      enabled = true
      days    = 7
    }
  }

  enabled_log {
    category = "ApplicationGatewayFirewallLog"
    retention_policy {
      enabled = true
      days    = 7
    }
  }

  metric {
    category = "AllMetrics"
    enabled  = true
    retention_policy {
      enabled = true
      days    = 7
    }
  }
}

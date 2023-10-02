resource "azurerm_monitor_diagnostic_setting" "firewall-diagnostics2" {
  name                       = "${var.fw_diag_name[terraform.workspace]}2"
  target_resource_id         = azurerm_application_gateway.appgw.id
  log_analytics_workspace_id = data.azurerm_log_analytics_workspace.log-analytics-ws.id

  count = terraform.workspace == "Prod" || terraform.workspace == "Load-Test" ? 1 : 0


  enabled_log {
    category = "ApplicationGatewayAccessLog"
    retention_policy {
      enabled = false
    }
  }

  enabled_log {
    category = "ApplicationGatewayPerformanceLog"
    retention_policy {
      enabled = false
    }
  }

  enabled_log {
    category = "ApplicationGatewayFirewallLog"
    retention_policy {
      enabled = false
    }
  }

  metric {
    category = "AllMetrics"
    enabled  = true
    retention_policy {
      enabled = false
    }
  }
}

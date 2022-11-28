resource "azurerm_log_analytics_workspace" "log-analytics-ws" {
  name                = var.log_analytics_ws_name[terraform.workspace]
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_application_insights" "appinsights" {
  name                = var.appinsights_name[terraform.workspace]
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  workspace_id        = azurerm_log_analytics_workspace.log-analytics-ws.id
  application_type    = "web"
}

output "instrumentation_key" {
  value = nonsensitive(azurerm_application_insights.appinsights.instrumentation_key)
}

output "app_id" {
  value = azurerm_application_insights.appinsights.app_id
}
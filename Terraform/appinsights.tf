data "azurerm_log_analytics_workspace" "log-analytics-ws" {
  name                = var.log_analytics_ws_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  # location            = data.azurerm_resource_group.rg.location
  # sku                 = "PerGB2018"
  # retention_in_days   = 30
  # tags                = data.azurerm_resource_group.rg.tags
}

data "azurerm_application_insights" "appinsights" {
  name                = var.appinsights_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  # location            = data.azurerm_resource_group.rg.location
  # workspace_id        = azurerm_log_analytics_workspace.log-analytics-ws.id
  # application_type    = "web"
  # tags                = data.azurerm_resource_group.rg.tags
}
